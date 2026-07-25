using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.ExamResults;
using SchoolERP.Application.DTOs.Exams;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📝  خدمة الامتحانات (ExamService)
    /// 📌  الوظيفة: تنفيذ عمليات إدارة الامتحانات
    /// 📦  الاستخدام: في ExamsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ExamService : IExamService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ExamService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public ExamService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ExamService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ الحصول على الامتحانات ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع الامتحانات
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ExamDto>>> GetAllAsync()
        {
            try
            {
                var exams = await _unitOfWork.Exams.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ExamDto>>(exams);

                foreach (var dto in dtos)
                {
                    // جلب الأسماء المرتبطة
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(dto.SubjectId);
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(dto.TeacherId ?? 0);
                    var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(dto.ClassRoomId ?? 0);
                    var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(dto.AcademicYearId);

                    dto.SubjectName = subject?.SubjectName;
                    dto.TeacherName = teacher?.User?.FullName;
                    dto.ClassRoomName = classRoom?.ClassName;
                    dto.AcademicYearName = academicYear?.YearName;
                    dto.ExamTypeName = GetExamTypeName(dto.ExamType);

                    // جلب عدد الطلاب والنتائج
                    var results = await _unitOfWork.ExamResults
                        .FindAsync(er => er.ExamId == dto.Id);
                    dto.StudentsCount = results.Count();
                    
                    if (results.Any())
                    {
                        dto.AverageScore = (decimal)results.Average(r => r.Score);
                        dto.MaxStudentScore = results.Max(r => r.Score);
                        dto.MinStudentScore = results.Min(r => r.Score);
                        dto.SuccessRate = (decimal)results.Count(r => r.Score >= 50) / results.Count() * 100;
                    }
                }

                _logger.LogInformation("تم جلب {Count} امتحان", dtos.Count());
                return ResponseDto<IEnumerable<ExamDto>>.Ok(dtos, "تم جلب الامتحانات بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جميع الامتحانات");
                return ResponseDto<IEnumerable<ExamDto>>.Fail("حدث خطأ أثناء جلب الامتحانات", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على امتحانات عام دراسي معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ExamDto>>> GetByAcademicYearIdAsync(int academicYearId)
        {
            try
            {
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(academicYearId);
                if (academicYear == null)
                {
                    return ResponseDto<IEnumerable<ExamDto>>.NotFound("العام الدراسي غير موجود");
                }

                var exams = await _unitOfWork.Exams
                    .FindAsync(e => e.AcademicYearId == academicYearId);
                var dtos = _mapper.Map<IEnumerable<ExamDto>>(exams);

                foreach (var dto in dtos)
                {
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(dto.SubjectId);
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(dto.TeacherId ?? 0);
                    var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(dto.ClassRoomId ?? 0);

                    dto.SubjectName = subject?.SubjectName;
                    dto.TeacherName = teacher?.User?.FullName;
                    dto.ClassRoomName = classRoom?.ClassName;
                    dto.AcademicYearName = academicYear.YearName;
                    dto.ExamTypeName = GetExamTypeName(dto.ExamType);
                }

                return ResponseDto<IEnumerable<ExamDto>>.Ok(dtos, "تم جلب الامتحانات بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الامتحانات للعام الدراسي {AcademicYearId}", academicYearId);
                return ResponseDto<IEnumerable<ExamDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على امتحانات مادة معينة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ExamDto>>> GetBySubjectIdAsync(int subjectId)
        {
            try
            {
                var subject = await _unitOfWork.Subjects.GetByIdAsync(subjectId);
                if (subject == null)
                {
                    return ResponseDto<IEnumerable<ExamDto>>.NotFound("المادة غير موجودة");
                }

                var exams = await _unitOfWork.Exams
                    .FindAsync(e => e.SubjectId == subjectId);
                var dtos = _mapper.Map<IEnumerable<ExamDto>>(exams);

                foreach (var dto in dtos)
                {
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(dto.TeacherId ?? 0);
                    var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(dto.ClassRoomId ?? 0);
                    var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(dto.AcademicYearId);

                    dto.TeacherName = teacher?.User?.FullName;
                    dto.ClassRoomName = classRoom?.ClassName;
                    dto.AcademicYearName = academicYear?.YearName;
                    dto.ExamTypeName = GetExamTypeName(dto.ExamType);
                    dto.SubjectName = subject.SubjectName;
                }

                return ResponseDto<IEnumerable<ExamDto>>.Ok(dtos, "تم جلب الامتحانات بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الامتحانات للمادة {SubjectId}", subjectId);
                return ResponseDto<IEnumerable<ExamDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على امتحانات فصل معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ExamDto>>> GetByClassRoomIdAsync(int classRoomId)
        {
            try
            {
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(classRoomId);
                if (classRoom == null)
                {
                    return ResponseDto<IEnumerable<ExamDto>>.NotFound("الفصل غير موجود");
                }

                var exams = await _unitOfWork.Exams
                    .FindAsync(e => e.ClassRoomId == classRoomId);
                var dtos = _mapper.Map<IEnumerable<ExamDto>>(exams);

                foreach (var dto in dtos)
                {
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(dto.SubjectId);
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(dto.TeacherId ?? 0);
                    var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(dto.AcademicYearId);

                    dto.SubjectName = subject?.SubjectName;
                    dto.TeacherName = teacher?.User?.FullName;
                    dto.AcademicYearName = academicYear?.YearName;
                    dto.ExamTypeName = GetExamTypeName(dto.ExamType);
                    dto.ClassRoomName = classRoom.ClassName;
                }

                return ResponseDto<IEnumerable<ExamDto>>.Ok(dtos, "تم جلب الامتحانات بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الامتحانات للفصل {ClassRoomId}", classRoomId);
                return ResponseDto<IEnumerable<ExamDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على امتحانات معلم معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ExamDto>>> GetByTeacherIdAsync(int teacherId)
        {
            try
            {
                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(teacherId);
                if (teacher == null)
                {
                    return ResponseDto<IEnumerable<ExamDto>>.NotFound("المعلم غير موجود");
                }

                var exams = await _unitOfWork.Exams
                    .FindAsync(e => e.TeacherId == teacherId);
                var dtos = _mapper.Map<IEnumerable<ExamDto>>(exams);

                foreach (var dto in dtos)
                {
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(dto.SubjectId);
                    var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(dto.ClassRoomId ?? 0);
                    var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(dto.AcademicYearId);

                    dto.SubjectName = subject?.SubjectName;
                    dto.ClassRoomName = classRoom?.ClassName;
                    dto.AcademicYearName = academicYear?.YearName;
                    dto.ExamTypeName = GetExamTypeName(dto.ExamType);
                    dto.TeacherName = teacher.User?.FullName;
                }

                return ResponseDto<IEnumerable<ExamDto>>.Ok(dtos, "تم جلب الامتحانات بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الامتحانات للمعلم {TeacherId}", teacherId);
                return ResponseDto<IEnumerable<ExamDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الامتحانات القادمة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ExamDto>>> GetUpcomingExamsAsync(DateTime fromDate)
        {
            try
            {
                var exams = await _unitOfWork.Exams
                    .FindAsync(e => e.ExamDate >= fromDate);
                var dtos = _mapper.Map<IEnumerable<ExamDto>>(exams);

                foreach (var dto in dtos)
                {
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(dto.SubjectId);
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(dto.TeacherId ?? 0);
                    var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(dto.ClassRoomId ?? 0);
                    var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(dto.AcademicYearId);

                    dto.SubjectName = subject?.SubjectName;
                    dto.TeacherName = teacher?.User?.FullName;
                    dto.ClassRoomName = classRoom?.ClassName;
                    dto.AcademicYearName = academicYear?.YearName;
                    dto.ExamTypeName = GetExamTypeName(dto.ExamType);
                }

                return ResponseDto<IEnumerable<ExamDto>>.Ok(dtos, "تم جلب الامتحانات القادمة");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الامتحانات القادمة");
                return ResponseDto<IEnumerable<ExamDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الامتحانات للقوائم المنسدلة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ExamLookupDto>>> GetLookupAsync(int? academicYearId = null)
        {
            try
            {
                IEnumerable<Exam> exams;

                if (academicYearId.HasValue)
                {
                    exams = await _unitOfWork.Exams
                        .FindAsync(e => e.AcademicYearId == academicYearId.Value);
                }
                else
                {
                    exams = await _unitOfWork.Exams.GetAllAsync();
                }

                var dtos = _mapper.Map<IEnumerable<ExamLookupDto>>(exams);

                foreach (var dto in dtos)
                {
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(dto.SubjectId);
                    var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(dto.ClassRoomId );

                    dto.SubjectName = subject?.SubjectName;
                    dto.ClassRoomName = classRoom?.ClassName;
                    dto.ExamTypeName = GetExamTypeName(dto.ExamType);
                }

                return ResponseDto<IEnumerable<ExamLookupDto>>.Ok(dtos, "تم جلب الامتحانات للقوائم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الامتحانات للقوائم");
                return ResponseDto<IEnumerable<ExamLookupDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ البحث عن امتحان ════════════════════════════════════

        /// <summary>
        /// 🔍 الحصول على امتحان بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<ExamDetailsDto>> GetByIdAsync(int id)
        {
            try
            {
                var exam = await _unitOfWork.Exams.GetByIdAsync(id);
                if (exam == null)
                {
                    return ResponseDto<ExamDetailsDto>.NotFound("الامتحان غير موجود");
                }

                var dto = _mapper.Map<ExamDetailsDto>(exam);

                var subject = await _unitOfWork.Subjects.GetByIdAsync(exam.SubjectId);
                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(exam.TeacherId ?? 0);
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(exam.ClassRoomId ?? 0);
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(exam.AcademicYearId);

                dto.SubjectName = subject?.SubjectName;
                dto.TeacherName = teacher?.User?.FullName;
                dto.ClassRoomName = classRoom?.ClassName;
                dto.AcademicYearName = academicYear?.YearName;
                dto.ExamTypeName = GetExamTypeName(exam.ExamType);

                // جلب النتائج
                var results = await _unitOfWork.ExamResults
                    .FindAsync(er => er.ExamId == id);
                dto.Results = _mapper.Map<List<ExamResultDto>>(results);

                // إحصائيات الامتحان
                dto.Statistics = await GetExamStatisticsAsync(id);

                return ResponseDto<ExamDetailsDto>.Ok(dto, "تم جلب الامتحان بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الامتحان {Id}", id);
                return ResponseDto<ExamDetailsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📊 الحصول على إحصائيات الامتحان
        /// </summary>
        public async Task<ResponseDto<ExamStatisticsDto>> GetStatisticsAsync(int examId)
        {
            try
            {
                var exam = await _unitOfWork.Exams.GetByIdAsync(examId);
                if (exam == null)
                {
                    return ResponseDto<ExamStatisticsDto>.NotFound("الامتحان غير موجود");
                }

                var statistics = await GetExamStatisticsAsync(examId);
                return ResponseDto<ExamStatisticsDto>.Ok(statistics, "تم جلب إحصائيات الامتحان");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب إحصائيات الامتحان {ExamId}", examId);
                return ResponseDto<ExamStatisticsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📊 حساب إحصائيات الامتحان
        /// </summary>
        private async Task<ExamStatisticsDto> GetExamStatisticsAsync(int examId)
        {
            try
            {
                var results = await _unitOfWork.ExamResults
                    .FindAsync(er => er.ExamId == examId);

                var exam = await _unitOfWork.Exams.GetByIdAsync(examId);

                var statistics = new ExamStatisticsDto
                {
                    TotalStudents = results.Count(),
                    PassedStudents = results.Count(r => r.Score >= 50),
                    FailedStudents = results.Count(r => r.Score < 50),
                    SuccessRate = results.Any() 
                        ? (decimal)results.Count(r => r.Score >= 50) / results.Count() * 100 
                        : 0,
                    AverageScore = results.Any() 
                        ? (decimal)results.Average(r => r.Score) 
                        : 0,
                    MaxScore = results.Any() ? results.Max(r => r.Score) : 0,
                    MinScore = results.Any() ? results.Min(r => r.Score) : 0,
                    AverageAnswerTime = 0, // سيتم حسابه لاحقاً
                    ScoreDistribution = new Dictionary<string, int>()
                };

                // توزيع الدرجات
                if (results.Any())
                {
                    var maxScore = exam?.MaxScore ?? 100;
                    var ranges = new[] { 0, 20, 40, 60, 80, 100 };
                    
                    foreach (var range in ranges)
                    {
                        var count = results.Count(r => r.Score >= range && r.Score < (range + 20));
                        if (count > 0 || range < 100)
                        {
                            statistics.ScoreDistribution[$"{range}-{range + 19}"] = count;
                        }
                    }
                }

                return statistics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء حساب إحصائيات الامتحان {ExamId}", examId);
                return new ExamStatisticsDto();
            }
        }

        #endregion

        #region ════════════════════════════════════ إنشاء وتحديث وحذف ════════════════════════════════════

        /// <summary>
        /// ➕ إنشاء امتحان جديد
        /// </summary>
        public async Task<ResponseDto<ExamDto>> CreateAsync(CreateExamDto createDto)
        {
            try
            {
                // التحقق من وجود العام الدراسي
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(createDto.AcademicYearId);
                if (academicYear == null)
                {
                    return ResponseDto<ExamDto>.Fail("العام الدراسي غير موجود");
                }

                // التحقق من وجود المادة
                var subject = await _unitOfWork.Subjects.GetByIdAsync(createDto.SubjectId);
                if (subject == null)
                {
                    return ResponseDto<ExamDto>.Fail("المادة غير موجودة");
                }

                // التحقق من وجود الفصل
                if (createDto.ClassRoomId.HasValue)
                {
                    var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(createDto.ClassRoomId.Value);
                    if (classRoom == null)
                    {
                        return ResponseDto<ExamDto>.Fail("الفصل غير موجود");
                    }
                }

                // التحقق من وجود المعلم
                if (createDto.TeacherId.HasValue)
                {
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(createDto.TeacherId.Value);
                    if (teacher == null)
                    {
                        return ResponseDto<ExamDto>.Fail("المعلم غير موجود");
                    }
                }

                // التحقق من عدم وجود امتحان مكرر في نفس اليوم والوقت
                if (createDto.ClassRoomId.HasValue)
                {
                    var existingExams = await _unitOfWork.Exams
                        .FindAsync(e => e.ClassRoomId == createDto.ClassRoomId.Value &&
                                        e.ExamDate == createDto.ExamDate &&
                                        e.StartTime == createDto.StartTime);
                    if (existingExams.Any())
                    {
                        return ResponseDto<ExamDto>.Fail("يوجد امتحان آخر في هذا الفصل في نفس التوقيت");
                    }
                }

                var exam = _mapper.Map<Exam>(createDto);
                exam.CreatedAt = DateTime.Now;
                exam.IsActive = true;

                var created = await _unitOfWork.Exams.AddAsync(exam);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<ExamDto>(created);
                _logger.LogInformation("تم إنشاء امتحان جديد: {Name}", created.ExamName);

                return ResponseDto<ExamDto>.Ok(dto, "تم إنشاء الامتحان بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء امتحان جديد");
                return ResponseDto<ExamDto>.Fail("حدث خطأ أثناء إنشاء الامتحان", statusCode: 500);
            }
        }

        /// <summary>
        /// ✏️ تحديث بيانات امتحان
        /// </summary>
        public async Task<ResponseDto<ExamDto>> UpdateAsync(int id, UpdateExamDto updateDto)
        {
            try
            {
                var exam = await _unitOfWork.Exams.GetByIdAsync(id);
                if (exam == null)
                {
                    return ResponseDto<ExamDto>.NotFound("الامتحان غير موجود");
                }

                // التحقق من وجود العام الدراسي
                if (updateDto.AcademicYearId.HasValue)
                {
                    var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(updateDto.AcademicYearId.Value);
                    if (academicYear == null)
                    {
                        return ResponseDto<ExamDto>.Fail("العام الدراسي غير موجود");
                    }
                }

                // التحقق من وجود المادة
                if (updateDto.SubjectId.HasValue)
                {
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(updateDto.SubjectId.Value);
                    if (subject == null)
                    {
                        return ResponseDto<ExamDto>.Fail("المادة غير موجودة");
                    }
                }

                _mapper.Map(updateDto, exam);
                exam.UpdatedAt = DateTime.Now;

                await _unitOfWork.Exams.UpdateAsync(exam);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<ExamDto>(exam);
                _logger.LogInformation("تم تحديث الامتحان: {Name}", exam.ExamName);

                return ResponseDto<ExamDto>.Ok(dto, "تم تحديث الامتحان بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تحديث الامتحان {Id}", id);
                return ResponseDto<ExamDto>.Fail("حدث خطأ أثناء تحديث الامتحان", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف امتحان
        /// </summary>
        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var exam = await _unitOfWork.Exams.GetByIdAsync(id);
                if (exam == null)
                {
                    return ResponseDto.NotFound("الامتحان غير موجود");
                }

                // التحقق من وجود نتائج مرتبطة
                var results = await _unitOfWork.ExamResults
                    .FindAsync(er => er.ExamId == id);
                if (results.Any())
                {
                    return ResponseDto.Fail("لا يمكن حذف الامتحان لأنه يحتوي على نتائج مسجلة");
                }

                await _unitOfWork.Exams.DeleteAsync(exam);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف الامتحان: {Name}", exam.ExamName);
                return ResponseDto.Ok("تم حذف الامتحان بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء حذف الامتحان {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف الامتحان", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        /// <summary>
        /// 📝 الحصول على اسم نوع الامتحان بالعربية
        /// </summary>
        private string GetExamTypeName(ExamType examType)
        {
            return examType switch
            {
                ExamType.Monthly => "شهري",
                ExamType.MidTerm => "نصفي",
                ExamType.Final => "نهائي",
                ExamType.Quiz => "اختبار قصير",
                ExamType.Assessment => "تقييم",
                _ => examType.ToString()
            };
        }

        #endregion
    }
}