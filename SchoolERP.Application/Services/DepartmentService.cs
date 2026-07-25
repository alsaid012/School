using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Departments;
using SchoolERP.Application.DTOs.Schools;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏢  خدمة الإدارات التعليمية (DepartmentService)
    /// 📌  الوظيفة: تنفيذ عمليات إدارة الإدارات التعليمية
    /// 📦  الاستخدام: في DepartmentsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class DepartmentService : IDepartmentService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public DepartmentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<DepartmentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ الحصول على الإدارات ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع الإدارات
        /// </summary>
        public async Task<ResponseDto<IEnumerable<DepartmentDto>>> GetAllAsync()
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
                foreach (var dto in dtos)
                {
                    // ✅ جلب عدد المدارس
                    var schools = await _unitOfWork.SchoolRepository
                        .FindAsync(s => s.DepartmentId == dto.Id);
                    dto.SchoolsCount = schools.Count();

                    // ✅ جلب اسم المحافظة
                    var governorate = await _unitOfWork.Governorates.GetByIdAsync(dto.GovernorateId);
                    dto.GovernorateName = governorate?.Name;
                }

                _logger.LogInformation("تم جلب {Count} إدارة", dtos.Count());
                return ResponseDto<IEnumerable<DepartmentDto>>.Ok(dtos, "تم جلب الإدارات بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جميع الإدارات");
                return ResponseDto<IEnumerable<DepartmentDto>>.Fail("حدث خطأ أثناء جلب الإدارات", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على جميع الإدارات مع التفاصيل
        /// </summary>
        public async Task<ResponseDto<IEnumerable<DepartmentDetailsDto>>> GetAllWithDetailsAsync()
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<DepartmentDetailsDto>>(departments);

                foreach (var dto in dtos)
                {
                    var schools = await _unitOfWork.SchoolRepository
                        .FindAsync(s => s.DepartmentId == dto.Id);
                    dto.Schools = _mapper.Map<List<SchoolLookupDto>>(schools);
                }

                return ResponseDto<IEnumerable<DepartmentDetailsDto>>.Ok(dtos, "تم جلب الإدارات مع التفاصيل");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الإدارات مع التفاصيل");
                return ResponseDto<IEnumerable<DepartmentDetailsDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الإدارات التابعة لمحافظة معينة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<DepartmentDto>>> GetByGovernorateIdAsync(int governorateId)
        {
            try
            {
                var governorate = await _unitOfWork.Governorates.GetByIdAsync(governorateId);
                if (governorate == null)
                {
                    return ResponseDto<IEnumerable<DepartmentDto>>.NotFound("المحافظة غير موجودة");
                }

                var departments = await _unitOfWork.Departments
                    .FindAsync(d => d.GovernorateId == governorateId);
                var dtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);

                return ResponseDto<IEnumerable<DepartmentDto>>.Ok(dtos, "تم جلب الإدارات بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الإدارات للمحافظة {GovernorateId}", governorateId);
                return ResponseDto<IEnumerable<DepartmentDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الإدارات للقوائم المنسدلة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<DepartmentLookupDto>>> GetLookupAsync(int? governorateId = null)
        {
            try
            {
                IEnumerable<Department> departments;

                if (governorateId.HasValue)
                {
                    departments = await _unitOfWork.Departments
                        .FindAsync(d => d.GovernorateId == governorateId.Value);
                }
                else
                {
                    departments = await _unitOfWork.Departments.GetAllAsync();
                }

                var dtos = _mapper.Map<IEnumerable<DepartmentLookupDto>>(departments);
                return ResponseDto<IEnumerable<DepartmentLookupDto>>.Ok(dtos, "تم جلب الإدارات للقوائم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الإدارات للقوائم");
                return ResponseDto<IEnumerable<DepartmentLookupDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ البحث عن إدارة ════════════════════════════════════

        /// <summary>
        /// 🔍 الحصول على إدارة بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<DepartmentDetailsDto>> GetByIdAsync(int id)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(id);
                if (department == null)
                {
                    return ResponseDto<DepartmentDetailsDto>.NotFound("الإدارة غير موجودة");
                }

                var dto = _mapper.Map<DepartmentDetailsDto>(department);

                // ✅ جلب المدارس التابعة
                var schools = await _unitOfWork.SchoolRepository
                    .FindAsync(s => s.DepartmentId == id);

                //dto.Schools = _mapper.Map<List<SchoolLookupDto>>(schools);


                // ✅ تحويل المدارس إلى SchoolLookupDto

                dto.Schools = schools.Select(s => new SchoolLookupDto
                {
                    Id = s.Id,
                    Name = s.SchoolName,
                    Code = s.SchoolCode,
                    SchoolType = s.SchoolType.ToString(),
                    PrincipalName = s.PrincipalName,
                    IsActive = s.IsActive
                }).ToList();
                dto.SchoolsCount = dto.Schools.Count;

                // ✅ جلب اسم المحافظة
                var governorate = await _unitOfWork.Governorates.GetByIdAsync(department.GovernorateId);
                dto.GovernorateName = governorate?.Name;

                // إحصائيات
                dto.Statistics = new DepartmentStatisticsDto
                {
                    TotalSchools = schools.Count(),
                    TotalStudents = 0, // سيتم حسابه لاحقاً
                    TotalTeachers = 0,
                    TotalEmployees = 0
                };

                return ResponseDto<DepartmentDetailsDto>.Ok(dto, "تم جلب الإدارة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الإدارة {Id}", id);
                return ResponseDto<DepartmentDetailsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على إدارة بواسطة الكود
        /// </summary>
        public async Task<ResponseDto<DepartmentDto>> GetByCodeAsync(string code)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByCodeAsync(code);
                if (department == null)
                {
                    return ResponseDto<DepartmentDto>.NotFound("الإدارة غير موجودة");
                }

                var dto = _mapper.Map<DepartmentDto>(department);
                return ResponseDto<DepartmentDto>.Ok(dto, "تم جلب الإدارة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الإدارة بالكود {Code}", code);
                return ResponseDto<DepartmentDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ إنشاء وتحديث وحذف ════════════════════════════════════

        /// <summary>
        /// ➕ إنشاء إدارة جديدة
        /// </summary>
        public async Task<ResponseDto<DepartmentDto>> CreateAsync(CreateDepartmentDto createDto)
        {
            try
            {
                // التحقق من وجود المحافظة
                var governorate = await _unitOfWork.Governorates.GetByIdAsync(createDto.GovernorateId);
                if (governorate == null)
                {
                    return ResponseDto<DepartmentDto>.Fail("المحافظة غير موجودة");
                }

                // التحقق من وجود كود مكرر
                if (await _unitOfWork.Departments.IsCodeExistsAsync(createDto.Code))
                {
                    return ResponseDto<DepartmentDto>.Fail($"الكود {createDto.Code} موجود بالفعل");
                }

                // التحقق من وجود اسم مكرر
                if (await _unitOfWork.Departments.IsNameExistsAsync(createDto.Name))
                {
                    return ResponseDto<DepartmentDto>.Fail($"الاسم {createDto.Name} موجود بالفعل");
                }

                var department = _mapper.Map<Department>(createDto);
                department.CreatedAt = DateTime.Now;
                department.IsActive = true;

                var created = await _unitOfWork.Departments.AddAsync(department);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<DepartmentDto>(created);
                _logger.LogInformation("تم إنشاء إدارة جديدة: {Name}", created.Name);

                return ResponseDto<DepartmentDto>.Ok(dto, "تم إنشاء الإدارة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء إدارة جديدة");
                return ResponseDto<DepartmentDto>.Fail("حدث خطأ أثناء إنشاء الإدارة", statusCode: 500);
            }
        }

        /// <summary>
        /// ✏️ تحديث بيانات إدارة
        /// </summary>
        public async Task<ResponseDto<DepartmentDto>> UpdateAsync(int id, UpdateDepartmentDto updateDto)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(id);
                if (department == null)
                {
                    return ResponseDto<DepartmentDto>.NotFound("الإدارة غير موجودة");
                }

                // التحقق من وجود اسم مكرر
                if (!string.IsNullOrEmpty(updateDto.Name) &&
                    await _unitOfWork.Departments.IsNameExistsAsync(updateDto.Name, id))
                {
                    return ResponseDto<DepartmentDto>.Fail($"الاسم {updateDto.Name} موجود بالفعل");
                }

                // التحقق من وجود المحافظة
                if (updateDto.GovernorateId.HasValue)
                {
                    var governorate = await _unitOfWork.Governorates.GetByIdAsync(updateDto.GovernorateId.Value);
                    if (governorate == null)
                    {
                        return ResponseDto<DepartmentDto>.Fail("المحافظة غير موجودة");
                    }
                }

                _mapper.Map(updateDto, department);
                department.UpdatedAt = DateTime.Now;

                await _unitOfWork.Departments.UpdateAsync(department);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<DepartmentDto>(department);
                _logger.LogInformation("تم تحديث الإدارة: {Name}", department.Name);

                return ResponseDto<DepartmentDto>.Ok(dto, "تم تحديث الإدارة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تحديث الإدارة {Id}", id);
                return ResponseDto<DepartmentDto>.Fail("حدث خطأ أثناء تحديث الإدارة", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف إدارة (Soft Delete)
        /// </summary>
        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(id);
                if (department == null)
                {
                    return ResponseDto.NotFound("الإدارة غير موجودة");
                }

                // التحقق من وجود مدارس تابعة
                var schools = await _unitOfWork.SchoolRepository
                    .FindAsync(s => s.DepartmentId == id);
                if (schools.Any())
                {
                    return ResponseDto.Fail("لا يمكن حذف الإدارة لأنها تحتوي على مدارس تابعة");
                }

                department.IsDeleted = true;
                department.IsActive = false;
                department.DeletedAt = DateTime.Now;
                department.UpdatedAt = DateTime.Now;

                await _unitOfWork.Departments.UpdateAsync(department);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف الإدارة: {Name}", department.Name);
                return ResponseDto.Ok("تم حذف الإدارة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء حذف الإدارة {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف الإدارة", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ التحقق من الوجود ════════════════════════════════════

        /// <summary>
        /// ✅ التحقق من وجود إدارة بنفس الاسم
        /// </summary>
        public async Task<ResponseDto<bool>> IsNameExistsAsync(string name, int? excludeId = null)
        {
            try
            {
                var exists = await _unitOfWork.Departments.IsNameExistsAsync(name, excludeId);
                return ResponseDto<bool>.Ok(exists, exists ? "الاسم موجود" : "الاسم غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء التحقق من الاسم {Name}", name);
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }
        /// <summary>
        /// ✅ التحقق من وجود إدارة بنفس الكود
        /// </summary>
        public async Task<ResponseDto<bool>> IsCodeExistsAsync(string code, int? excludeId = null)
        {
            try
            {
                var exists = await _unitOfWork.Departments.IsCodeExistsAsync(code, excludeId);
                return ResponseDto<bool>.Ok(exists, exists ? "الكود موجود" : "الكود غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في IsCodeExistsAsync للإدارة {Code}", code);
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }

        #endregion
    }
}