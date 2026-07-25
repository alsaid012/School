using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Students;
using SchoolERP.Application.DTOs.Subjects;
using SchoolERP.Application.DTOs.Teachers;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📖  خدمة المواد الدراسية (SubjectService)
    /// 📌  الوظيفة: تنفيذ عمليات إدارة المواد الدراسية
    /// 📦  الاستخدام: في SubjectsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class SubjectService : ISubjectService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SubjectService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public SubjectService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SubjectService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ جلب البيانات ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع المواد الدراسية
        /// </summary>
        public async Task<ResponseDto<IEnumerable<SubjectDto>>> GetAllAsync()
        {
            try
            {
                var subjects = await _unitOfWork.Subjects.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<SubjectDto>>(subjects);

                foreach (var dto in dtos)
                {
                    // جلب اسم الصف
                    var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(dto.GradeLevelId);
                    dto.GradeLevelName = gradeLevel?.GradeName;

                    // جلب اسم المدرسة
                    if (gradeLevel != null)
                    {
                        var school = await _unitOfWork.SchoolRepository.GetByIdAsync(gradeLevel.SchoolId);
                        dto.SchoolName = school?.SchoolName;
                    }

                    // جلب عدد المعلمين
                    var teacherSubjects = await _unitOfWork.TeacherSubjects
                        .FindAsync(ts => ts.SubjectId == dto.Id);
                    dto.TeachersCount = teacherSubjects.Count();

                    // جلب عدد الطلاب
                    var students = await _unitOfWork.Students
                        .FindAsync(s => s.ClassRoom != null && s.ClassRoom.GradeLevelId == dto.GradeLevelId);
                    dto.StudentsCount = students.Count();
                }

                _logger.LogInformation("تم جلب {Count} مادة دراسية", dtos.Count());
                return ResponseDto<IEnumerable<SubjectDto>>.Ok(dtos, "تم جلب المواد الدراسية بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetAllAsync");
                return ResponseDto<IEnumerable<SubjectDto>>.Fail("حدث خطأ أثناء جلب المواد", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على مادة بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<SubjectDetailsDto>> GetByIdAsync(int id)
        {
            try
            {
                var subject = await _unitOfWork.Subjects.GetByIdAsync(id);
                if (subject == null)
                    return ResponseDto<SubjectDetailsDto>.NotFound("المادة غير موجودة");

                var dto = _mapper.Map<SubjectDetailsDto>(subject);

                // جلب اسم الصف
                var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
                dto.GradeLevelName = gradeLevel?.GradeName;

                // جلب اسم المدرسة
                if (gradeLevel != null)
                {
                    var school = await _unitOfWork.SchoolRepository.GetByIdAsync(gradeLevel.SchoolId);
                    dto.SchoolName = school?.SchoolName;
                }

                // جلب المعلمين
                var teacherSubjects = await _unitOfWork.TeacherSubjects
                    .FindAsync(ts => ts.SubjectId == id);
                var teachers = new List<TeacherDto>();
                foreach (var ts in teacherSubjects)
                {
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(ts.TeacherId);
                    if (teacher != null)
                    {
                        var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
                        teachers.Add(new TeacherDto
                        {
                            Id = teacher.Id,
                            TeacherCode = teacher.TeacherCode,
                            FullName = user?.FullName ?? string.Empty,
                            Specialization = teacher.Specialization
                        });
                    }
                }
                dto.Teachers = teachers;

                // جلب الطلاب
                var students = await _unitOfWork.Students
                    .FindAsync(s => s.ClassRoom != null && s.ClassRoom.GradeLevelId == subject.GradeLevelId);
                dto.Students = _mapper.Map<List<StudentDto>>(students);

                // جلب الإحصائيات
                dto.Statistics = await GetSubjectStatisticsAsync(id);

                return ResponseDto<SubjectDetailsDto>.Ok(dto, "تم جلب المادة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetByIdAsync للمادة {Id}", id);
                return ResponseDto<SubjectDetailsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على مادة بواسطة الكود
        /// </summary>
        public async Task<ResponseDto<SubjectDto>> GetByCodeAsync(string code)
        {
            try
            {
                var subject = await _unitOfWork.Subjects
                    .FindAsync(s => s.SubjectCode == code);
                var subjectItem = subject.FirstOrDefault();

                if (subjectItem == null)
                    return ResponseDto<SubjectDto>.NotFound("المادة غير موجودة");

                var dto = _mapper.Map<SubjectDto>(subjectItem);
                return ResponseDto<SubjectDto>.Ok(dto, "تم جلب المادة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetByCodeAsync للمادة {Code}", code);
                return ResponseDto<SubjectDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }


        public async Task<ResponseDto<PagedResultDto<SubjectDto>>> GetPagedAsync(PaginationDto pagination)
        {
            try
            {
                // ✅ جلب جميع المواد مع Includes
                var allSubjects = await _unitOfWork.Subjects
                    .GetAllWithIncludesAsync(
                        s => s.GradeLevel,
                        s => s.GradeLevel.School,
                        s => s.TeacherSubjects
                    );

                var query = allSubjects.AsEnumerable();

                // ✅ البحث
                if (!string.IsNullOrEmpty(pagination.SearchTerm))
                {
                    query = query.Where(s => s.SubjectName.Contains(pagination.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                             (s.SubjectCode != null && s.SubjectCode.Contains(pagination.SearchTerm, StringComparison.OrdinalIgnoreCase)));
                }

                // ✅ الترتيب
                query = pagination.SortDirection?.ToUpper() == "DESC"
                    ? query.OrderByDescending(s => s.SubjectName)
                    : query.OrderBy(s => s.SubjectName);

                var totalCount = query.Count();

                // ✅ تحويل إلى DTO
                var items = query
                    .Skip(pagination.Skip)
                    .Take(pagination.PageSize)
                    .Select(s => new SubjectDto
                    {
                        Id = s.Id,
                        SubjectName = s.SubjectName,
                        SubjectCode = s.SubjectCode,
                        GradeLevelId = s.GradeLevelId,
                        GradeLevelName = s.GradeLevel != null ? s.GradeLevel.GradeName : string.Empty,
                        SchoolName = s.GradeLevel != null && s.GradeLevel.School != null ? s.GradeLevel.School.SchoolName : string.Empty,
                        TeachersCount = s.TeacherSubjects != null ? s.TeacherSubjects.Count : 0,
                        IsActive = s.IsActive,
                        CreatedAt = s.CreatedAt
                    })
                    .ToList();

                var result = new PagedResultDto<SubjectDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize
                };

                return ResponseDto<PagedResultDto<SubjectDto>>.Ok(result, "تم جلب المواد بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetPagedAsync");
                return ResponseDto<PagedResultDto<SubjectDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }


        ///// <summary>
        ///// 📋 الحصول على المواد الدراسية مع Pagination
        ///// </summary>
        //public async Task<ResponseDto<PagedResultDto<SubjectDto>>> GetPagedAsync(PaginationDto pagination)
        //{
        //    try
        //    {
        //        var query = _unitOfWork.Subjects.AsQueryable();

        //        // ✅ البحث
        //        if (!string.IsNullOrEmpty(pagination.SearchTerm))
        //        {
        //            query = query.Where(s => s.SubjectName.Contains(pagination.SearchTerm) ||
        //                                     (s.SubjectCode != null && s.SubjectCode.Contains(pagination.SearchTerm)));
        //        }

        //        // ✅ الترتيب
        //        if (!string.IsNullOrEmpty(pagination.SortBy))
        //        {
        //            query = pagination.SortDirection?.ToUpper() == "DESC"
        //                ? query.OrderByDescending(s => EF.Property<object>(s, pagination.SortBy))
        //                : query.OrderBy(s => EF.Property<object>(s, pagination.SortBy));
        //        }
        //        else
        //        {
        //            query = query.OrderBy(s => s.SubjectName);
        //        }

        //        var totalCount = await query.CountAsync();

        //        var items = await query
        //            .Skip(pagination.Skip)
        //            .Take(pagination.PageSize)
        //            .Select(s => new SubjectDto
        //            {
        //                Id = s.Id,
        //                SubjectName = s.SubjectName,
        //                SubjectCode = s.SubjectCode,
        //                GradeLevelId = s.GradeLevelId,
        //                GradeLevelName = s.GradeLevel.GradeName,
        //                SchoolName = s.GradeLevel.School.SchoolName,
        //                TeachersCount = s.TeacherSubjects.Count,
        //                IsActive = s.IsActive,
        //                CreatedAt = s.CreatedAt
        //            })
        //            .ToListAsync();

        //        var result = new PagedResultDto<SubjectDto>
        //        {
        //            Items = items,
        //            TotalCount = totalCount,
        //            PageNumber = pagination.PageNumber,
        //            PageSize = pagination.PageSize
        //        };

        //        return ResponseDto<PagedResultDto<SubjectDto>>.Ok(result, "تم جلب المواد بنجاح");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "خطأ في GetPagedAsync");
        //        return ResponseDto<PagedResultDto<SubjectDto>>.Fail("حدث خطأ", statusCode: 500);
        //    }
        //}

        ///// <summary>
        ///// 📋 الحصول على المواد الدراسية مع Pagination (بدون AsQueryable)
        ///// </summary>
        //public async Task<ResponseDto<PagedResultDto<SubjectDto>>> GetPagedAsync(PaginationDto pagination)
        //{
        //    try
        //    {
        //        // ✅ جلب جميع المواد أولاً (مع Include)
        //        var allSubjects = await _unitOfWork.Subjects
        //            .FindAsync(s => true); // جلب الكل

        //        var query = allSubjects.AsEnumerable();

        //        // ✅ البحث
        //        if (!string.IsNullOrEmpty(pagination.SearchTerm))
        //        {
        //            query = query.Where(s => s.SubjectName.Contains(pagination.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
        //                                     (s.SubjectCode != null && s.SubjectCode.Contains(pagination.SearchTerm, StringComparison.OrdinalIgnoreCase)));
        //        }

        //        // ✅ الترتيب
        //        query = pagination.SortDirection?.ToUpper() == "DESC"
        //            ? query.OrderByDescending(s => s.SubjectName)
        //            : query.OrderBy(s => s.SubjectName);

        //        var totalCount = query.Count();

        //        // ✅ Pagination
        //        var items = query
        //            .Skip(pagination.Skip)
        //            .Take(pagination.PageSize)
        //            .Select(s => new SubjectDto
        //            {
        //                Id = s.Id,
        //                SubjectName = s.SubjectName,
        //                SubjectCode = s.SubjectCode,
        //                GradeLevelId = s.GradeLevelId,
        //                GradeLevelName = s.GradeLevel?.GradeName ?? string.Empty,
        //                SchoolName = s.GradeLevel?.School?.SchoolName ?? string.Empty,
        //                TeachersCount = s.TeacherSubjects?.Count ?? 0,
        //                IsActive = s.IsActive,
        //                CreatedAt = s.CreatedAt
        //            })
        //            .ToList();

        //        var result = new PagedResultDto<SubjectDto>
        //        {
        //            Items = items,
        //            TotalCount = totalCount,
        //            PageNumber = pagination.PageNumber,
        //            PageSize = pagination.PageSize
        //        };

        //        return ResponseDto<PagedResultDto<SubjectDto>>.Ok(result, "تم جلب المواد بنجاح");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "خطأ في GetPagedAsync");
        //        return ResponseDto<PagedResultDto<SubjectDto>>.Fail("حدث خطأ", statusCode: 500);
        //    }
        //}


        /////======================================  هذه الداله بطيئة في جلب البيانات  ==========================
        ///// <summary>
        ///// 📋 الحصول على المواد الدراسية مع Pagination
        ///// </summary>
        //public async Task<ResponseDto<PagedResultDto<SubjectDto>>> GetPagedAsync(PaginationDto pagination)
        //{
        //    try
        //    {
        //        // ✅ جلب جميع المواد مع Includes
        //        var allSubjects = await _unitOfWork.Subjects
        //            .FindAsync(s => true); // جلب الكل

        //        // ✅ جلب البيانات الإضافية يدوياً
        //        var subjectList = allSubjects.ToList();
        //        var dtos = new List<SubjectDto>();

        //        foreach (var subject in subjectList)
        //        {
        //            // ✅ جلب اسم الصف
        //            var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
        //            var gradeLevelName = gradeLevel?.GradeName ?? string.Empty;

        //            // ✅ جلب اسم المدرسة
        //            var schoolName = string.Empty;
        //            if (gradeLevel != null)
        //            {
        //                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(gradeLevel.SchoolId);
        //                schoolName = school?.SchoolName ?? string.Empty;
        //            }

        //            // ✅ جلب عدد المعلمين
        //            var teacherSubjects = await _unitOfWork.TeacherSubjects
        //                .FindAsync(ts => ts.SubjectId == subject.Id);
        //            var teachersCount = teacherSubjects.Count();

        //            dtos.Add(new SubjectDto
        //            {
        //                Id = subject.Id,
        //                SubjectName = subject.SubjectName,
        //                SubjectCode = subject.SubjectCode,
        //                GradeLevelId = subject.GradeLevelId,
        //                GradeLevelName = gradeLevelName,
        //                SchoolName = schoolName,
        //                TeachersCount = teachersCount,
        //                IsActive = subject.IsActive,
        //                CreatedAt = subject.CreatedAt
        //            });
        //        }

        //        // ✅ البحث والفلترة والترتيب
        //        var query = dtos.AsEnumerable();

        //        if (!string.IsNullOrEmpty(pagination.SearchTerm))
        //        {
        //            query = query.Where(s => s.SubjectName.Contains(pagination.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
        //                                     (s.SubjectCode != null && s.SubjectCode.Contains(pagination.SearchTerm, StringComparison.OrdinalIgnoreCase)));
        //        }

        //        query = pagination.SortDirection?.ToUpper() == "DESC"
        //            ? query.OrderByDescending(s => s.SubjectName)
        //            : query.OrderBy(s => s.SubjectName);

        //        var totalCount = query.Count();

        //        var items = query
        //            .Skip(pagination.Skip)
        //            .Take(pagination.PageSize)
        //            .ToList();

        //        var result = new PagedResultDto<SubjectDto>
        //        {
        //            Items = items,
        //            TotalCount = totalCount,
        //            PageNumber = pagination.PageNumber,
        //            PageSize = pagination.PageSize
        //        };

        //        return ResponseDto<PagedResultDto<SubjectDto>>.Ok(result, "تم جلب المواد بنجاح");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "خطأ في GetPagedAsync");
        //        return ResponseDto<PagedResultDto<SubjectDto>>.Fail("حدث خطأ", statusCode: 500);
        //    }
        //}






        #endregion

        #region ════════════════════════════════════ البحث والفلترة ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على المواد التابعة لصف معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<SubjectDto>>> GetByGradeLevelIdAsync(int gradeLevelId)
        {
            try
            {
                var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(gradeLevelId);
                if (gradeLevel == null)
                    return ResponseDto<IEnumerable<SubjectDto>>.NotFound("الصف غير موجود");

                var subjects = await _unitOfWork.Subjects
                    .FindAsync(s => s.GradeLevelId == gradeLevelId);
                var dtos = _mapper.Map<IEnumerable<SubjectDto>>(subjects);

                foreach (var dto in dtos)
                {
                    dto.GradeLevelName = gradeLevel.GradeName;

                    var teacherSubjects = await _unitOfWork.TeacherSubjects
                        .FindAsync(ts => ts.SubjectId == dto.Id);
                    dto.TeachersCount = teacherSubjects.Count();
                }

                return ResponseDto<IEnumerable<SubjectDto>>.Ok(dtos, "تم جلب المواد بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetByGradeLevelIdAsync للصف {GradeLevelId}", gradeLevelId);
                return ResponseDto<IEnumerable<SubjectDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على المواد التي يدرسها معلم معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<SubjectDto>>> GetByTeacherIdAsync(int teacherId)
        {
            try
            {
                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(teacherId);
                if (teacher == null)
                    return ResponseDto<IEnumerable<SubjectDto>>.NotFound("المعلم غير موجود");

                var teacherSubjects = await _unitOfWork.TeacherSubjects
                    .FindAsync(ts => ts.TeacherId == teacherId);
                var subjectIds = teacherSubjects.Select(ts => ts.SubjectId).ToList();

                var subjects = new List<Subject>();
                foreach (var subjectId in subjectIds)
                {
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(subjectId);
                    if (subject != null)
                        subjects.Add(subject);
                }

                var dtos = _mapper.Map<IEnumerable<SubjectDto>>(subjects);
                return ResponseDto<IEnumerable<SubjectDto>>.Ok(dtos, "تم جلب المواد بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetByTeacherIdAsync للمعلم {TeacherId}", teacherId);
                return ResponseDto<IEnumerable<SubjectDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ القوائم المنسدلة ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على المواد للقوائم المنسدلة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<SubjectLookupDto>>> GetLookupAsync(int? gradeLevelId = null)
        {
            try
            {
                IEnumerable<Subject> subjects;

                if (gradeLevelId.HasValue)
                {
                    subjects = await _unitOfWork.Subjects
                        .FindAsync(s => s.GradeLevelId == gradeLevelId.Value);
                }
                else
                {
                    subjects = await _unitOfWork.Subjects.GetAllAsync();
                }

                var dtos = _mapper.Map<IEnumerable<SubjectLookupDto>>(subjects);

                foreach (var dto in dtos)
                {
                    var gradeInfo = await _unitOfWork.GradeLevels.GetByIdAsync(dto.Id);
                    dto.GradeLevelName = gradeInfo?.GradeName;
                }

                return ResponseDto<IEnumerable<SubjectLookupDto>>.Ok(dtos, "تم جلب المواد للقوائم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetLookupAsync");
                return ResponseDto<IEnumerable<SubjectLookupDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ الإحصائيات ════════════════════════════════════

        /// <summary>
        /// 📊 الحصول على إحصائيات المادة
        /// </summary>
        public async Task<ResponseDto<SubjectStatisticsDto>> GetStatisticsAsync(int subjectId)
        {
            try
            {
                var subject = await _unitOfWork.Subjects.GetByIdAsync(subjectId);
                if (subject == null)
                    return ResponseDto<SubjectStatisticsDto>.NotFound("المادة غير موجودة");

                var statistics = await GetSubjectStatisticsAsync(subjectId);
                return ResponseDto<SubjectStatisticsDto>.Ok(statistics, "تم جلب إحصائيات المادة");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetStatisticsAsync للمادة {SubjectId}", subjectId);
                return ResponseDto<SubjectStatisticsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        private async Task<SubjectStatisticsDto> GetSubjectStatisticsAsync(int subjectId)
        {
            try
            {
                // جلب المعلمين
                var teacherSubjects = await _unitOfWork.TeacherSubjects
                    .FindAsync(ts => ts.SubjectId == subjectId);

                // جلب الفصول
                var schedules = await _unitOfWork.ClassSchedules
                    .FindAsync(cs => cs.SubjectId == subjectId);
                var classRoomIds = schedules.Select(cs => cs.ClassRoomId).Distinct().ToList();

                // جلب الامتحانات
                var exams = await _unitOfWork.Exams
                    .FindAsync(e => e.SubjectId == subjectId);

                // جلب النتائج
                var examResults = new List<ExamResult>();
                foreach (var exam in exams)
                {
                    var results = await _unitOfWork.ExamResults
                        .FindAsync(er => er.ExamId == exam.Id);
                    examResults.AddRange(results);
                }

                // حساب عدد الطلاب
                var totalStudents = 0;
                foreach (var classRoomId in classRoomIds)
                {
                    var students = await _unitOfWork.Students
                        .FindAsync(s => s.ClassRoomId == classRoomId);
                    totalStudents += students.Count();
                }

                var subject = await _unitOfWork.Subjects.GetByIdAsync(subjectId);

                var statistics = new SubjectStatisticsDto
                {
                    TotalTeachers = teacherSubjects.Count(),
                    TotalStudents = totalStudents,
                    TotalClassRooms = classRoomIds.Count,
                    TotalWeeklyHours = schedules.Count(),
                    AverageScore = examResults.Any() ? (decimal)examResults.Average(r => r.Score) : 0,
                    SuccessRate = examResults.Any()
                        ? (decimal)examResults.Count(r => r.Score >= 50) / examResults.Count() * 100
                        : 0,
                    TotalExams = exams.Count(),
                    MaxScore = examResults.Any() ? examResults.Max(r => r.Score) : 0,
                    MinScore = examResults.Any() ? examResults.Min(r => r.Score) : 0
                };

                return statistics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في حساب إحصائيات المادة {SubjectId}", subjectId);
                return new SubjectStatisticsDto();
            }
        }

        #endregion

        #region ════════════════════════════════════ العمليات الأساسية ════════════════════════════════════

        /// <summary>
        /// ➕ إنشاء مادة جديدة
        /// </summary>
        public async Task<ResponseDto<SubjectDto>> CreateAsync(CreateSubjectDto createDto)
        {
            try
            {
                // التحقق من وجود الصف
                var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(createDto.GradeLevelId);
                if (gradeLevel == null)
                    return ResponseDto<SubjectDto>.Fail("الصف غير موجود");

                // التحقق من وجود اسم مكرر
                var existing = await _unitOfWork.Subjects
             .FindAsync(s => s.GradeLevelId == createDto.GradeLevelId && s.SubjectName == createDto.SubjectName);
                if (existing.Any())
                    return ResponseDto<SubjectDto>.Fail($"المادة {createDto.SubjectName} موجودة بالفعل");

                // التحقق من وجود كود مكرر
                if (!string.IsNullOrEmpty(createDto.SubjectCode))
                {
                    var existingCode = await _unitOfWork.Subjects
                        .FindAsync(s => s.SubjectCode == createDto.SubjectCode);
                    if (existingCode.Any())
                        return ResponseDto<SubjectDto>.Fail($"كود المادة {createDto.SubjectCode} موجود بالفعل");
                }

                var subject = _mapper.Map<Subject>(createDto);
                subject.CreatedAt = DateTime.Now;
                subject.IsActive = true;

                var created = await _unitOfWork.Subjects.AddAsync(subject);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<SubjectDto>(created);
                dto.GradeLevelName = gradeLevel.GradeName;

                _logger.LogInformation("تم إنشاء مادة جديدة: {SubjectName}", created.SubjectName);
                return ResponseDto<SubjectDto>.Ok(dto, "تم إنشاء المادة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في CreateAsync للمادة {SubjectName}", createDto.SubjectName);
                return ResponseDto<SubjectDto>.Fail("حدث خطأ أثناء إنشاء المادة", statusCode: 500);
            }
        }

        /// <summary>
        /// ✏️ تحديث بيانات مادة
        /// </summary>
        public async Task<ResponseDto<SubjectDto>> UpdateAsync(int id, UpdateSubjectDto updateDto)
        {
            try
            {
                var subject = await _unitOfWork.Subjects.GetByIdAsync(id);
                if (subject == null)
                    return ResponseDto<SubjectDto>.NotFound("المادة غير موجودة");

                // التحقق من وجود الصف
                if (updateDto.GradeLevelId.HasValue)
                {
                    var gradeLevelCheck = await _unitOfWork.GradeLevels.GetByIdAsync(updateDto.GradeLevelId.Value);
                    if (gradeLevelCheck == null)
                        return ResponseDto<SubjectDto>.Fail("الصف غير موجود");
                }

                // ✅ التحقق من وجود اسم مكرر (باستخدام FindAsync)
                if (!string.IsNullOrEmpty(updateDto.SubjectName))
                {
                    var gradeLevelId = updateDto.GradeLevelId ?? subject.GradeLevelId;
                    var existing = await _unitOfWork.Subjects
                        .FindAsync(s => s.GradeLevelId == gradeLevelId && s.SubjectName == updateDto.SubjectName && s.Id != id);
                    if (existing.Any())
                        return ResponseDto<SubjectDto>.Fail($"الاسم {updateDto.SubjectName} موجود بالفعل");
                }

                _mapper.Map(updateDto, subject);
                subject.UpdatedAt = DateTime.Now;

                await _unitOfWork.Subjects.UpdateAsync(subject);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<SubjectDto>(subject);

                var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(subject.GradeLevelId);
                dto.GradeLevelName = gradeLevel?.GradeName;

                _logger.LogInformation("تم تحديث المادة: {SubjectName}", subject.SubjectName);
                return ResponseDto<SubjectDto>.Ok(dto, "تم تحديث المادة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في UpdateAsync للمادة {Id}", id);
                return ResponseDto<SubjectDto>.Fail("حدث خطأ أثناء تحديث المادة", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف مادة (Soft Delete)
        /// </summary>
        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var subject = await _unitOfWork.Subjects.GetByIdAsync(id);
                if (subject == null)
                    return ResponseDto.NotFound("المادة غير موجودة");

                // التحقق من وجود معلمين مرتبطين
                var teacherSubjects = await _unitOfWork.TeacherSubjects
                    .FindAsync(ts => ts.SubjectId == id);
                if (teacherSubjects.Any())
                    return ResponseDto.Fail("لا يمكن حذف المادة لأنها مرتبطة بمعلمين");

                // التحقق من وجود جدول حصص مرتبط
                var schedules = await _unitOfWork.ClassSchedules
                    .FindAsync(cs => cs.SubjectId == id);
                if (schedules.Any())
                    return ResponseDto.Fail("لا يمكن حذف المادة لأنها مرتبطة بجدول حصص");

                subject.IsDeleted = true;
                subject.IsActive = false;
                subject.DeletedAt = DateTime.Now;
                subject.UpdatedAt = DateTime.Now;

                await _unitOfWork.Subjects.UpdateAsync(subject);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف المادة: {SubjectName}", subject.SubjectName);
                return ResponseDto.Ok("تم حذف المادة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في DeleteAsync للمادة {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف المادة", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ التحقق ════════════════════════════════════

        /// <summary>
        /// ✅ التحقق من وجود مادة بنفس الاسم في الصف
        /// </summary>
        public async Task<ResponseDto<bool>> IsNameExistsAsync(int gradeLevelId, string name, int? excludeId = null)
        {
            try
            {
                // ✅ استخدام FindAsync بدلاً من IsNameExistsAsync
                var existing = await _unitOfWork.Subjects
                    .FindAsync(s => s.GradeLevelId == gradeLevelId && s.SubjectName == name);

                // ✅ التحقق من excludeId
                var exists = existing.Any() && (excludeId == null || existing.All(s => s.Id != excludeId));

                return ResponseDto<bool>.Ok(exists, exists ? "الاسم موجود" : "الاسم غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في IsNameExistsAsync للمادة {Name}", name);
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }
        #endregion

        /// <summary>
        /// 🔄 تفعيل / إلغاء تفعيل المادة
        /// </summary>
        public async Task<ResponseDto> ToggleActiveAsync(int id)
        {
            try
            {
                var subject = await _unitOfWork.Subjects.GetByIdAsync(id);
                if (subject == null)
                    return ResponseDto.NotFound("المادة غير موجودة");

                // ✅ تغيير الحالة
                subject.IsActive = !subject.IsActive;
                subject.UpdatedAt = DateTime.Now;

                await _unitOfWork.Subjects.UpdateAsync(subject);
                await _unitOfWork.CompleteAsync();

                var status = subject.IsActive ? "تفعيل" : "إلغاء تفعيل";
                _logger.LogInformation("تم {Status} المادة: {SubjectName}", status, subject.SubjectName);

                return ResponseDto.Ok($"تم {status} المادة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في ToggleActiveAsync للمادة {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء تغيير حالة المادة", statusCode: 500);
            }
        }

    }
}