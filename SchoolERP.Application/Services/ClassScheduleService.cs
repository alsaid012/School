using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.ClassSchedules;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;
using System.Linq.Expressions;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📅  خدمة جدول الحصص (ClassScheduleService)
    /// 📌  الوظيفة: تنفيذ عمليات إدارة جدول الحصص
    /// 📦  الاستخدام: في ClassSchedulesController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ClassScheduleService : IClassScheduleService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ClassScheduleService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public ClassScheduleService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ClassScheduleService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        /// <summary>
        /// 📅 الحصول على اسم اليوم بالعربية
        /// </summary>
        private static string GetDayName(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Sunday => "الأحد",
                DayOfWeek.Monday => "الإثنين",
                DayOfWeek.Tuesday => "الثلاثاء",
                DayOfWeek.Wednesday => "الأربعاء",
                DayOfWeek.Thursday => "الخميس",
                DayOfWeek.Friday => "الجمعة",
                DayOfWeek.Saturday => "السبت",
                _ => day.ToString()
            };
        }

        /// <summary>
        /// 🔄 تحويل قائمة الكيانات إلى DTO مع تعبئة البيانات الإضافية
        /// </summary>
        private async Task<IEnumerable<ClassScheduleDto>> MapToDtoListAsync(IEnumerable<ClassSchedule> schedules)
        {
            var dtos = new List<ClassScheduleDto>();

            foreach (var schedule in schedules)
            {
                var dto = _mapper.Map<ClassScheduleDto>(schedule);

                // ✅ جلب البيانات الإضافية
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(schedule.ClassRoomId);
                var subject = await _unitOfWork.Subjects.GetByIdAsync(schedule.SubjectId);
                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(schedule.TeacherId);
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(schedule.AcademicYearId);


                // ✅ تعبئة السنة الدراسية
                dto.AcademicYearName = academicYear?.YearName ?? string.Empty;

                // ✅ تعبئة المادة
                dto.SubjectName = subject?.SubjectName ?? string.Empty;
                dto.SubjectCode = subject?.SubjectCode ?? string.Empty;
                // ✅ تعبئة اسم المعلم
                if (teacher != null)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
                    dto.TeacherName = user?.FullName ?? teacher.TeacherCode ?? "غير معروف";
                    dto.TeacherCode = teacher.TeacherCode ?? string.Empty;
                }
                else
                {
                    dto.TeacherName = "غير معروف";
                    dto.TeacherCode = string.Empty;
                }

                // ✅ تعبئة بيانات الفصل والصف
                if (classRoom != null)
                {
                    dto.ClassRoomName = classRoom.ClassName ?? string.Empty;
                    dto.ClassRoomId = classRoom.Id;
                    dto.GradeLevelId = classRoom.GradeLevelId;

                    if (classRoom.GradeLevel != null)
                    {
                        dto.GradeLevelName = classRoom.GradeLevel.GradeName ?? string.Empty;
                    }
                    else
                    {
                        var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(classRoom.GradeLevelId);
                        dto.GradeLevelName = gradeLevel?.GradeName ?? "غير محدد";
                    }
                }
                else
                {
                    dto.ClassRoomName = "غير محدد";
                    dto.GradeLevelName = "غير محدد";
                }

                // ✅ تعبئة اليوم
                dto.DayName = GetDayName(schedule.DayOfWeek);
                dtos.Add(dto);
            }

            return dtos;
        }

        /// <summary>
        /// 🔄 تحويل كيان واحد إلى DTO مع تعبئة البيانات الإضافية
        /// </summary>
        private async Task<ClassScheduleDto> MapToDtoAsync(ClassSchedule schedule)
        {
            var dto = _mapper.Map<ClassScheduleDto>(schedule);

            var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(schedule.AcademicYearId);
            var subject = await _unitOfWork.Subjects.GetByIdAsync(schedule.SubjectId);
            var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(schedule.TeacherId);
            var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(schedule.ClassRoomId);

            dto.AcademicYearName = academicYear?.YearName ?? string.Empty;

            dto.SubjectName = subject?.SubjectName ?? string.Empty;
            dto.SubjectCode = subject?.SubjectCode ?? string.Empty;

            // ✅ تعبئة اسم المعلم
            if (teacher != null)
            {
                var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
                dto.TeacherName = user?.FullName ?? teacher.TeacherCode ?? "غير معروف";
                dto.TeacherCode = teacher.TeacherCode ?? string.Empty;
            }
            else
            {
                dto.TeacherName = "غير معروف";
                dto.TeacherCode = string.Empty;
            }
            // ✅ تعبئة بيانات الفصل والصف - مع التحقق الشامل
            if (classRoom != null)
            {
                dto.ClassRoomName = classRoom.ClassName ?? string.Empty;
                dto.ClassRoomId = classRoom.Id;
                dto.GradeLevelId = classRoom.GradeLevelId;

                // ✅ التحقق من وجود GradeLevel
                if (classRoom.GradeLevel != null)
                {
                    dto.GradeLevelName = classRoom.GradeLevel.GradeName ?? string.Empty;
                }
                else
                {
                    // ✅ محاولة جلب GradeLevel بشكل منفصل إذا لم يتم تضمينه
                    var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(classRoom.GradeLevelId);
                    dto.GradeLevelName = gradeLevel?.GradeName ?? "غير محدد";
                }
            }
            else
            {
                dto.ClassRoomName = "غير محدد";
                dto.GradeLevelName = "غير محدد";
            }
            // ✅ تعبئة اليوم
            dto.DayName = GetDayName(schedule.DayOfWeek);
            return dto;
        }

        #endregion

        #region ════════════════════════════════════ جلب البيانات ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع جداول الحصص
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ClassScheduleDto>>> GetAllAsync()
        {
            try
            {
                var schedules = await _unitOfWork.ClassSchedules.GetAllAsync();

                if (schedules == null || !schedules.Any())
                {
                    return ResponseDto<IEnumerable<ClassScheduleDto>>.Ok(
                        new List<ClassScheduleDto>(),
                        "لا توجد حصص حالياً");
                }

                var dtos = await MapToDtoListAsync(schedules);

                _logger.LogInformation("✅ تم جلب {Count} حصة", dtos.Count());
                return ResponseDto<IEnumerable<ClassScheduleDto>>.Ok(dtos, "تم جلب الحصص بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في GetAllAsync");
                return ResponseDto<IEnumerable<ClassScheduleDto>>.Fail(
                    "حدث خطأ أثناء جلب الحصص", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على جميع جداول الحصص مع الفلترة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ClassScheduleDto>>> GetFilteredAsync(ClassScheduleFilterDto filter)
        {
            try
            {
                // ✅ بناء تعبيرات الفلترة
                var predicates = new List<Expression<Func<ClassSchedule, bool>>>();

                if (filter.AcademicYearId.HasValue)
                    predicates.Add(cs => cs.AcademicYearId == filter.AcademicYearId.Value);

                if (filter.ClassRoomId.HasValue)
                    predicates.Add(cs => cs.ClassRoomId == filter.ClassRoomId.Value);

                if (filter.TeacherId.HasValue)
                    predicates.Add(cs => cs.TeacherId == filter.TeacherId.Value);

                if (filter.SubjectId.HasValue)
                    predicates.Add(cs => cs.SubjectId == filter.SubjectId.Value);

                if (filter.DayOfWeek.HasValue)
                    predicates.Add(cs => cs.DayOfWeek == filter.DayOfWeek.Value);

                if (filter.PeriodNumber.HasValue)
                    predicates.Add(cs => cs.PeriodNumber == filter.PeriodNumber.Value);

                if (filter.IsActive.HasValue)
                    predicates.Add(cs => cs.IsActive == filter.IsActive.Value);

                if (filter.StartTimeFrom.HasValue)
                    predicates.Add(cs => cs.StartTime >= filter.StartTimeFrom.Value);

                if (filter.StartTimeTo.HasValue)
                    predicates.Add(cs => cs.StartTime <= filter.StartTimeTo.Value);

                // ✅ تنفيذ البحث
                IEnumerable<ClassSchedule> schedules;

                if (predicates.Any())
                {
                    // ✅ دمج جميع الشروط بـ AND
                    var combinedPredicate = predicates.Aggregate((current, next) =>
                        current.AndAlso(next));

                    schedules = await _unitOfWork.ClassSchedules
                        .FindAsync(combinedPredicate);
                }
                else
                {
                    schedules = await _unitOfWork.ClassSchedules.GetAllAsync();
                }

                // ✅ فلترة إضافية للبحث النصي
                if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                {
                    var search = filter.SearchTerm.Trim();
                    schedules = schedules.Where(cs =>
                        cs.Subject?.SubjectName?.Contains(search) == true ||
                        cs.Teacher?.User?.FullName?.Contains(search) == true ||
                        cs.ClassRoom?.ClassName?.Contains(search) == true);
                }

                // ✅ ترتيب النتائج
                schedules = schedules
                    .OrderBy(cs => cs.DayOfWeek)
                    .ThenBy(cs => cs.StartTime);

                // ✅ تطبيق التصفح (Pagination)
                var totalCount = schedules.Count();
                var pagedItems = schedules
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToList();

                var dtos = await MapToDtoListAsync(pagedItems);

                _logger.LogInformation("✅ تم جلب {Count} حصة من أصل {Total}", pagedItems.Count, totalCount);
                return ResponseDto<IEnumerable<ClassScheduleDto>>.Ok(dtos, "تم جلب الحصص بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في GetFilteredAsync");
                return ResponseDto<IEnumerable<ClassScheduleDto>>.Fail(
                    "حدث خطأ أثناء جلب الحصص", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على جداول فصل معين (مع إمكانية تحديد السنة)
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ClassScheduleDto>>> GetByClassRoomIdAsync(
            int classRoomId, int? academicYearId = null)
        {
            try
            {
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(classRoomId);
                if (classRoom == null)
                {
                    return ResponseDto<IEnumerable<ClassScheduleDto>>.NotFound("الفصل غير موجود");
                }

                // ✅ بناء شرط البحث
                Expression<Func<ClassSchedule, bool>> predicate = cs => cs.ClassRoomId == classRoomId;

                if (academicYearId.HasValue)
                {
                    predicate = predicate.AndAlso(cs => cs.AcademicYearId == academicYearId.Value);
                }

                var schedules = await _unitOfWork.ClassSchedules
                    .FindAsync(predicate);

                var sortedSchedules = schedules
                    .OrderBy(cs => cs.DayOfWeek)
                    .ThenBy(cs => cs.StartTime)
                    .ToList();

                var dtos = await MapToDtoListAsync(sortedSchedules);

                _logger.LogInformation("✅ تم جلب {Count} حصة للفصل {ClassRoomId}", dtos.Count(), classRoomId);
                return ResponseDto<IEnumerable<ClassScheduleDto>>.Ok(dtos, "تم جلب حصص الفصل بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في GetByClassRoomIdAsync للفصل {ClassRoomId}", classRoomId);
                return ResponseDto<IEnumerable<ClassScheduleDto>>.Fail(
                    "حدث خطأ أثناء جلب حصص الفصل", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على جداول معلم معين (مع إمكانية تحديد السنة)
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ClassScheduleDto>>> GetByTeacherIdAsync(
            int teacherId, int? academicYearId = null)
        {
            try
            {
                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(teacherId);
                if (teacher == null)
                {
                    return ResponseDto<IEnumerable<ClassScheduleDto>>.NotFound("المعلم غير موجود");
                }

                // ✅ بناء شرط البحث
                Expression<Func<ClassSchedule, bool>> predicate = cs => cs.TeacherId == teacherId;

                if (academicYearId.HasValue)
                {
                    predicate = predicate.AndAlso(cs => cs.AcademicYearId == academicYearId.Value);
                }

                var schedules = await _unitOfWork.ClassSchedules
                    .FindAsync(predicate);

                var sortedSchedules = schedules
                    .OrderBy(cs => cs.DayOfWeek)
                    .ThenBy(cs => cs.StartTime)
                    .ToList();

                var dtos = await MapToDtoListAsync(sortedSchedules);

                _logger.LogInformation("✅ تم جلب {Count} حصة للمعلم {TeacherId}", dtos.Count(), teacherId);
                return ResponseDto<IEnumerable<ClassScheduleDto>>.Ok(dtos, "تم جلب حصص المعلم بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في GetByTeacherIdAsync للمعلم {TeacherId}", teacherId);
                return ResponseDto<IEnumerable<ClassScheduleDto>>.Fail(
                    "حدث خطأ أثناء جلب حصص المعلم", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على جداول مادة معينة (مع إمكانية تحديد السنة)
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ClassScheduleDto>>> GetBySubjectIdAsync(
            int subjectId, int? academicYearId = null)
        {
            try
            {
                var subject = await _unitOfWork.Subjects.GetByIdAsync(subjectId);
                if (subject == null)
                {
                    return ResponseDto<IEnumerable<ClassScheduleDto>>.NotFound("المادة غير موجودة");
                }

                // ✅ بناء شرط البحث
                Expression<Func<ClassSchedule, bool>> predicate = cs => cs.SubjectId == subjectId;

                if (academicYearId.HasValue)
                {
                    predicate = predicate.AndAlso(cs => cs.AcademicYearId == academicYearId.Value);
                }

                var schedules = await _unitOfWork.ClassSchedules
                    .FindAsync(predicate);

                var sortedSchedules = schedules
                    .OrderBy(cs => cs.DayOfWeek)
                    .ThenBy(cs => cs.StartTime)
                    .ToList();

                var dtos = await MapToDtoListAsync(sortedSchedules);

                _logger.LogInformation("✅ تم جلب {Count} حصة للمادة {SubjectId}", dtos.Count(), subjectId);
                return ResponseDto<IEnumerable<ClassScheduleDto>>.Ok(dtos, "تم جلب حصص المادة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في GetBySubjectIdAsync للمادة {SubjectId}", subjectId);
                return ResponseDto<IEnumerable<ClassScheduleDto>>.Fail(
                    "حدث خطأ أثناء جلب حصص المادة", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على جداول عام دراسي معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ClassScheduleDto>>> GetByAcademicYearIdAsync(int academicYearId)
        {
            try
            {
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(academicYearId);
                if (academicYear == null)
                {
                    return ResponseDto<IEnumerable<ClassScheduleDto>>.NotFound("السنة الدراسية غير موجودة");
                }

                var schedules = await _unitOfWork.ClassSchedules
                    .FindAsync(cs => cs.AcademicYearId == academicYearId);

                var sortedSchedules = schedules
                    .OrderBy(cs => cs.DayOfWeek)
                    .ThenBy(cs => cs.StartTime)
                    .ToList();

                var dtos = await MapToDtoListAsync(sortedSchedules);

                _logger.LogInformation("✅ تم جلب {Count} حصة للسنة {AcademicYearId}", dtos.Count(), academicYearId);
                return ResponseDto<IEnumerable<ClassScheduleDto>>.Ok(dtos, "تم جلب حصص السنة الدراسية بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في GetByAcademicYearIdAsync للسنة {AcademicYearId}", academicYearId);
                return ResponseDto<IEnumerable<ClassScheduleDto>>.Fail(
                    "حدث خطأ أثناء جلب حصص السنة الدراسية", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الجدول الأسبوعي لفصل معين
        /// </summary>
        public async Task<ResponseDto<Dictionary<string, IEnumerable<ClassScheduleDto>>>> GetWeeklyScheduleAsync(
            int classRoomId, int? academicYearId = null)
        {
            try
            {
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(classRoomId);
                if (classRoom == null)
                {
                    return ResponseDto<Dictionary<string, IEnumerable<ClassScheduleDto>>>.NotFound("الفصل غير موجود");
                }

                var response = await GetByClassRoomIdAsync(classRoomId, academicYearId);
                if (!response.Success || response.Data == null)
                {
                    return ResponseDto<Dictionary<string, IEnumerable<ClassScheduleDto>>>.Fail(
                        response.Message ?? "حدث خطأ أثناء جلب الجدول", statusCode: 500);
                }

                var schedules = response.Data;

                var days = new[]
                {
                    DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday,
                    DayOfWeek.Saturday
                };

                var dayNames = new Dictionary<DayOfWeek, string>
                {
                    { DayOfWeek.Sunday, "الأحد" },
                    { DayOfWeek.Monday, "الإثنين" },
                    { DayOfWeek.Tuesday, "الثلاثاء" },
                    { DayOfWeek.Wednesday, "الأربعاء" },
                    { DayOfWeek.Thursday, "الخميس" },
                    { DayOfWeek.Friday, "الجمعة" },
                    { DayOfWeek.Saturday, "السبت" }
                };

                var weeklySchedule = new Dictionary<string, IEnumerable<ClassScheduleDto>>();

                foreach (var day in days)
                {
                    var daySchedules = schedules
                        .Where(cs => cs.DayOfWeek == day)
                        .OrderBy(cs => cs.StartTime)
                        .ToList();

                    weeklySchedule[dayNames[day]] = daySchedules;
                }

                _logger.LogInformation("✅ تم جلب الجدول الأسبوعي للفصل {ClassRoomId}", classRoomId);
                return ResponseDto<Dictionary<string, IEnumerable<ClassScheduleDto>>>.Ok(
                    weeklySchedule, "تم جلب الجدول الأسبوعي بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في GetWeeklyScheduleAsync للفصل {ClassRoomId}", classRoomId);
                return ResponseDto<Dictionary<string, IEnumerable<ClassScheduleDto>>>.Fail(
                    "حدث خطأ أثناء جلب الجدول الأسبوعي", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على جداول الحصص للقوائم المنسدلة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<ClassScheduleLookupDto>>> GetLookupAsync(int? classRoomId = null)
        {
            try
            {
                IEnumerable<ClassSchedule> schedules;

                if (classRoomId.HasValue)
                {
                    schedules = await _unitOfWork.ClassSchedules
                        .FindAsync(cs => cs.ClassRoomId == classRoomId.Value && cs.IsActive);
                }
                else
                {
                    schedules = await _unitOfWork.ClassSchedules
                        .FindAsync(cs => cs.IsActive);
                }

                var dtos = new List<ClassScheduleLookupDto>();

                foreach (var schedule in schedules)
                {
                    var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(schedule.ClassRoomId);
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(schedule.SubjectId);
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(schedule.TeacherId);

                    dtos.Add(new ClassScheduleLookupDto
                    {
                        Id = schedule.Id,
                        ClassRoomId = schedule.ClassRoomId,
                        SubjectId = schedule.SubjectId,
                        TeacherId = schedule.TeacherId,
                        DayOfWeek = schedule.DayOfWeek,
                        ClassRoomName = classRoom?.ClassName,
                        SubjectName = subject?.SubjectName,
                        TeacherName = teacher?.User?.FullName,
                        DayName = GetDayName(schedule.DayOfWeek),
                        StartTime = schedule.StartTime,
                        EndTime = schedule.EndTime,
                        PeriodNumber = schedule.PeriodNumber,
                        IsActive = schedule.IsActive
                    });
                }

                _logger.LogInformation("✅ تم جلب {Count} حصة للقوائم", dtos.Count);
                return ResponseDto<IEnumerable<ClassScheduleLookupDto>>.Ok(dtos, "تم جلب البيانات للقوائم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في GetLookupAsync");
                return ResponseDto<IEnumerable<ClassScheduleLookupDto>>.Fail(
                    "حدث خطأ أثناء جلب البيانات", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على جدول بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<ClassScheduleDto>> GetByIdAsync(int id)
        {
            try
            {
                var schedule = await _unitOfWork.ClassSchedules.GetByIdAsync(id);

                if (schedule == null)
                {
                    return ResponseDto<ClassScheduleDto>.NotFound("الحصة غير موجودة");
                }

                var dto = await MapToDtoAsync(schedule);

                _logger.LogInformation("✅ تم جلب الحصة {Id}", id);
                return ResponseDto<ClassScheduleDto>.Ok(dto, "تم جلب الحصة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في GetByIdAsync للحصة {Id}", id);
                return ResponseDto<ClassScheduleDto>.Fail(
                    "حدث خطأ أثناء جلب الحصة", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ التحقق من التعارض ════════════════════════════════════

        /// <summary>
        /// ✅ التحقق من وجود تعارض في الوقت لنفس الفصل
        /// </summary>
        public async Task<ResponseDto<bool>> IsConflictExistsAsync(
            int classRoomId,
            int academicYearId,
            DayOfWeek dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime,
            int? excludeId = null)
        {
            try
            {
                var schedules = await _unitOfWork.ClassSchedules
                    .FindAsync(cs => cs.ClassRoomId == classRoomId
                        && cs.AcademicYearId == academicYearId
                        && cs.DayOfWeek == dayOfWeek
                        && cs.IsActive
                        && (excludeId == null || cs.Id != excludeId));

                var hasConflict = schedules.Any(cs =>
                    (startTime >= cs.StartTime && startTime < cs.EndTime) ||
                    (endTime > cs.StartTime && endTime <= cs.EndTime) ||
                    (startTime <= cs.StartTime && endTime >= cs.EndTime));

                if (hasConflict)
                {
                    _logger.LogWarning("⚠️ تعارض في الوقت للفصل {ClassRoomId} في اليوم {Day} من {Start} إلى {End}",
                        classRoomId, dayOfWeek, startTime, endTime);
                }

                return ResponseDto<bool>.Ok(hasConflict,
                    hasConflict ? "يوجد تعارض في الوقت مع حصة أخرى" : "لا يوجد تعارض في الوقت");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في IsConflictExistsAsync");
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق من التعارض", statusCode: 500);
            }
        }

        /// <summary>
        /// ✅ التحقق من وجود تعارض في وقت المعلم
        /// </summary>
        public async Task<ResponseDto<bool>> IsTeacherConflictExistsAsync(
            int teacherId,
            int academicYearId,
            DayOfWeek dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime,
            int? excludeId = null)
        {
            try
            {
                var schedules = await _unitOfWork.ClassSchedules
                    .FindAsync(cs => cs.TeacherId == teacherId
                        && cs.AcademicYearId == academicYearId
                        && cs.DayOfWeek == dayOfWeek
                        && cs.IsActive
                        && (excludeId == null || cs.Id != excludeId));

                var hasConflict = schedules.Any(cs =>
                    (startTime >= cs.StartTime && startTime < cs.EndTime) ||
                    (endTime > cs.StartTime && endTime <= cs.EndTime) ||
                    (startTime <= cs.StartTime && endTime >= cs.EndTime));

                if (hasConflict)
                {
                    _logger.LogWarning("⚠️ تعارض في وقت المعلم {TeacherId} في اليوم {Day} من {Start} إلى {End}",
                        teacherId, dayOfWeek, startTime, endTime);
                }

                return ResponseDto<bool>.Ok(hasConflict,
                    hasConflict ? "يوجد تعارض في وقت المعلم مع حصة أخرى" : "لا يوجد تعارض في وقت المعلم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في IsTeacherConflictExistsAsync");
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق من تعارض المعلم", statusCode: 500);
            }
        }

        /// <summary>
        /// ✅ التحقق من وجود تعارض في رقم الحصة لنفس الفصل
        /// </summary>
        public async Task<ResponseDto<bool>> IsPeriodConflictExistsAsync(
            int classRoomId,
            int academicYearId,
            DayOfWeek dayOfWeek,
            int periodNumber,
            int? excludeId = null)
        {
            try
            {
                var hasConflict = await _unitOfWork.ClassSchedules
                    .AnyAsync(cs => cs.ClassRoomId == classRoomId
                        && cs.AcademicYearId == academicYearId
                        && cs.DayOfWeek == dayOfWeek
                        && cs.PeriodNumber == periodNumber
                        && cs.IsActive
                        && (excludeId == null || cs.Id != excludeId));

                if (hasConflict)
                {
                    _logger.LogWarning("⚠️ تعارض في رقم الحصة {PeriodNumber} للفصل {ClassRoomId} في اليوم {Day}",
                        periodNumber, classRoomId, dayOfWeek);
                }

                return ResponseDto<bool>.Ok(hasConflict,
                    hasConflict ? $"يوجد تعارض في رقم الحصة {periodNumber}" : "لا يوجد تعارض في رقم الحصة");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في IsPeriodConflictExistsAsync");
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق من تعارض رقم الحصة", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ العمليات الأساسية ════════════════════════════════════

        /// <summary>
        /// ➕ إنشاء جدول جديد
        /// </summary>
        public async Task<ResponseDto<ClassScheduleDto>> CreateAsync(CreateClassScheduleDto createDto)
        {
            try
            {
                // ✅ التحقق من وجود السنة الدراسية
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(createDto.AcademicYearId);
                if (academicYear == null)
                {
                    return ResponseDto<ClassScheduleDto>.NotFound("السنة الدراسية غير موجودة");
                }

                // ✅ التحقق من وجود الفصل
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(createDto.ClassRoomId);
                if (classRoom == null)
                {
                    return ResponseDto<ClassScheduleDto>.NotFound("الفصل غير موجود");
                }

                // ✅ التحقق من وجود المادة
                var subject = await _unitOfWork.Subjects.GetByIdAsync(createDto.SubjectId);
                if (subject == null)
                {
                    return ResponseDto<ClassScheduleDto>.NotFound("المادة غير موجودة");
                }

                // ✅ التحقق من وجود المعلم
                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(createDto.TeacherId);
                if (teacher == null)
                {
                    return ResponseDto<ClassScheduleDto>.NotFound("المعلم غير موجود");
                }

                // ✅ التحقق من تعارض الوقت للفصل
                var conflictResponse = await IsConflictExistsAsync(
                    createDto.ClassRoomId,
                    createDto.AcademicYearId,
                    createDto.DayOfWeek,
                    createDto.StartTime,
                    createDto.EndTime);

                if (conflictResponse.Success && conflictResponse.Data)
                {
                    return ResponseDto<ClassScheduleDto>.Fail("يوجد تعارض في الوقت مع حصة أخرى للفصل نفسه");
                }

                // ✅ التحقق من تعارض وقت المعلم
                var teacherConflictResponse = await IsTeacherConflictExistsAsync(
                    createDto.TeacherId,
                    createDto.AcademicYearId,
                    createDto.DayOfWeek,
                    createDto.StartTime,
                    createDto.EndTime);

                if (teacherConflictResponse.Success && teacherConflictResponse.Data)
                {
                    return ResponseDto<ClassScheduleDto>.Fail("يوجد تعارض في وقت المعلم مع حصة أخرى");
                }

                // ✅ التحقق من تعارض رقم الحصة (إذا كان موجوداً)
                if (createDto.PeriodNumber.HasValue)
                {
                    var periodConflictResponse = await IsPeriodConflictExistsAsync(
                        createDto.ClassRoomId,
                        createDto.AcademicYearId,
                        createDto.DayOfWeek,
                        createDto.PeriodNumber.Value);

                    if (periodConflictResponse.Success && periodConflictResponse.Data)
                    {
                        return ResponseDto<ClassScheduleDto>.Fail(
                            $"يوجد تعارض في رقم الحصة {createDto.PeriodNumber.Value}");
                    }
                }

                // ✅ إنشاء الحصة
                var schedule = _mapper.Map<ClassSchedule>(createDto);
                schedule.IsActive = true;
                schedule.CreatedAt = DateTime.Now;

                var created = await _unitOfWork.ClassSchedules.AddAsync(schedule);
                await _unitOfWork.CompleteAsync();

                // ✅ جلب البيانات الكاملة
                var result = await GetByIdAsync(created.Id);
                if (!result.Success)
                {
                    return ResponseDto<ClassScheduleDto>.Fail(
                        "تم إنشاء الحصة ولكن حدث خطأ في جلب البيانات", statusCode: 500);
                }

                _logger.LogInformation("✅ تم إنشاء حصة جديدة للمعلم {TeacherId} في الفصل {ClassRoomId}",
                    createDto.TeacherId, createDto.ClassRoomId);

                return ResponseDto<ClassScheduleDto>.Ok(result.Data!, "تم إنشاء الحصة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في CreateAsync");
                return ResponseDto<ClassScheduleDto>.Fail(
                    "حدث خطأ أثناء إنشاء الحصة", statusCode: 500);
            }
        }

        /// <summary>
        /// ✏️ تحديث بيانات جدول
        /// </summary>
        public async Task<ResponseDto<ClassScheduleDto>> UpdateAsync(int id, UpdateClassScheduleDto updateDto)
        {
            try
            {
                // ✅ جلب الحصة
                var schedule = await _unitOfWork.ClassSchedules.GetByIdAsync(id);
                if (schedule == null)
                {
                    return ResponseDto<ClassScheduleDto>.NotFound("الحصة غير موجودة");
                }

                // ✅ التحقق من التغييرات
                var hasChanges = false;

                if (updateDto.DayOfWeek.HasValue && updateDto.DayOfWeek.Value != schedule.DayOfWeek)
                {
                    schedule.DayOfWeek = updateDto.DayOfWeek.Value;
                    hasChanges = true;
                }

                if (updateDto.StartTime.HasValue && updateDto.StartTime.Value != schedule.StartTime)
                {
                    schedule.StartTime = updateDto.StartTime.Value;
                    hasChanges = true;
                }

                if (updateDto.EndTime.HasValue && updateDto.EndTime.Value != schedule.EndTime)
                {
                    schedule.EndTime = updateDto.EndTime.Value;
                    hasChanges = true;
                }

                if (updateDto.PeriodNumber.HasValue && updateDto.PeriodNumber.Value != schedule.PeriodNumber)
                {
                    schedule.PeriodNumber = updateDto.PeriodNumber.Value;
                    hasChanges = true;
                }

                if (!string.IsNullOrWhiteSpace(updateDto.Notes))
                {
                    schedule.Notes = updateDto.Notes;
                    hasChanges = true;
                }

                if (updateDto.IsActive && updateDto.IsActive != schedule.IsActive)
                {
                    schedule.IsActive = updateDto.IsActive;
                    hasChanges = true;
                }

                if (updateDto.AcademicYearId.HasValue && updateDto.AcademicYearId.Value != schedule.AcademicYearId)
                {
                    var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(updateDto.AcademicYearId.Value);
                    if (academicYear == null)
                    {
                        return ResponseDto<ClassScheduleDto>.NotFound("السنة الدراسية غير موجودة");
                    }
                    schedule.AcademicYearId = updateDto.AcademicYearId.Value;
                    hasChanges = true;
                }

                if (updateDto.ClassRoomId.HasValue && updateDto.ClassRoomId.Value != schedule.ClassRoomId)
                {
                    var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(updateDto.ClassRoomId.Value);
                    if (classRoom == null)
                    {
                        return ResponseDto<ClassScheduleDto>.NotFound("الفصل غير موجود");
                    }
                    schedule.ClassRoomId = updateDto.ClassRoomId.Value;
                    hasChanges = true;
                }

                if (updateDto.SubjectId.HasValue && updateDto.SubjectId.Value != schedule.SubjectId)
                {
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(updateDto.SubjectId.Value);
                    if (subject == null)
                    {
                        return ResponseDto<ClassScheduleDto>.NotFound("المادة غير موجودة");
                    }
                    schedule.SubjectId = updateDto.SubjectId.Value;
                    hasChanges = true;
                }

                if (updateDto.TeacherId.HasValue && updateDto.TeacherId.Value != schedule.TeacherId)
                {
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(updateDto.TeacherId.Value);
                    if (teacher == null)
                    {
                        return ResponseDto<ClassScheduleDto>.NotFound("المعلم غير موجود");
                    }
                    schedule.TeacherId = updateDto.TeacherId.Value;
                    hasChanges = true;
                }

                if (!hasChanges)
                {
                    var currentDto = await MapToDtoAsync(schedule);
                    return ResponseDto<ClassScheduleDto>.Ok(currentDto, "لا توجد تغييرات للحفظ");
                }

                // ✅ التحقق من التعارضات (إذا تغير الوقت أو اليوم أو الفصل)
                if (updateDto.DayOfWeek.HasValue || updateDto.StartTime.HasValue || updateDto.EndTime.HasValue ||
                    updateDto.ClassRoomId.HasValue || updateDto.AcademicYearId.HasValue)
                {
                    var currentClassRoomId = updateDto.ClassRoomId ?? schedule.ClassRoomId;
                    var currentAcademicYearId = updateDto.AcademicYearId ?? schedule.AcademicYearId;
                    var currentDay = updateDto.DayOfWeek ?? schedule.DayOfWeek;
                    var currentStart = updateDto.StartTime ?? schedule.StartTime;
                    var currentEnd = updateDto.EndTime ?? schedule.EndTime;

                    var conflictResponse = await IsConflictExistsAsync(
                        currentClassRoomId,
                        currentAcademicYearId,
                        currentDay,
                        currentStart,
                        currentEnd,
                        id);

                    if (conflictResponse.Success && conflictResponse.Data)
                    {
                        return ResponseDto<ClassScheduleDto>.Fail("يوجد تعارض في الوقت مع حصة أخرى للفصل نفسه");
                    }

                    var teacherConflictResponse = await IsTeacherConflictExistsAsync(
                        schedule.TeacherId,
                        currentAcademicYearId,
                        currentDay,
                        currentStart,
                        currentEnd,
                        id);

                    if (teacherConflictResponse.Success && teacherConflictResponse.Data)
                    {
                        return ResponseDto<ClassScheduleDto>.Fail("يوجد تعارض في وقت المعلم مع حصة أخرى");
                    }

                    if (schedule.PeriodNumber.HasValue)
                    {
                        var periodConflictResponse = await IsPeriodConflictExistsAsync(
                            currentClassRoomId,
                            currentAcademicYearId,
                            currentDay,
                            schedule.PeriodNumber.Value,
                            id);

                        if (periodConflictResponse.Success && periodConflictResponse.Data)
                        {
                            return ResponseDto<ClassScheduleDto>.Fail(
                                $"يوجد تعارض في رقم الحصة {schedule.PeriodNumber.Value}");
                        }
                    }
                }

                schedule.UpdatedAt = DateTime.Now;
                await _unitOfWork.ClassSchedules.UpdateAsync(schedule);
                await _unitOfWork.CompleteAsync();

                var result = await GetByIdAsync(id);
                if (!result.Success)
                {
                    return ResponseDto<ClassScheduleDto>.Fail(
                        "تم تحديث الحصة ولكن حدث خطأ في جلب البيانات", statusCode: 500);
                }

                _logger.LogInformation("✅ تم تحديث الحصة {Id}", id);
                return ResponseDto<ClassScheduleDto>.Ok(result.Data!, "تم تحديث الحصة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في UpdateAsync للحصة {Id}", id);
                return ResponseDto<ClassScheduleDto>.Fail(
                    "حدث خطأ أثناء تحديث الحصة", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف جدول
        /// </summary>
        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var schedule = await _unitOfWork.ClassSchedules.GetByIdAsync(id);
                if (schedule == null)
                {
                    return ResponseDto.NotFound("الحصة غير موجودة");
                }

                await _unitOfWork.ClassSchedules.DeleteAsync(schedule);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("✅ تم حذف الحصة {Id}", id);
                return ResponseDto.Ok("تم حذف الحصة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في DeleteAsync للحصة {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف الحصة", statusCode: 500);
            }
        }

        /// <summary>
        /// ✅ تفعيل/تعطيل جدول
        /// </summary>
        public async Task<ResponseDto> ToggleStatusAsync(int id)
        {
            try
            {
                var schedule = await _unitOfWork.ClassSchedules.GetByIdAsync(id);
                if (schedule == null)
                {
                    return ResponseDto.NotFound("الحصة غير موجودة");
                }

                schedule.IsActive = !schedule.IsActive;
                schedule.UpdatedAt = DateTime.Now;

                await _unitOfWork.ClassSchedules.UpdateAsync(schedule);
                await _unitOfWork.CompleteAsync();

                var status = schedule.IsActive ? "مفعلة" : "غير مفعلة";
                _logger.LogInformation("✅ تم تغيير حالة الحصة {Id} إلى {Status}", id, status);

                return ResponseDto.Ok($"تم {status} الحصة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في ToggleStatusAsync للحصة {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء تغيير حالة الحصة", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ الإحصاءات ════════════════════════════════════

        /// <summary>
        /// 📊 الحصول على إحصاءات الحصص
        /// </summary>
        public async Task<ResponseDto<ClassScheduleStatisticsDto>> GetStatisticsAsync(int? academicYearId = null)
        {
            try
            {
                IEnumerable<ClassSchedule> schedules;

                if (academicYearId.HasValue)
                {
                    schedules = await _unitOfWork.ClassSchedules
                        .FindAsync(cs => cs.AcademicYearId == academicYearId.Value);
                }
                else
                {
                    schedules = await _unitOfWork.ClassSchedules.GetAllAsync();
                }

                if (!schedules.Any())
                {
                    return ResponseDto<ClassScheduleStatisticsDto>.Ok(
                        new ClassScheduleStatisticsDto(),
                        "لا توجد حصص لعرض الإحصاءات");
                }

                var dayNames = new Dictionary<DayOfWeek, string>
                {
                    { DayOfWeek.Sunday, "الأحد" },
                    { DayOfWeek.Monday, "الإثنين" },
                    { DayOfWeek.Tuesday, "الثلاثاء" },
                    { DayOfWeek.Wednesday, "الأربعاء" },
                    { DayOfWeek.Thursday, "الخميس" },
                    { DayOfWeek.Friday, "الجمعة" },
                    { DayOfWeek.Saturday, "السبت" }
                };

                // ✅ حساب الإحصاءات الأساسية
                var total = schedules.Count();
                var active = schedules.Count(cs => cs.IsActive);
                var inactive = total - active;

                // ✅ توزيع الحصص حسب الأيام
                var dailyDistribution = schedules
                    .GroupBy(cs => cs.DayOfWeek)
                    .ToDictionary(
                        g => dayNames.GetValueOrDefault(g.Key, g.Key.ToString()),
                        g => g.Count());

                // ✅ توزيع الحصص حسب المواد
                var subjectDistribution = new Dictionary<string, int>();
                foreach (var schedule in schedules)
                {
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(schedule.SubjectId);
                    var subjectName = subject?.SubjectName ?? "غير معروف";
                    if (subjectDistribution.ContainsKey(subjectName))
                        subjectDistribution[subjectName]++;
                    else
                        subjectDistribution[subjectName] = 1;
                }

                // ✅ أكثر وأقل المعلمين حصصاً
                var teacherHours = new Dictionary<string, int>();
                foreach (var schedule in schedules)
                {
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(schedule.TeacherId);
                    var teacherName = teacher?.User?.FullName ?? "غير معروف";
                    if (teacherHours.ContainsKey(teacherName))
                        teacherHours[teacherName]++;
                    else
                        teacherHours[teacherName] = 1;
                }

                var mostBusy = teacherHours.OrderByDescending(t => t.Value).FirstOrDefault();
                var leastBusy = teacherHours.OrderBy(t => t.Value).FirstOrDefault();

                // ✅ عدد الفصول والمعلمين والمواد
                var classRoomsCount = schedules.Select(cs => cs.ClassRoomId).Distinct().Count();
                var teachersCount = schedules.Select(cs => cs.TeacherId).Distinct().Count();
                var subjectsCount = schedules.Select(cs => cs.SubjectId).Distinct().Count();

                var statistics = new ClassScheduleStatisticsDto
                {
                    TotalWeeklyHours = total,
                    TotalSubjects = subjectsCount,
                    TotalTeachers = teachersCount,
                    ActiveSchedules = active,
                    InactiveSchedules = inactive,
                    DailyDistribution = dailyDistribution,
                    SubjectDistribution = subjectDistribution,
                    MostBusyTeacher = mostBusy.Key,
                    MostBusyTeacherHours = mostBusy.Value,
                    LeastBusyTeacher = leastBusy.Key,
                    LeastBusyTeacherHours = leastBusy.Value
                };

                _logger.LogInformation("✅ تم حساب إحصاءات الحصص بنجاح");
                return ResponseDto<ClassScheduleStatisticsDto>.Ok(statistics, "تم جلب الإحصاءات بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في GetStatisticsAsync");
                return ResponseDto<ClassScheduleStatisticsDto>.Fail(
                    "حدث خطأ أثناء جلب الإحصاءات", statusCode: 500);
            }
        }

        #endregion
    }

    /// <summary>
    /// ✅ ملحق لتجميع تعبيرات LINQ
    /// </summary>
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            var parameter = Expression.Parameter(typeof(T));
            var leftVisitor = new ReplaceExpressionVisitor(left.Parameters[0], parameter);
            var leftExpr = leftVisitor.Visit(left.Body);
            var rightVisitor = new ReplaceExpressionVisitor(right.Parameters[0], parameter);
            var rightExpr = rightVisitor.Visit(right.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(leftExpr!, rightExpr!), parameter);
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression? Visit(Expression? node)
            {
                return node == _oldValue ? _newValue : base.Visit(node);
            }
        }
    }
}