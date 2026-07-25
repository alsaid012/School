using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.EmployeeAttendances;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ✅  خدمة حضور الموظفين (EmployeeAttendanceService)
    /// </summary>
    public class EmployeeAttendanceService : IEmployeeAttendanceService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeAttendanceService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public EmployeeAttendanceService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<EmployeeAttendanceService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ الحصول على سجلات الحضور ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع سجلات حضور الموظفين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetAllAsync()
        {
            try
            {
                var attendances = await _unitOfWork.EmployeeAttendances.GetAllAsync();
                var dtos = new List<EmployeeAttendanceDto>();

                foreach (var attendance in attendances)
                {
                    var dto = _mapper.Map<EmployeeAttendanceDto>(attendance);
                    await PopulateEmployeeAttendanceDto(dto);
                    dtos.Add(dto);
                }

                _logger.LogInformation("تم جلب {Count} سجل حضور موظفين", dtos.Count);
                return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.Ok(dtos, "تم جلب سجلات الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جميع سجلات حضور الموظفين");
                return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.Fail("حدث خطأ أثناء جلب سجلات الحضور", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على سجلات حضور موظف معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetByEmployeeIdAsync(int employeeId)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
                if (employee == null)
                {
                    return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.NotFound("الموظف غير موجود");
                }

                var attendances = await _unitOfWork.EmployeeAttendances
                    .FindAsync(ea => ea.EmployeeId == employeeId);

                var dtos = new List<EmployeeAttendanceDto>();

                foreach (var attendance in attendances)
                {
                    var dto = _mapper.Map<EmployeeAttendanceDto>(attendance);
                    await PopulateEmployeeAttendanceDto(dto);
                    dtos.Add(dto);
                }

                return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.Ok(dtos, "تم جلب سجلات الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب سجلات حضور الموظف {EmployeeId}", employeeId);
                return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على سجلات حضور قسم معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetByDepartmentAsync(string department)
        {
            try
            {
                if (string.IsNullOrEmpty(department))
                {
                    return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.Fail("يرجى تحديد القسم");
                }

                var attendances = await _unitOfWork.EmployeeAttendances
                    .FindAsync(ea => ea.Employee.Department == department);

                var dtos = new List<EmployeeAttendanceDto>();

                foreach (var attendance in attendances)
                {
                    var dto = _mapper.Map<EmployeeAttendanceDto>(attendance);
                    await PopulateEmployeeAttendanceDto(dto);
                    dtos.Add(dto);
                }

                return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.Ok(dtos, "تم جلب سجلات الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب سجلات حضور القسم {Department}", department);
                return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على سجلات حضور في تاريخ محدد
        /// </summary>
        public async Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetByDateAsync(DateTime date)
        {
            try
            {
                var attendances = await _unitOfWork.EmployeeAttendances
                    .FindAsync(ea => ea.AttendanceDate.Date == date.Date);

                var dtos = new List<EmployeeAttendanceDto>();

                foreach (var attendance in attendances)
                {
                    var dto = _mapper.Map<EmployeeAttendanceDto>(attendance);
                    await PopulateEmployeeAttendanceDto(dto);
                    dtos.Add(dto);
                }

                return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.Ok(dtos, "تم جلب سجلات الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب سجلات الحضور في تاريخ {Date}", date);
                return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على سجلات حضور مع فلترة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetFilteredAsync(EmployeeAttendanceFilterDto filter)
        {
            try
            {
                var attendances = await _unitOfWork.EmployeeAttendances
                    .FindAsync(ea =>
                        (filter.EmployeeId == null || ea.EmployeeId == filter.EmployeeId) &&
                        (string.IsNullOrEmpty(filter.Department) || ea.Employee.Department == filter.Department) &&
                        (filter.Status == null || ea.Status == filter.Status) &&
                        (filter.FromDate == null || ea.AttendanceDate >= filter.FromDate) &&
                        (filter.ToDate == null || ea.AttendanceDate <= filter.ToDate) &&
                        (filter.IsActive == null || ea.IsActive == filter.IsActive)
                    );

                var dtos = new List<EmployeeAttendanceDto>();

                foreach (var attendance in attendances)
                {
                    var dto = _mapper.Map<EmployeeAttendanceDto>(attendance);
                    await PopulateEmployeeAttendanceDto(dto);
                    dtos.Add(dto);
                }

                return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.Ok(dtos, "تم جلب سجلات الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب سجلات الحضور المفلترة");
                return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ البحث عن سجل حضور ════════════════════════════════════

        /// <summary>
        /// 🔍 الحصول على سجل حضور بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<EmployeeAttendanceDto>> GetByIdAsync(int id)
        {
            try
            {
                var attendance = await _unitOfWork.EmployeeAttendances.GetByIdAsync(id);
                if (attendance == null)
                {
                    return ResponseDto<EmployeeAttendanceDto>.NotFound("سجل الحضور غير موجود");
                }

                var dto = _mapper.Map<EmployeeAttendanceDto>(attendance);
                await PopulateEmployeeAttendanceDto(dto);

                return ResponseDto<EmployeeAttendanceDto>.Ok(dto, "تم جلب سجل الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب سجل الحضور {Id}", id);
                return ResponseDto<EmployeeAttendanceDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على سجل حضور موظف في تاريخ محدد
        /// </summary>
        public async Task<ResponseDto<EmployeeAttendanceDto>> GetByEmployeeAndDateAsync(int employeeId, DateTime date)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
                if (employee == null)
                {
                    return ResponseDto<EmployeeAttendanceDto>.NotFound("الموظف غير موجود");
                }

                var attendances = await _unitOfWork.EmployeeAttendances
                    .FindAsync(ea => ea.EmployeeId == employeeId && ea.AttendanceDate.Date == date.Date);

                var attendance = attendances.FirstOrDefault();

                if (attendance == null)
                {
                    return ResponseDto<EmployeeAttendanceDto>.NotFound("لا يوجد سجل حضور للموظف في هذا التاريخ");
                }

                var dto = _mapper.Map<EmployeeAttendanceDto>(attendance);
                await PopulateEmployeeAttendanceDto(dto);

                return ResponseDto<EmployeeAttendanceDto>.Ok(dto, "تم جلب سجل الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب سجل حضور الموظف {EmployeeId} في تاريخ {Date}", employeeId, date);
                return ResponseDto<EmployeeAttendanceDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ الإحصائيات والتقارير ════════════════════════════════════

        /// <summary>
        /// 📊 الحصول على إحصائيات حضور موظف معين
        /// </summary>
        public async Task<ResponseDto<EmployeeAttendanceStatisticsDto>> GetStatisticsAsync(int employeeId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
                if (employee == null)
                {
                    return ResponseDto<EmployeeAttendanceStatisticsDto>.NotFound("الموظف غير موجود");
                }

                var attendances = await _unitOfWork.EmployeeAttendances
                    .FindAsync(ea => ea.EmployeeId == employeeId && ea.AttendanceDate >= fromDate && ea.AttendanceDate <= toDate);

                var totalDays = attendances.Count();
                var presentDays = attendances.Count(a => a.Status == AttendanceStatus.Present);
                var absentDays = attendances.Count(a => a.Status == AttendanceStatus.Absent);
                var lateDays = attendances.Count(a => a.Status == AttendanceStatus.Late);
                var excusedDays = attendances.Count(a => a.Status == AttendanceStatus.Excused);

                var statistics = new EmployeeAttendanceStatisticsDto
                {
                    TotalAttendanceDays = totalDays,
                    PresentDays = presentDays,
                    AbsentDays = absentDays,
                    LateDays = lateDays,
                    ExcusedDays = excusedDays,
                    AttendancePercentage = totalDays > 0 ? (decimal)presentDays / totalDays * 100 : 0,
                    FullAttendanceEmployees = 0,
                    FrequentAbsentEmployees = 0,
                    AttendanceByDepartment = new Dictionary<string, DepartmentAttendanceSummaryDto>()
                };

                return ResponseDto<EmployeeAttendanceStatisticsDto>.Ok(statistics, "تم جلب إحصائيات الحضور");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب إحصائيات حضور الموظف {EmployeeId}", employeeId);
                return ResponseDto<EmployeeAttendanceStatisticsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على تقرير الحضور اليومي للموظفين
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

                var attendances = await _unitOfWork.EmployeeAttendances
                    .FindAsync(ea => ea.Employee.User.SchoolId == schoolId && ea.AttendanceDate.Date == date.Date);

                var total = attendances.Count();
                var present = attendances.Count(ea => ea.Status == AttendanceStatus.Present);
                var absent = attendances.Count(ea => ea.Status == AttendanceStatus.Absent);
                var late = attendances.Count(ea => ea.Status == AttendanceStatus.Late);
                var excused = attendances.Count(ea => ea.Status == AttendanceStatus.Excused);

                var byDepartment = attendances
                    .GroupBy(ea => ea.Employee.Department ?? "بدون قسم")
                    .Select(g => new
                    {
                        القسم = g.Key,
                        إجمالي = g.Count(),
                        حاضر = g.Count(ea => ea.Status == AttendanceStatus.Present),
                        غائب = g.Count(ea => ea.Status == AttendanceStatus.Absent),
                        متأخر = g.Count(ea => ea.Status == AttendanceStatus.Late),
                        معذور = g.Count(ea => ea.Status == AttendanceStatus.Excused)
                    })
                    .ToList();

                var report = new
                {
                    المدرسة = school.SchoolName,
                    التاريخ = date.ToString("yyyy-MM-dd"),
                    إجمالي_الموظفين = total,
                    حاضر = present,
                    غائب = absent,
                    متأخر = late,
                    معذور = excused,
                    نسبة_الحضور = total > 0 ? (decimal)present / total * 100 : 0,
                    تفاصيل_حسب_القسم = byDepartment
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

        #region ════════════════════════════════════ القوائم المنسدلة ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على سجلات الحضور للقوائم المنسدلة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<EmployeeAttendanceLookupDto>>> GetLookupAsync(int? employeeId = null)
        {
            try
            {
                IEnumerable<EmployeeAttendance> attendances;

                if (employeeId.HasValue)
                {
                    attendances = await _unitOfWork.EmployeeAttendances
                        .FindAsync(ea => ea.EmployeeId == employeeId.Value);
                }
                else
                {
                    attendances = await _unitOfWork.EmployeeAttendances.GetAllAsync();
                }

                var dtos = new List<EmployeeAttendanceLookupDto>();

                foreach (var attendance in attendances)
                {
                    var dto = _mapper.Map<EmployeeAttendanceLookupDto>(attendance);

                    var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(attendance.EmployeeId);
                    if (employee != null)
                    {
                        var user = await _unitOfWork.Users.GetByIdAsync(employee.UserId);
                        dto.EmployeeName = user?.FullName ?? employee.EmployeeCode;
                        dto.JobTitle = employee.JobTitle;

                        var school = await _unitOfWork.SchoolRepository.GetByIdAsync(user?.SchoolId ?? 0);
                        dto.SchoolName = school?.SchoolName;
                    }
                    dto.StatusName = GetAttendanceStatusName(attendance.Status);

                    dtos.Add(dto);
                }

                return ResponseDto<IEnumerable<EmployeeAttendanceLookupDto>>.Ok(dtos, "تم جلب البيانات للقوائم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب بيانات القوائم");
                return ResponseDto<IEnumerable<EmployeeAttendanceLookupDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ إنشاء وتحديث وحذف ════════════════════════════════════

        /// <summary>
        /// ➕ إنشاء سجل حضور موظف جديد
        /// </summary>
        public async Task<ResponseDto<EmployeeAttendanceDto>> CreateAsync(CreateEmployeeAttendanceDto createDto)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(createDto.EmployeeId);
                if (employee == null)
                {
                    return ResponseDto<EmployeeAttendanceDto>.Fail("الموظف غير موجود");
                }

                var existing = await _unitOfWork.EmployeeAttendances
                    .FindAsync(ea => ea.EmployeeId == createDto.EmployeeId && ea.AttendanceDate.Date == createDto.AttendanceDate.Date);

                if (existing.Any())
                {
                    return ResponseDto<EmployeeAttendanceDto>.Fail("يوجد سجل حضور لهذا الموظف في هذا التاريخ");
                }

                var attendance = _mapper.Map<EmployeeAttendance>(createDto);
                attendance.CreatedAt = DateTime.Now;
                attendance.IsActive = true;

                var created = await _unitOfWork.EmployeeAttendances.AddAsync(attendance);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<EmployeeAttendanceDto>(created);
                await PopulateEmployeeAttendanceDto(dto);

                _logger.LogInformation("تم إنشاء سجل حضور للموظف {EmployeeId} في تاريخ {Date}", createDto.EmployeeId, createDto.AttendanceDate);

                return ResponseDto<EmployeeAttendanceDto>.Ok(dto, "تم إنشاء سجل الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء سجل حضور جديد");
                return ResponseDto<EmployeeAttendanceDto>.Fail("حدث خطأ أثناء إنشاء سجل الحضور", statusCode: 500);
            }
        }

        /// <summary>
        /// ✏️ تحديث سجل حضور موظف
        /// </summary>
        public async Task<ResponseDto<EmployeeAttendanceDto>> UpdateAsync(int id, UpdateEmployeeAttendanceDto updateDto)
        {
            try
            {
                var attendance = await _unitOfWork.EmployeeAttendances.GetByIdAsync(id);
                if (attendance == null)
                {
                    return ResponseDto<EmployeeAttendanceDto>.NotFound("سجل الحضور غير موجود");
                }

                _mapper.Map(updateDto, attendance);
                attendance.UpdatedAt = DateTime.Now;

                await _unitOfWork.EmployeeAttendances.UpdateAsync(attendance);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<EmployeeAttendanceDto>(attendance);
                await PopulateEmployeeAttendanceDto(dto);

                _logger.LogInformation("تم تحديث سجل الحضور {Id}", id);
                return ResponseDto<EmployeeAttendanceDto>.Ok(dto, "تم تحديث سجل الحضور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تحديث سجل الحضور {Id}", id);
                return ResponseDto<EmployeeAttendanceDto>.Fail("حدث خطأ أثناء تحديث سجل الحضور", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف سجل حضور موظف
        /// </summary>
        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var attendance = await _unitOfWork.EmployeeAttendances.GetByIdAsync(id);
                if (attendance == null)
                {
                    return ResponseDto.NotFound("سجل الحضور غير موجود");
                }

                await _unitOfWork.EmployeeAttendances.DeleteAsync(attendance);
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
        /// 📝 تعبئة البيانات الإضافية في EmployeeAttendanceDto
        /// </summary>
        private async Task PopulateEmployeeAttendanceDto(EmployeeAttendanceDto dto)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(dto.EmployeeId);
            if (employee != null)
            {
                var user = await _unitOfWork.Users.GetByIdAsync(employee.UserId);
                dto.EmployeeName = user?.FullName ?? employee.EmployeeCode;
                dto.EmployeeCode = employee.EmployeeCode;
                dto.JobTitle = employee.JobTitle;
                dto.Department = employee.Department;

                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(user?.SchoolId ?? 0);
                dto.SchoolName = school?.SchoolName;
            }
            dto.StatusName = GetAttendanceStatusName(dto.Status);
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


//using AutoMapper;
//using Microsoft.Extensions.Logging;
//using SchoolERP.Application.DTOs.Common;
//using SchoolERP.Application.DTOs.EmployeeAttendances;
//using SchoolERP.Application.Interfaces;
//using SchoolERP.Application.Interfaces.Services;
//using SchoolERP.Domain.Entities;
//using SchoolERP.Domain.Enums;

//namespace SchoolERP.Application.Services
//{
//    /// <summary>
//    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
//    /// ✅  خدمة حضور الموظفين (EmployeeAttendanceService)
//    /// 📌  الوظيفة: تنفيذ عمليات إدارة حضور الموظفين
//    /// 📦  الاستخدام: في EmployeeAttendancesController
//    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
//    /// </summary>
//    public class EmployeeAttendanceService : IEmployeeAttendanceService
//    {
//        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;
//        private readonly ILogger<EmployeeAttendanceService> _logger;

//        #endregion

//        #region ════════════════════════════════════ البناء ════════════════════════════════════

//        public EmployeeAttendanceService(
//            IUnitOfWork unitOfWork,
//            IMapper mapper,
//            ILogger<EmployeeAttendanceService> logger)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//            _logger = logger;
//        }

//        #endregion

//        #region ════════════════════════════════════ الحصول على سجلات الحضور ════════════════════════════════════

//        /// <summary>
//        /// 📋 الحصول على جميع سجلات حضور الموظفين
//        /// </summary>
//        public async Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetAllAsync()
//        {
//            try
//            {
//                var attendances = await _unitOfWork.EmployeeAttendances.GetAllAsync();
//                var dtos = _mapper.Map<IEnumerable<EmployeeAttendanceDto>>(attendances);

//                foreach (var dto in dtos)
//                {
//                    await PopulateEmployeeAttendanceDto(dto);
//                }

//                _logger.LogInformation("تم جلب {Count} سجل حضور موظفين", dtos.Count());
//                return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.Ok(dtos, "تم جلب سجلات الحضور بنجاح");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ أثناء جلب جميع سجلات حضور الموظفين");
//                return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.Fail("حدث خطأ أثناء جلب سجلات الحضور", statusCode: 500);
//            }
//        }

//        /// <summary>
//        /// 📋 الحصول على سجلات حضور موظف معين
//        /// </summary>
//        public async Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetByEmployeeIdAsync(int employeeId)
//        {
//            try
//            {
//                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
//                if (employee == null)
//                {
//                    return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.NotFound("الموظف غير موجود");
//                }

//                var attendances = await _unitOfWork.EmployeeAttendances
//                    .FindAsync(ea => ea.EmployeeId == employeeId);
//                var dtos = _mapper.Map<IEnumerable<EmployeeAttendanceDto>>(attendances);

//                foreach (var dto in dtos)
//                {
//                    await PopulateEmployeeAttendanceDto(dto);
//                }

//                return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.Ok(dtos, "تم جلب سجلات الحضور بنجاح");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ أثناء جلب سجلات حضور الموظف {EmployeeId}", employeeId);
//                return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.Fail("حدث خطأ", statusCode: 500);
//            }
//        }

//        /// <summary>
//        /// 📋 الحصول على سجلات حضور مدرسة معينة في تاريخ محدد
//        /// </summary>
//        public async Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetBySchoolAndDateAsync(int schoolId, DateTime date)
//        {
//            try
//            {
//                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(schoolId);
//                if (school == null)
//                {
//                    return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.NotFound("المدرسة غير موجودة");
//                }

//                var attendances = await _unitOfWork.EmployeeAttendances
//                    .FindAsync(ea => ea.Employee != null && ea.Employee.User != null && ea.Employee.User.SchoolId == schoolId && ea.AttendanceDate.Date == date.Date);
//                var dtos = _mapper.Map<IEnumerable<EmployeeAttendanceDto>>(attendances);

//                foreach (var dto in dtos)
//                {
//                    await PopulateEmployeeAttendanceDto(dto);
//                }

//                return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.Ok(dtos, "تم جلب سجلات الحضور بنجاح");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ أثناء جلب سجلات حضور المدرسة {SchoolId} في تاريخ {Date}", schoolId, date);
//                return ResponseDto<IEnumerable<EmployeeAttendanceDto>>.Fail("حدث خطأ", statusCode: 500);
//            }
//        }

//        #endregion

//        #region ════════════════════════════════════ البحث عن سجل حضور ════════════════════════════════════

//        /// <summary>
//        /// 🔍 الحصول على سجل حضور بواسطة المعرف
//        /// </summary>
//        public async Task<ResponseDto<EmployeeAttendanceDto>> GetByIdAsync(int id)
//        {
//            try
//            {
//                var attendance = await _unitOfWork.EmployeeAttendances.GetByIdAsync(id);
//                if (attendance == null)
//                {
//                    return ResponseDto<EmployeeAttendanceDto>.NotFound("سجل الحضور غير موجود");
//                }

//                var dto = _mapper.Map<EmployeeAttendanceDto>(attendance);
//                await PopulateEmployeeAttendanceDto(dto);

//                return ResponseDto<EmployeeAttendanceDto>.Ok(dto, "تم جلب سجل الحضور بنجاح");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ أثناء جلب سجل الحضور {Id}", id);
//                return ResponseDto<EmployeeAttendanceDto>.Fail("حدث خطأ", statusCode: 500);
//            }
//        }

//        /// <summary>
//        /// 🔍 الحصول على سجل حضور موظف في تاريخ محدد
//        /// </summary>
//        public async Task<ResponseDto<EmployeeAttendanceDto>> GetByEmployeeAndDateAsync(int employeeId, DateTime date)
//        {
//            try
//            {
//                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
//                if (employee == null)
//                {
//                    return ResponseDto<EmployeeAttendanceDto>.NotFound("الموظف غير موجود");
//                }

//                var attendances = await _unitOfWork.EmployeeAttendances
//                    .FindAsync(ea => ea.EmployeeId == employeeId && ea.AttendanceDate.Date == date.Date);
//                var attendance = attendances.FirstOrDefault();

//                if (attendance == null)
//                {
//                    return ResponseDto<EmployeeAttendanceDto>.NotFound("لا يوجد سجل حضور للموظف في هذا التاريخ");
//                }

//                var dto = _mapper.Map<EmployeeAttendanceDto>(attendance);
//                await PopulateEmployeeAttendanceDto(dto);

//                return ResponseDto<EmployeeAttendanceDto>.Ok(dto, "تم جلب سجل الحضور بنجاح");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ أثناء جلب سجل حضور الموظف {EmployeeId} في تاريخ {Date}", employeeId, date);
//                return ResponseDto<EmployeeAttendanceDto>.Fail("حدث خطأ", statusCode: 500);
//            }
//        }

//        #endregion

//        #region ════════════════════════════════════ الإحصائيات ════════════════════════════════════

//        /// <summary>
//        /// 📊 الحصول على إحصائيات حضور موظف معين
//        /// </summary>
//        public async Task<ResponseDto<EmployeeAttendanceStatisticsDto>> GetStatisticsAsync(int employeeId, DateTime fromDate, DateTime toDate)
//        {
//            try
//            {
//                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
//                if (employee == null)
//                {
//                    return ResponseDto<EmployeeAttendanceStatisticsDto>.NotFound("الموظف غير موجود");
//                }

//                var attendances = await _unitOfWork.EmployeeAttendances
//                    .FindAsync(ea => ea.EmployeeId == employeeId && ea.AttendanceDate >= fromDate && ea.AttendanceDate <= toDate);

//                var totalDays = attendances.Count();
//                var presentDays = attendances.Count(a => a.Status == AttendanceStatus.Present);
//                var absentDays = attendances.Count(a => a.Status == AttendanceStatus.Absent);
//                var lateDays = attendances.Count(a => a.Status == AttendanceStatus.Late);
//                var excusedDays = attendances.Count(a => a.Status == AttendanceStatus.Excused);

//                var statistics = new EmployeeAttendanceStatisticsDto
//                {
//                    TotalAttendanceDays = totalDays,
//                    PresentDays = presentDays,
//                    AbsentDays = absentDays,
//                    LateDays = lateDays,
//                    ExcusedDays = excusedDays,
//                    AttendancePercentage = totalDays > 0 ? (decimal)presentDays / totalDays * 100 : 0,
//                    MaxAttendanceDays = presentDays,
//                    MinAttendanceDays = 0,
//                    AverageAttendanceDays = totalDays > 0 ? (decimal)presentDays / totalDays : 0,
//                    FullAttendanceEmployees = 0,
//                    FrequentAbsentEmployees = 0,
//                    AttendanceByDepartment = new Dictionary<string, DepartmentAttendanceSummaryDto>()
//                };

//                return ResponseDto<EmployeeAttendanceStatisticsDto>.Ok(statistics, "تم جلب إحصائيات الحضور");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ أثناء جلب إحصائيات حضور الموظف {EmployeeId}", employeeId);
//                return ResponseDto<EmployeeAttendanceStatisticsDto>.Fail("حدث خطأ", statusCode: 500);
//            }
//        }

//        #endregion

//        #region ════════════════════════════════════ إنشاء وتحديث وحذف ════════════════════════════════════

//        /// <summary>
//        /// ➕ إنشاء سجل حضور موظف جديد
//        /// </summary>
//        public async Task<ResponseDto<EmployeeAttendanceDto>> CreateAsync(CreateEmployeeAttendanceDto createDto)
//        {
//            try
//            {
//                // التحقق من وجود الموظف
//                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(createDto.EmployeeId);
//                if (employee == null)
//                {
//                    return ResponseDto<EmployeeAttendanceDto>.Fail("الموظف غير موجود");
//                }

//                // التحقق من عدم وجود سجل مكرر
//                var existing = await _unitOfWork.EmployeeAttendances
//                    .FindAsync(ea => ea.EmployeeId == createDto.EmployeeId && ea.AttendanceDate.Date == createDto.AttendanceDate.Date);
//                if (existing.Any())
//                {
//                    return ResponseDto<EmployeeAttendanceDto>.Fail("يوجد سجل حضور لهذا الموظف في هذا التاريخ");
//                }

//                var attendance = _mapper.Map<EmployeeAttendance>(createDto);
//                attendance.CreatedAt = DateTime.Now;
//                attendance.IsActive = true;

//                var created = await _unitOfWork.EmployeeAttendances.AddAsync(attendance);
//                await _unitOfWork.CompleteAsync();

//                var dto = _mapper.Map<EmployeeAttendanceDto>(created);
//                await PopulateEmployeeAttendanceDto(dto);

//                _logger.LogInformation("تم إنشاء سجل حضور للموظف {EmployeeId} في تاريخ {Date}", createDto.EmployeeId, createDto.AttendanceDate);

//                return ResponseDto<EmployeeAttendanceDto>.Ok(dto, "تم إنشاء سجل الحضور بنجاح");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ أثناء إنشاء سجل حضور جديد");
//                return ResponseDto<EmployeeAttendanceDto>.Fail("حدث خطأ أثناء إنشاء سجل الحضور", statusCode: 500);
//            }
//        }

//        /// <summary>
//        /// ✏️ تحديث سجل حضور موظف
//        /// </summary>
//        public async Task<ResponseDto<EmployeeAttendanceDto>> UpdateAsync(int id, UpdateEmployeeAttendanceDto updateDto)
//        {
//            try
//            {
//                var attendance = await _unitOfWork.EmployeeAttendances.GetByIdAsync(id);
//                if (attendance == null)
//                {
//                    return ResponseDto<EmployeeAttendanceDto>.NotFound("سجل الحضور غير موجود");
//                }

//                _mapper.Map(updateDto, attendance);
//                attendance.UpdatedAt = DateTime.Now;

//                await _unitOfWork.EmployeeAttendances.UpdateAsync(attendance);
//                await _unitOfWork.CompleteAsync();

//                var dto = _mapper.Map<EmployeeAttendanceDto>(attendance);
//                await PopulateEmployeeAttendanceDto(dto);

//                _logger.LogInformation("تم تحديث سجل الحضور {Id}", id);
//                return ResponseDto<EmployeeAttendanceDto>.Ok(dto, "تم تحديث سجل الحضور بنجاح");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ أثناء تحديث سجل الحضور {Id}", id);
//                return ResponseDto<EmployeeAttendanceDto>.Fail("حدث خطأ أثناء تحديث سجل الحضور", statusCode: 500);
//            }
//        }

//        /// <summary>
//        /// 🗑️ حذف سجل حضور موظف
//        /// </summary>
//        public async Task<ResponseDto> DeleteAsync(int id)
//        {
//            try
//            {
//                var attendance = await _unitOfWork.EmployeeAttendances.GetByIdAsync(id);
//                if (attendance == null)
//                {
//                    return ResponseDto.NotFound("سجل الحضور غير موجود");
//                }

//                await _unitOfWork.EmployeeAttendances.DeleteAsync(attendance);
//                await _unitOfWork.CompleteAsync();

//                _logger.LogInformation("تم حذف سجل الحضور {Id}", id);
//                return ResponseDto.Ok("تم حذف سجل الحضور بنجاح");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "خطأ أثناء حذف سجل الحضور {Id}", id);
//                return ResponseDto.Fail("حدث خطأ أثناء حذف سجل الحضور", statusCode: 500);
//            }
//        }

//        #endregion

//        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

//        /// <summary>
//        /// 📝 تعبئة البيانات الإضافية في EmployeeAttendanceDto
//        /// </summary>
//        private async Task PopulateEmployeeAttendanceDto(EmployeeAttendanceDto dto)
//        {
//            var employee = await _unitOfWork.EmployeeRepository.GetWithDetailsAsync(dto.EmployeeId);
//            dto.EmployeeName = employee?.User?.FullName;
//            dto.EmployeeCode = employee?.EmployeeCode;
//            dto.JobTitle = employee?.JobTitle;
//            dto.StatusName = GetAttendanceStatusName(dto.Status);
//            dto.SchoolName = employee?.User?.School?.SchoolName;
//            dto.Department = employee?.Department;
//        }

//        /// <summary>
//        /// 📝 الحصول على اسم حالة الحضور بالعربية
//        /// </summary>
//        private string GetAttendanceStatusName(AttendanceStatus status)
//        {
//            return status switch
//            {
//                AttendanceStatus.Present => "حاضر",
//                AttendanceStatus.Absent => "غائب",
//                AttendanceStatus.Late => "متأخر",
//                AttendanceStatus.Excused => "معذور",
//                _ => status.ToString()
//            };
//        }

//        #endregion
//    }
//}