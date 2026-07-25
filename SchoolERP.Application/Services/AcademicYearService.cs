using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.AcademicYears;
using SchoolERP.Application.DTOs.ClassSchedules;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Exams;
using SchoolERP.Application.DTOs.Students;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📆  خدمة العام الدراسي (AcademicYearService)
    /// 📌  الوظيفة: تنفيذ عمليات إدارة الأعوام الدراسية
    /// 📦  الاستخدام: في AcademicYearsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class AcademicYearService : IAcademicYearService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AcademicYearService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public AcademicYearService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AcademicYearService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ الحصول على الأعوام الدراسية ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع الأعوام الدراسية
        /// </summary>
        public async Task<ResponseDto<IEnumerable<AcademicYearDto>>> GetAllAsync()
        {
            try
            {
                var academicYears = await _unitOfWork.AcademicYears.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<AcademicYearDto>>(academicYears);

                foreach (var dto in dtos)
                {
                    // جلب اسم المدرسة
                    var school = await _unitOfWork.SchoolRepository.GetByIdAsync(dto.SchoolId);
                    dto.SchoolName = school?.SchoolName;

                    // جلب إحصائيات
                    var students = await _unitOfWork.Students
                        .FindAsync(s => s.AcademicYearId == dto.Id);
                    dto.StudentsCount = students.Count();

                    var classRooms = await _unitOfWork.ClassRooms
                        .FindAsync(c => c.GradeLevel.SchoolId == dto.SchoolId);
                    dto.ClassRoomsCount = classRooms.Count();

                    var subjects = await _unitOfWork.Subjects
                        .FindAsync(s => s.GradeLevel.SchoolId == dto.SchoolId);
                    dto.SubjectsCount = subjects.Count();
                }

                _logger.LogInformation("تم جلب {Count} عام دراسي", dtos.Count());
                return ResponseDto<IEnumerable<AcademicYearDto>>.Ok(dtos, "تم جلب الأعوام الدراسية بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جميع الأعوام الدراسية");
                return ResponseDto<IEnumerable<AcademicYearDto>>.Fail("حدث خطأ أثناء جلب الأعوام الدراسية", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الأعوام الدراسية لمدرسة معينة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<AcademicYearDto>>> GetBySchoolIdAsync(int schoolId)
        {
            try
            {
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(schoolId);
                if (school == null)
                {
                    return ResponseDto<IEnumerable<AcademicYearDto>>.NotFound("المدرسة غير موجودة");
                }

                var academicYears = await _unitOfWork.AcademicYears
                    .FindAsync(ay => ay.SchoolId == schoolId);
                var dtos = _mapper.Map<IEnumerable<AcademicYearDto>>(academicYears);

                foreach (var dto in dtos)
                {
                    dto.SchoolName = school.SchoolName;

                    var students = await _unitOfWork.Students
                        .FindAsync(s => s.AcademicYearId == dto.Id);
                    dto.StudentsCount = students.Count();
                }

                return ResponseDto<IEnumerable<AcademicYearDto>>.Ok(dtos, "تم جلب الأعوام الدراسية بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الأعوام الدراسية للمدرسة {SchoolId}", schoolId);
                return ResponseDto<IEnumerable<AcademicYearDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على العام الدراسي الحالي لمدرسة معينة
        /// </summary>
        public async Task<ResponseDto<AcademicYearDto>> GetCurrentYearAsync(int schoolId)
        {
            try
            {
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(schoolId);
                if (school == null)
                {
                    return ResponseDto<AcademicYearDto>.NotFound("المدرسة غير موجودة");
                }

                var academicYear = await _unitOfWork.AcademicYears
                    .FindAsync(ay => ay.SchoolId == schoolId && ay.IsCurrent);
                var currentYear = academicYear.FirstOrDefault();

                if (currentYear == null)
                {
                    return ResponseDto<AcademicYearDto>.NotFound("لا يوجد عام دراسي حالي لهذه المدرسة");
                }

                var dto = _mapper.Map<AcademicYearDto>(currentYear);
                dto.SchoolName = school.SchoolName;

                return ResponseDto<AcademicYearDto>.Ok(dto, "تم جلب العام الدراسي الحالي");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب العام الدراسي الحالي للمدرسة {SchoolId}", schoolId);
                return ResponseDto<AcademicYearDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الأعوام الدراسية للقوائم المنسدلة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<AcademicYearLookupDto>>> GetLookupAsync(int? schoolId = null)
        {
            try
            {
                IEnumerable<AcademicYear> academicYears;

                if (schoolId.HasValue)
                {
                    academicYears = await _unitOfWork.AcademicYears
                        .FindAsync(ay => ay.SchoolId == schoolId.Value);
                }
                else
                {
                    academicYears = await _unitOfWork.AcademicYears.GetAllAsync();
                }

                var dtos = _mapper.Map<IEnumerable<AcademicYearLookupDto>>(academicYears);

                foreach (var dto in dtos)
                {
                    var school = await _unitOfWork.SchoolRepository.GetByIdAsync(dto.SchoolId);
                    dto.SchoolName = school?.SchoolName;
                }

                return ResponseDto<IEnumerable<AcademicYearLookupDto>>.Ok(dtos, "تم جلب الأعوام الدراسية للقوائم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الأعوام الدراسية للقوائم");
                return ResponseDto<IEnumerable<AcademicYearLookupDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ البحث عن عام دراسي ════════════════════════════════════

        /// <summary>
        /// 🔍 الحصول على عام دراسي بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<AcademicYearDetailsDto>> GetByIdAsync(int id)
        {
            try
            {
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(id);
                if (academicYear == null)
                {
                    return ResponseDto<AcademicYearDetailsDto>.NotFound("العام الدراسي غير موجود");
                }

                var dto = _mapper.Map<AcademicYearDetailsDto>(academicYear);

                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(academicYear.SchoolId);
                dto.SchoolName = school?.SchoolName;

                // جلب الطلاب
                var students = await _unitOfWork.Students
                    .FindAsync(s => s.AcademicYearId == id);
                dto.Students = _mapper.Map<List<StudentDto>>(students);

                // جلب جدول الحصص
                var schedules = await _unitOfWork.ClassSchedules
                    .FindAsync(cs => cs.AcademicYearId == id);
                dto.Schedules = _mapper.Map<List<ClassScheduleDto>>(schedules);

                // جلب الامتحانات
                var exams = await _unitOfWork.Exams
                    .FindAsync(e => e.AcademicYearId == id);
                dto.Exams = _mapper.Map<List<ExamDto>>(exams);

                // إحصائيات
                dto.Statistics = await GetAcademicYearStatisticsAsync(id);

                return ResponseDto<AcademicYearDetailsDto>.Ok(dto, "تم جلب العام الدراسي بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب العام الدراسي {Id}", id);
                return ResponseDto<AcademicYearDetailsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📊 الحصول على إحصائيات العام الدراسي
        /// </summary>
        public async Task<ResponseDto<AcademicYearStatisticsDto>> GetStatisticsAsync(int academicYearId)
        {
            try
            {
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(academicYearId);
                if (academicYear == null)
                {
                    return ResponseDto<AcademicYearStatisticsDto>.NotFound("العام الدراسي غير موجود");
                }

                var statistics = await GetAcademicYearStatisticsAsync(academicYearId);
                return ResponseDto<AcademicYearStatisticsDto>.Ok(statistics, "تم جلب إحصائيات العام الدراسي");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب إحصائيات العام الدراسي {AcademicYearId}", academicYearId);
                return ResponseDto<AcademicYearStatisticsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📊 حساب إحصائيات العام الدراسي
        /// </summary>
        private async Task<AcademicYearStatisticsDto> GetAcademicYearStatisticsAsync(int academicYearId)
        {
            try
            {
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(academicYearId);
                if (academicYear == null)
                {
                    return new AcademicYearStatisticsDto();
                }

                // جلب الطلاب
                var students = await _unitOfWork.Students
                    .FindAsync(s => s.AcademicYearId == academicYearId);

                // جلب المعلمين (من خلال المواد)
                var subjects = await _unitOfWork.Subjects
                    .FindAsync(s => s.GradeLevel.SchoolId == academicYear.SchoolId);
                var teacherIds = new List<int>();
                foreach (var subject in subjects)
                {
                    var teacherSubjects = await _unitOfWork.TeacherSubjects
                        .FindAsync(ts => ts.SubjectId == subject.Id);
                    foreach (var ts in teacherSubjects)
                    {
                        if (!teacherIds.Contains(ts.TeacherId))
                        {
                            teacherIds.Add(ts.TeacherId);
                        }
                    }
                }

                // جلب الفصول
                var classRooms = await _unitOfWork.ClassRooms
                    .FindAsync(c => c.GradeLevel.SchoolId == academicYear.SchoolId);

                // جلب الامتحانات
                var exams = await _unitOfWork.Exams
                    .FindAsync(e => e.AcademicYearId == academicYearId);

                // حساب الأيام الدراسية
                var totalDays = (academicYear.EndDate - academicYear.StartDate).Days;
                var remainingDays = (academicYear.EndDate - DateTime.Now).Days > 0 
                    ? (academicYear.EndDate - DateTime.Now).Days 
                    : 0;

                var statistics = new AcademicYearStatisticsDto
                {
                    TotalStudents = students.Count(),
                    TotalTeachers = teacherIds.Count,
                    TotalEmployees = 0, // سيتم حسابه لاحقاً
                    TotalClassRooms = classRooms.Count(),
                    TotalSubjects = subjects.Count(),
                    TotalExams = exams.Count(),
                    TotalWeeklyHours = 0, // سيتم حسابه لاحقاً
                    OverallAttendanceRate = 90.0m, // سيتم حسابه من الحضور الفعلي
                    OverallSuccessRate = 85.0m, // سيتم حسابه من النتائج الفعلية
                    TotalSchoolDays = totalDays,
                    RemainingSchoolDays = remainingDays > 0 ? remainingDays : 0,
                    StudentDistributionByGrade = new Dictionary<string, int>()
                };

                // توزيع الطلاب حسب الصفوف
                foreach (var student in students)
                {
                    var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(student.ClassRoom?.GradeLevelId ?? 0);
                    if (gradeLevel != null)
                    {
                        var key = gradeLevel.GradeName;
                        if (statistics.StudentDistributionByGrade.ContainsKey(key))
                        {
                            statistics.StudentDistributionByGrade[key]++;
                        }
                        else
                        {
                            statistics.StudentDistributionByGrade[key] = 1;
                        }
                    }
                }

                return statistics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء حساب إحصائيات العام الدراسي {AcademicYearId}", academicYearId);
                return new AcademicYearStatisticsDto();
            }
        }

        #endregion

        #region ════════════════════════════════════ إنشاء وتحديث وحذف ════════════════════════════════════

        /// <summary>
        /// ➕ إنشاء عام دراسي جديد
        /// </summary>
        public async Task<ResponseDto<AcademicYearDto>> CreateAsync(CreateAcademicYearDto createDto)
        {
            try
            {
                // التحقق من وجود المدرسة
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(createDto.SchoolId);
                if (school == null)
                {
                    return ResponseDto<AcademicYearDto>.Fail("المدرسة غير موجودة");
                }

                // التحقق من وجود اسم مكرر
                if (await _unitOfWork.AcademicYears.IsNameExistsAsync( createDto.YearName,createDto.SchoolId))
                {
                    return ResponseDto<AcademicYearDto>.Fail($"العام الدراسي {createDto.YearName} موجود بالفعل");
                }

                // التحقق من صحة التاريخ
                if (createDto.StartDate >= createDto.EndDate)
                {
                    return ResponseDto<AcademicYearDto>.Fail("تاريخ البداية يجب أن يكون قبل تاريخ النهاية");
                }

                // إذا كان IsCurrent = true، إلغاء التحديد من الأعوام الأخرى
                if (createDto.IsCurrent)
                {
                    await UnsetCurrentYearAsync(createDto.SchoolId);
                }

                var academicYear = _mapper.Map<AcademicYear>(createDto);
                academicYear.CreatedAt = DateTime.Now;
                academicYear.IsActive = true;

                var created = await _unitOfWork.AcademicYears.AddAsync(academicYear);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<AcademicYearDto>(created);
                dto.SchoolName = school.SchoolName;

                _logger.LogInformation("تم إنشاء عام دراسي جديد: {YearName}", created.YearName);

                return ResponseDto<AcademicYearDto>.Ok(dto, "تم إنشاء العام الدراسي بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء عام دراسي جديد");
                return ResponseDto<AcademicYearDto>.Fail("حدث خطأ أثناء إنشاء العام الدراسي", statusCode: 500);
            }
        }

        /// <summary>
        /// ✏️ تحديث بيانات عام دراسي
        /// </summary>
        public async Task<ResponseDto<AcademicYearDto>> UpdateAsync(int id, UpdateAcademicYearDto updateDto)
        {
            try
            {
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(id);
                if (academicYear == null)
                {
                    return ResponseDto<AcademicYearDto>.NotFound("العام الدراسي غير موجود");
                }

                // التحقق من وجود اسم مكرر
                if (!string.IsNullOrEmpty(updateDto.YearName) &&
                    await _unitOfWork.AcademicYears.IsNameExistsAsync( updateDto.YearName, id))
                {
                    return ResponseDto<AcademicYearDto>.Fail($"العام الدراسي {updateDto.YearName} موجود بالفعل");
                }

                // التحقق من صحة التاريخ
                if (updateDto.StartDate.HasValue && updateDto.EndDate.HasValue &&
                    updateDto.StartDate.Value >= updateDto.EndDate.Value)
                {
                    return ResponseDto<AcademicYearDto>.Fail("تاريخ البداية يجب أن يكون قبل تاريخ النهاية");
                }

                // إذا كان IsCurrent = true، إلغاء التحديد من الأعوام الأخرى
                if (updateDto.IsCurrent && updateDto.IsCurrent)
                {
                    await UnsetCurrentYearAsync(academicYear.SchoolId, id);
                }

                _mapper.Map(updateDto, academicYear);
                academicYear.UpdatedAt = DateTime.Now;

                await _unitOfWork.AcademicYears.UpdateAsync(academicYear);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<AcademicYearDto>(academicYear);

                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(academicYear.SchoolId);
                dto.SchoolName = school?.SchoolName;

                _logger.LogInformation("تم تحديث العام الدراسي: {YearName}", academicYear.YearName);

                return ResponseDto<AcademicYearDto>.Ok(dto, "تم تحديث العام الدراسي بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تحديث العام الدراسي {Id}", id);
                return ResponseDto<AcademicYearDto>.Fail("حدث خطأ أثناء تحديث العام الدراسي", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔄 تعيين عام دراسي كعام حالي
        /// </summary>
        public async Task<ResponseDto> SetCurrentYearAsync(int id)
        {
            try
            {
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(id);
                if (academicYear == null)
                {
                    return ResponseDto.NotFound("العام الدراسي غير موجود");
                }

                // إلغاء التحديد من الأعوام الأخرى
                await UnsetCurrentYearAsync(academicYear.SchoolId, id);

                // تعيين العام الحالي
                academicYear.IsCurrent = true;
                academicYear.UpdatedAt = DateTime.Now;

                await _unitOfWork.AcademicYears.UpdateAsync(academicYear);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم تعيين العام الدراسي {YearName} كعام حالي", academicYear.YearName);
                return ResponseDto.Ok($"تم تعيين العام الدراسي {academicYear.YearName} كعام حالي");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تعيين العام الدراسي {Id} كعام حالي", id);
                return ResponseDto.Fail("حدث خطأ أثناء تعيين العام الدراسي", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف عام دراسي (Soft Delete)
        /// </summary>
        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(id);
                if (academicYear == null)
                {
                    return ResponseDto.NotFound("العام الدراسي غير موجود");
                }

                // التحقق من وجود طلاب مرتبطين
                var students = await _unitOfWork.Students
                    .FindAsync(s => s.AcademicYearId == id);
                if (students.Any())
                {
                    return ResponseDto.Fail("لا يمكن حذف العام الدراسي لأنه يحتوي على طلاب مسجلين");
                }

                academicYear.IsDeleted = true;
                academicYear.IsActive = false;
                academicYear.DeletedAt = DateTime.Now;
                academicYear.UpdatedAt = DateTime.Now;

                await _unitOfWork.AcademicYears.UpdateAsync(academicYear);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف العام الدراسي: {YearName}", academicYear.YearName);
                return ResponseDto.Ok("تم حذف العام الدراسي بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء حذف العام الدراسي {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف العام الدراسي", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        /// <summary>
        /// 🔄 إلغاء تحديد العام الحالي من جميع الأعوام الدراسية للمدرسة
        /// </summary>
        private async Task UnsetCurrentYearAsync(int schoolId, int? excludeId = null)
        {
            var academicYears = await _unitOfWork.AcademicYears
                .FindAsync(ay => ay.SchoolId == schoolId && ay.IsCurrent && (excludeId == null || ay.Id != excludeId));

            foreach (var year in academicYears)
            {
                year.IsCurrent = false;
                year.UpdatedAt = DateTime.Now;
                await _unitOfWork.AcademicYears.UpdateAsync(year);
            }
            await _unitOfWork.CompleteAsync();
        }

        #endregion

        #region ════════════════════════════════════ التحقق من الوجود ════════════════════════════════════

        /// <summary>
        /// ✅ التحقق من وجود عام دراسي بنفس الاسم
        /// </summary>
        public async Task<ResponseDto<bool>> IsNameExistsAsync(int schoolId, string name, int? excludeId = null)
        {
            try
            {
                // ✅ استخدام FindAsync مع الشرط الكامل
                var existing = await _unitOfWork.AcademicYears
                    .FindAsync(ay => ay.SchoolId == schoolId && ay.YearName == name);

                var exists = existing.Any() && (excludeId == null || existing.All(ay => ay.Id != excludeId));

                return ResponseDto<bool>.Ok(exists, exists ? "الاسم موجود" : "الاسم غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء التحقق من الاسم {Name}", name);
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }
        #endregion
    }
}