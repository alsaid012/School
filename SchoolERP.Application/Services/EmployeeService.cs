using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Employees;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 👨‍💼  خدمة الموظفين (EmployeeService)
    /// 📌  الوظيفة: تنفيذ عمليات إدارة الموظفين
    /// 📦  الاستخدام: في EmployeesController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EmployeeService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ جلب البيانات ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع الموظفين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<EmployeeDto>>> GetAllAsync()
        {
            try
            {
                var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetUserWithDetailsAsync(dto.UserId);
                    if (user != null)
                    {
                        dto.FullName = user.FullName;
                        dto.Email = user.Email;

                        var phoneContact = user.Contacts?.FirstOrDefault(c => c.ContactType == ContactType.Phone || c.ContactType == ContactType.Mobile);
                        dto.PhoneNumber = phoneContact?.ContactValue;
                    }

                    var school = await _unitOfWork.SchoolRepository.GetByIdAsync(user?.SchoolId ?? 0);
                    dto.SchoolName = school?.SchoolName;
                }

                _logger.LogInformation("تم جلب {Count} موظف", dtos.Count());
                return ResponseDto<IEnumerable<EmployeeDto>>.Ok(dtos, "تم جلب الموظفين بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetAllAsync");
                return ResponseDto<IEnumerable<EmployeeDto>>.Fail("حدث خطأ أثناء جلب الموظفين", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على موظف بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<EmployeeDetailsDto>> GetByIdAsync(int id)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeRepository.GetWithDetailsAsync(id);
                if (employee == null)
                    return ResponseDto<EmployeeDetailsDto>.NotFound("الموظف غير موجود");

                var dto = _mapper.Map<EmployeeDetailsDto>(employee);

                var user = await _unitOfWork.Users.GetUserWithDetailsAsync(employee.UserId);
                if (user != null)
                {
                    dto.FullName = user.FullName;
                    dto.Email = user.Email;

                    var phoneContact = user.Contacts?.FirstOrDefault(c => c.ContactType == ContactType.Phone || c.ContactType == ContactType.Mobile);
                    dto.PhoneNumber = phoneContact?.ContactValue;
                }

                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(user?.SchoolId ?? 0);
                dto.SchoolName = school?.SchoolName;

                // جلب الإحصائيات
                dto.Statistics = await GetEmployeeStatisticsAsync(id);

                return ResponseDto<EmployeeDetailsDto>.Ok(dto, "تم جلب الموظف بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetByIdAsync للموظف {Id}", id);
                return ResponseDto<EmployeeDetailsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على موظف بواسطة الكود
        /// </summary>
        public async Task<ResponseDto<EmployeeDto>> GetByCodeAsync(string employeeCode)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeRepository.GetByCodeAsync(employeeCode);
                if (employee == null)
                    return ResponseDto<EmployeeDto>.NotFound("الموظف غير موجود");

                var dto = _mapper.Map<EmployeeDto>(employee);

                var user = await _unitOfWork.Users.GetByIdAsync(employee.UserId);
                dto.FullName = user?.FullName ?? string.Empty;

                return ResponseDto<EmployeeDto>.Ok(dto, "تم جلب الموظف بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetByCodeAsync للموظف {EmployeeCode}", employeeCode);
                return ResponseDto<EmployeeDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ البحث والفلترة ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على الموظفين في مدرسة معينة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<EmployeeDto>>> GetBySchoolIdAsync(int schoolId)
        {
            try
            {
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(schoolId);
                if (school == null)
                    return ResponseDto<IEnumerable<EmployeeDto>>.NotFound("المدرسة غير موجودة");

                var employees = await _unitOfWork.EmployeeRepository.GetBySchoolIdAsync(schoolId);
                var dtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.FullName = user?.FullName ?? string.Empty;
                }

                return ResponseDto<IEnumerable<EmployeeDto>>.Ok(dtos, "تم جلب الموظفين بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetBySchoolIdAsync للمدرسة {SchoolId}", schoolId);
                return ResponseDto<IEnumerable<EmployeeDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الموظفين حسب المسمى الوظيفي
        /// </summary>
        public async Task<ResponseDto<IEnumerable<EmployeeDto>>> GetByJobTitleAsync(string jobTitle)
        {
            try
            {
                var employees = await _unitOfWork.EmployeeRepository.GetByJobTitleAsync(jobTitle);
                var dtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.FullName = user?.FullName ?? string.Empty;
                }

                return ResponseDto<IEnumerable<EmployeeDto>>.Ok(dtos, "تم جلب الموظفين حسب المسمى الوظيفي");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetByJobTitleAsync للمسمى الوظيفي {JobTitle}", jobTitle);
                return ResponseDto<IEnumerable<EmployeeDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ القوائم المنسدلة ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على الموظفين للقوائم المنسدلة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<EmployeeLookupDto>>> GetLookupAsync(int? schoolId = null)
        {
            try
            {
                IEnumerable<Employee> employees;

                if (schoolId.HasValue)
                {
                    employees = await _unitOfWork.EmployeeRepository.GetBySchoolIdAsync(schoolId.Value);
                }
                else
                {
                    employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                }

                var dtos = _mapper.Map<IEnumerable<EmployeeLookupDto>>(employees);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.Id);
                    dto.FullName = user?.FullName ?? string.Empty;
                }

                return ResponseDto<IEnumerable<EmployeeLookupDto>>.Ok(dtos, "تم جلب الموظفين للقوائم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetLookupAsync");
                return ResponseDto<IEnumerable<EmployeeLookupDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ الإحصائيات ════════════════════════════════════

        /// <summary>
        /// 📊 الحصول على إحصائيات الموظف
        /// </summary>
        public async Task<ResponseDto<EmployeeStatisticsDto>> GetStatisticsAsync(int employeeId)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
                if (employee == null)
                    return ResponseDto<EmployeeStatisticsDto>.NotFound("الموظف غير موجود");

                var statistics = await GetEmployeeStatisticsAsync(employeeId);
                return ResponseDto<EmployeeStatisticsDto>.Ok(statistics, "تم جلب إحصائيات الموظف");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في GetStatisticsAsync للموظف {EmployeeId}", employeeId);
                return ResponseDto<EmployeeStatisticsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        private async Task<EmployeeStatisticsDto> GetEmployeeStatisticsAsync(int employeeId)
        {
            try
            {
                var attendances = await _unitOfWork.EmployeeAttendances
                    .FindAsync(ea => ea.EmployeeId == employeeId);

                var totalDays = attendances.Count();
                var presentDays = attendances.Count(a => a.Status == AttendanceStatus.Present);

                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);

                var statistics = new EmployeeStatisticsDto
                {
                    YearsOfExperience = employee != null ? DateTime.Now.Year - employee.HireDate.Year : 0,
                    PresentDays = presentDays,
                    AbsentDays = attendances.Count(a => a.Status == AttendanceStatus.Absent),
                    LateDays = attendances.Count(a => a.Status == AttendanceStatus.Late),
                    AttendancePercentage = totalDays > 0 ? (decimal)presentDays / totalDays * 100 : 0,
                    CompletedTasks = 0,
                    PendingTasks = 0,
                    PerformanceRating = 0
                };

                return statistics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في حساب إحصائيات الموظف {EmployeeId}", employeeId);
                return new EmployeeStatisticsDto();
            }
        }

        #endregion

        #region ════════════════════════════════════ العمليات الأساسية ════════════════════════════════════

        /// <summary>
        /// ➕ إنشاء موظف جديد
        /// </summary>
        public async Task<ResponseDto<EmployeeDto>> CreateAsync(CreateEmployeeDto createDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(createDto.UserId);
                if (user == null)
                    return ResponseDto<EmployeeDto>.Fail("المستخدم غير موجود");

                if (await _unitOfWork.EmployeeRepository.IsEmployeeCodeExistsAsync(createDto.EmployeeCode))
                    return ResponseDto<EmployeeDto>.Fail($"كود الموظف {createDto.EmployeeCode} موجود بالفعل");

                var employee = _mapper.Map<Employee>(createDto);
                employee.CreatedAt = DateTime.Now;
                employee.IsActive = true;

                var created = await _unitOfWork.EmployeeRepository.AddAsync(employee);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<EmployeeDto>(created);
                dto.FullName = user.FullName;

                _logger.LogInformation("تم إنشاء موظف جديد: {EmployeeCode}", created.EmployeeCode);
                return ResponseDto<EmployeeDto>.Ok(dto, "تم إنشاء الموظف بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في CreateAsync للموظف {EmployeeCode}", createDto.EmployeeCode);
                return ResponseDto<EmployeeDto>.Fail("حدث خطأ أثناء إنشاء الموظف", statusCode: 500);
            }
        }

        /// <summary>
        /// ✏️ تحديث بيانات موظف
        /// </summary>
        public async Task<ResponseDto<EmployeeDto>> UpdateAsync(int id, UpdateEmployeeDto updateDto)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
                if (employee == null)
                    return ResponseDto<EmployeeDto>.NotFound("الموظف غير موجود");

                _mapper.Map(updateDto, employee);
                employee.UpdatedAt = DateTime.Now;

                await _unitOfWork.EmployeeRepository.UpdateAsync(employee);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<EmployeeDto>(employee);

                var user = await _unitOfWork.Users.GetByIdAsync(employee.UserId);
                dto.FullName = user?.FullName ?? string.Empty;

                _logger.LogInformation("تم تحديث الموظف: {EmployeeCode}", employee.EmployeeCode);
                return ResponseDto<EmployeeDto>.Ok(dto, "تم تحديث الموظف بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في UpdateAsync للموظف {Id}", id);
                return ResponseDto<EmployeeDto>.Fail("حدث خطأ أثناء تحديث الموظف", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف موظف (Soft Delete)
        /// </summary>
        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
                if (employee == null)
                    return ResponseDto.NotFound("الموظف غير موجود");

                employee.IsDeleted = true;
                employee.IsActive = false;
                employee.DeletedAt = DateTime.Now;
                employee.UpdatedAt = DateTime.Now;

                await _unitOfWork.EmployeeRepository.UpdateAsync(employee);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف الموظف: {EmployeeCode}", employee.EmployeeCode);
                return ResponseDto.Ok("تم حذف الموظف بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في DeleteAsync للموظف {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف الموظف", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ التحقق ════════════════════════════════════

        public async Task<ResponseDto<bool>> IsEmployeeCodeExistsAsync(string employeeCode)
        {
            try
            {
                var exists = await _unitOfWork.EmployeeRepository.IsEmployeeCodeExistsAsync(employeeCode);
                return ResponseDto<bool>.Ok(exists, exists ? "كود الموظف موجود" : "كود الموظف غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في IsEmployeeCodeExistsAsync للموظف {EmployeeCode}", employeeCode);
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }

        #endregion
    }
}