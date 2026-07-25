using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.StudentAttendances;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✅  خدمة حضور الطلاب (StudentAttendanceService)
    /// 📌  الوظيفة: تنفيذ عمليات إدارة حضور الطلاب
    /// 📦  الاستخدام: في StudentAttendancesController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class StudentAttendanceService : IStudentAttendanceService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentAttendanceService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public StudentAttendanceService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<StudentAttendanceService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ الحصول على سجلات الحضور ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع سجلات حضور الطلاب
        /// </summary>
        public async Task<ResponseDto<IEnumerable<StudentAttendanceDto>>> GetAllAsync()
        {
            try
            {
                // ✅ جلب جميع سجلات الحضور مع البيانات المرتبطة دفعة واحدة
                var attendances = await _unitOfWork.StudentAttendances
                    .GetAllWithDetailsAsync(
                        sa => sa.Student,
                        sa => sa.Student.User,
                        sa => sa.Student.ClassRoom,
                        sa => sa.Student.ClassRoom.GradeLevel
                    );

                var dtos = new List<StudentAttendanceDto>();

                foreach (var attendance in attendances)
                {
                    var dto = _mapper.Map<StudentAttendanceDto>(attendance);

                    // ✅ البيانات موجودة بالفعل، لا حاجة لجلبها مرة أخرى
                    dto.StudentName = attendance.Student?.User?.FullName ?? "غير معروف";
                    dto.StudentCode = attendance.Student?.StudentCode ?? string.Empty;
                    dto.ClassRoomName = attendance.Student?.ClassRoom?.ClassName ?? string.Empty;
                    dto.GradeLevelName = attendance.Student?.ClassRoom?.GradeLevel?.GradeName ?? string.Empty;
                    dto.StatusName = GetAttendanceStatusName(dto.Status);

                    dtos.Add(dto);
                }

                _logger.LogInformation("تم جلب {Count} سجل حضور طلاب", dtos.Count);
                return ResponseDto<IEnumerable<StudentAttendanceDto>>.Ok(dtos, "تم جلب سجلات الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جميع سجلات حضور الطلاب");
                return ResponseDto<IEnumerable<StudentAttendanceDto>>.Fail("حدث خطأ أثناء جلب سجلات الحضور", statusCode: 500);
            }
        }


        ///// <summary>
        ///// 📋 الحصول على جميع سجلات حضور الطلاب
        ///// </summary>
        //public async Task<ResponseDto<IEnumerable<StudentAttendanceDto>>> GetAllAsync()
        //{
        //    try
        //    {
        //        var attendances = await _unitOfWork.StudentAttendances.GetAllAsync();
        //        var dtos = _mapper.Map<IEnumerable<StudentAttendanceDto>>(attendances);

        //        foreach (var dto in dtos)
        //        {
        //            await PopulateStudentAttendanceDto(dto);
        //        }

        //        _logger.LogInformation("تم جلب {Count} سجل حضور طلاب", dtos.Count());
        //        return ResponseDto<IEnumerable<StudentAttendanceDto>>.Ok(dtos, "تم جلب سجلات الحضور بنجاح");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "خطأ أثناء جلب جميع سجلات حضور الطلاب");
        //        return ResponseDto<IEnumerable<StudentAttendanceDto>>.Fail("حدث خطأ أثناء جلب سجلات الحضور", statusCode: 500);
        //    }
        //}

        /// <summary>
        /// 📋 الحصول على سجلات حضور طالب معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<StudentAttendanceDto>>> GetByStudentIdAsync(int studentId)
        {
            try
            {
                var student = await _unitOfWork.Students.GetByIdAsync(studentId);
                if (student == null)
                {
                    return ResponseDto<IEnumerable<StudentAttendanceDto>>.NotFound("الطالب غير موجود");
                }

                var attendances = await _unitOfWork.StudentAttendances
                    .FindAsync(sa => sa.StudentId == studentId);
                var dtos = _mapper.Map<IEnumerable<StudentAttendanceDto>>(attendances);

                foreach (var dto in dtos)
                {
                    await PopulateStudentAttendanceDto(dto);
                }

                return ResponseDto<IEnumerable<StudentAttendanceDto>>.Ok(dtos, "تم جلب سجلات الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب سجلات حضور الطالب {StudentId}", studentId);
                return ResponseDto<IEnumerable<StudentAttendanceDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على سجلات حضور فصل معين في تاريخ محدد
        /// </summary>
        public async Task<ResponseDto<IEnumerable<StudentAttendanceDto>>> GetByClassRoomAndDateAsync(int classRoomId, DateTime date)
        {
            try
            {
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(classRoomId);
                if (classRoom == null)
                {
                    return ResponseDto<IEnumerable<StudentAttendanceDto>>.NotFound("الفصل غير موجود");
                }

                var attendances = await _unitOfWork.StudentAttendances
                    .FindAsync(sa => sa.Student.ClassRoomId == classRoomId && sa.AttendanceDate.Date == date.Date);
                var dtos = _mapper.Map<IEnumerable<StudentAttendanceDto>>(attendances);

                foreach (var dto in dtos)
                {
                    await PopulateStudentAttendanceDto(dto);
                    dto.ClassRoomName = classRoom.ClassName;
                }

                return ResponseDto<IEnumerable<StudentAttendanceDto>>.Ok(dtos, "تم جلب سجلات الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب سجلات حضور الفصل {ClassRoomId} في تاريخ {Date}", classRoomId, date);
                return ResponseDto<IEnumerable<StudentAttendanceDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على سجلات حضور صف معين في تاريخ محدد
        /// </summary>
        public async Task<ResponseDto<IEnumerable<StudentAttendanceDto>>> GetByGradeLevelAndDateAsync(int gradeLevelId, DateTime date)
        {
            try
            {
                var gradeLevel = await _unitOfWork.GradeLevels.GetByIdAsync(gradeLevelId);
                if (gradeLevel == null)
                {
                    return ResponseDto<IEnumerable<StudentAttendanceDto>>.NotFound("الصف غير موجود");
                }

                var attendances = await _unitOfWork.StudentAttendances
                     .FindAsync(sa => sa.Student != null &&
                       sa.Student.ClassRoom != null &&
                       sa.Student.ClassRoom.GradeLevelId == gradeLevelId &&
                       sa.AttendanceDate.Date == date.Date);
                var dtos = _mapper.Map<IEnumerable<StudentAttendanceDto>>(attendances);

                foreach (var dto in dtos)
                {
                    await PopulateStudentAttendanceDto(dto);
                }

                return ResponseDto<IEnumerable<StudentAttendanceDto>>.Ok(dtos, "تم جلب سجلات الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب سجلات حضور الصف {GradeLevelId} في تاريخ {Date}", gradeLevelId, date);
                return ResponseDto<IEnumerable<StudentAttendanceDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ البحث عن سجل حضور ════════════════════════════════════

        /// <summary>
        /// 🔍 الحصول على سجل حضور بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<StudentAttendanceDto>> GetByIdAsync(int id)
        {
            try
            {
                var attendance = await _unitOfWork.StudentAttendances.GetByIdAsync(id);
                if (attendance == null)
                {
                    return ResponseDto<StudentAttendanceDto>.NotFound("سجل الحضور غير موجود");
                }

                var dto = _mapper.Map<StudentAttendanceDto>(attendance);
                await PopulateStudentAttendanceDto(dto);

                return ResponseDto<StudentAttendanceDto>.Ok(dto, "تم جلب سجل الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب سجل الحضور {Id}", id);
                return ResponseDto<StudentAttendanceDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على سجل حضور طالب في تاريخ محدد
        /// </summary>
        public async Task<ResponseDto<StudentAttendanceDto>> GetByStudentAndDateAsync(int studentId, DateTime date)
        {
            try
            {
                var student = await _unitOfWork.Students.GetByIdAsync(studentId);
                if (student == null)
                {
                    return ResponseDto<StudentAttendanceDto>.NotFound("الطالب غير موجود");
                }

                var attendances = await _unitOfWork.StudentAttendances
                    .FindAsync(sa => sa.StudentId == studentId && sa.AttendanceDate.Date == date.Date);
                var attendance = attendances.FirstOrDefault();

                if (attendance == null)
                {
                    return ResponseDto<StudentAttendanceDto>.NotFound("لا يوجد سجل حضور للطالب في هذا التاريخ");
                }

                var dto = _mapper.Map<StudentAttendanceDto>(attendance);
                await PopulateStudentAttendanceDto(dto);

                return ResponseDto<StudentAttendanceDto>.Ok(dto, "تم جلب سجل الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب سجل حضور الطالب {StudentId} في تاريخ {Date}", studentId, date);
                return ResponseDto<StudentAttendanceDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ الإحصائيات والتقارير ════════════════════════════════════

        /// <summary>
        /// 📊 الحصول على إحصائيات حضور طالب معين
        /// </summary>
        public async Task<ResponseDto<StudentAttendanceStatisticsDto>> GetStatisticsAsync(int studentId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var student = await _unitOfWork.Students.GetByIdAsync(studentId);
                if (student == null)
                {
                    return ResponseDto<StudentAttendanceStatisticsDto>.NotFound("الطالب غير موجود");
                }

                var attendances = await _unitOfWork.StudentAttendances
                    .FindAsync(sa => sa.StudentId == studentId && sa.AttendanceDate >= fromDate && sa.AttendanceDate <= toDate);

                var totalDays = attendances.Count();
                var presentDays = attendances.Count(a => a.Status == AttendanceStatus.Present);
                var absentDays = attendances.Count(a => a.Status == AttendanceStatus.Absent);
                var lateDays = attendances.Count(a => a.Status == AttendanceStatus.Late);
                var excusedDays = attendances.Count(a => a.Status == AttendanceStatus.Excused);

                var statistics = new StudentAttendanceStatisticsDto
                {
                    TotalAttendanceDays = totalDays,
                    PresentDays = presentDays,
                    AbsentDays = absentDays,
                    LateDays = lateDays,
                    ExcusedDays = excusedDays,
                    AttendancePercentage = totalDays > 0 ? (decimal)presentDays / totalDays * 100 : 0,
                    MaxAttendanceDays = presentDays,
                    MinAttendanceDays = 0,
                    AverageAttendanceDays = totalDays > 0 ? (decimal)presentDays / totalDays : 0,
                    FullAttendanceStudents = 0,
                    FrequentAbsentStudents = 0,
                    AttendanceByClass = new Dictionary<string, ClassAttendanceSummaryDto>()
                };

                return ResponseDto<StudentAttendanceStatisticsDto>.Ok(statistics, "تم جلب إحصائيات الحضور");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب إحصائيات حضور الطالب {StudentId}", studentId);
                return ResponseDto<StudentAttendanceStatisticsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على تقرير الحضور اليومي لمدرسة معينة
        /// </summary>
        public async Task<ResponseDto<object>> GetDailyReportAsync(int schoolId, DateTime date)
        {
            try
            {
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(schoolId);
                if (school == null)
                {
                    return ResponseDto<object>.NotFound("المدرسة غير موجودة");
                }

                var attendances = await _unitOfWork.StudentAttendances
                    .FindAsync(sa => sa.Student.User.SchoolId == schoolId && sa.AttendanceDate.Date == date.Date);

                var total = attendances.Count();
                var present = attendances.Count(sa => sa.Status == AttendanceStatus.Present);
                var absent = attendances.Count(sa => sa.Status == AttendanceStatus.Absent);
                var late = attendances.Count(sa => sa.Status == AttendanceStatus.Late);
                var excused = attendances.Count(sa => sa.Status == AttendanceStatus.Excused);

                // تجميع حسب الفصول
                var byClass = attendances
                    .GroupBy(sa => sa.Student.ClassRoom?.ClassName ?? "بدون فصل")
                    .Select(g => new
                    {
                        الفصل = g.Key,
                        إجمالي = g.Count(),
                        حاضر = g.Count(sa => sa.Status == AttendanceStatus.Present),
                        غائب = g.Count(sa => sa.Status == AttendanceStatus.Absent),
                        متأخر = g.Count(sa => sa.Status == AttendanceStatus.Late),
                        معذور = g.Count(sa => sa.Status == AttendanceStatus.Excused)
                    })
                    .ToList();

                var report = new
                {
                    المدرسة = school.SchoolName,
                    التاريخ = date.ToString("yyyy-MM-dd"),
                    إجمالي_الطلاب = total,
                    حاضر = present,
                    غائب = absent,
                    متأخر = late,
                    معذور = excused,
                    نسبة_الحضور = total > 0 ? (decimal)present / total * 100 : 0,
                    تفاصيل_حسب_الفصل = byClass
                };

                return ResponseDto<object>.Ok(report, "تم جلب تقرير الحضور اليومي");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب تقرير الحضور اليومي للمدرسة {SchoolId} في تاريخ {Date}", schoolId, date);
                return ResponseDto<object>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ إنشاء وتحديث وحذف ════════════════════════════════════

        /// <summary>
        /// ➕ إنشاء سجل حضور طالب جديد
        /// </summary>
        public async Task<ResponseDto<StudentAttendanceDto>> CreateAsync(CreateStudentAttendanceDto createDto)
        {
            try
            {
                // التحقق من وجود الطالب
                var student = await _unitOfWork.Students.GetByIdAsync(createDto.StudentId);
                if (student == null)
                {
                    return ResponseDto<StudentAttendanceDto>.Fail("الطالب غير موجود");
                }

                // التحقق من عدم وجود سجل مكرر
                var existing = await _unitOfWork.StudentAttendances
                    .FindAsync(sa => sa.StudentId == createDto.StudentId && sa.AttendanceDate.Date == createDto.AttendanceDate.Date);
                if (existing.Any())
                {
                    return ResponseDto<StudentAttendanceDto>.Fail("يوجد سجل حضور لهذا الطالب في هذا التاريخ");
                }

                var attendance = _mapper.Map<StudentAttendance>(createDto);
                attendance.CreatedAt = DateTime.Now;
                attendance.IsActive = true;

                var created = await _unitOfWork.StudentAttendances.AddAsync(attendance);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<StudentAttendanceDto>(created);
                await PopulateStudentAttendanceDto(dto);

                _logger.LogInformation("تم إنشاء سجل حضور للطالب {StudentId} في تاريخ {Date}", createDto.StudentId, createDto.AttendanceDate);

                return ResponseDto<StudentAttendanceDto>.Ok(dto, "تم إنشاء سجل الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء سجل حضور جديد");
                return ResponseDto<StudentAttendanceDto>.Fail("حدث خطأ أثناء إنشاء سجل الحضور", statusCode: 500);
            }
        }

        /// <summary>
        /// ✏️ تحديث سجل حضور طالب
        /// </summary>
        public async Task<ResponseDto<StudentAttendanceDto>> UpdateAsync(int id, UpdateStudentAttendanceDto updateDto)
        {
            try
            {
                var attendance = await _unitOfWork.StudentAttendances.GetByIdAsync(id);
                if (attendance == null)
                {
                    return ResponseDto<StudentAttendanceDto>.NotFound("سجل الحضور غير موجود");
                }

                _mapper.Map(updateDto, attendance);
                attendance.UpdatedAt = DateTime.Now;

                await _unitOfWork.StudentAttendances.UpdateAsync(attendance);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<StudentAttendanceDto>(attendance);
                await PopulateStudentAttendanceDto(dto);

                _logger.LogInformation("تم تحديث سجل الحضور {Id}", id);
                return ResponseDto<StudentAttendanceDto>.Ok(dto, "تم تحديث سجل الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تحديث سجل الحضور {Id}", id);
                return ResponseDto<StudentAttendanceDto>.Fail("حدث خطأ أثناء تحديث سجل الحضور", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف سجل حضور طالب
        /// </summary>
        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var attendance = await _unitOfWork.StudentAttendances.GetByIdAsync(id);
                if (attendance == null)
                {
                    return ResponseDto.NotFound("سجل الحضور غير موجود");
                }

                await _unitOfWork.StudentAttendances.DeleteAsync(attendance);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف سجل الحضور {Id}", id);
                return ResponseDto.Ok("تم حذف سجل الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء حذف سجل الحضور {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف سجل الحضور", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        /// <summary>
        /// 📝 تعبئة البيانات الإضافية في StudentAttendanceDto
        /// </summary>
        private async Task PopulateStudentAttendanceDto(StudentAttendanceDto dto)
        {
            var student = await _unitOfWork.Students.GetStudentWithDetailsAsync(dto.StudentId);
            dto.StudentName = student?.User?.FullName;
            dto.StudentCode = student?.StudentCode;
            dto.StatusName = GetAttendanceStatusName(dto.Status);
            dto.ClassRoomName = student?.ClassRoom?.ClassName;
            dto.GradeLevelName = student?.ClassRoom?.GradeLevel?.GradeName;
        }

        /// <summary>
        /// 📝 الحصول على اسم حالة الحضور بالعربية
        /// </summary>
        private string GetAttendanceStatusName(AttendanceStatus status)
        {
            return status switch
            {
                AttendanceStatus.Present => "حاضر",
                AttendanceStatus.Absent => "غائب",
                AttendanceStatus.Late => "متأخر",
                AttendanceStatus.Excused => "معذور",
                _ => status.ToString()
            };
        }

        #endregion
    }
}