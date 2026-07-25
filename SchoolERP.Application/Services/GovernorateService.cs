using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Departments;
using SchoolERP.Application.DTOs.Governorates;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📍  خدمة المحافظات (GovernorateService)
    /// 📌  الوظيفة: تنفيذ عمليات إدارة المحافظات
    /// 📦  الاستخدام: في GovernoratesController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class GovernorateService : IGovernorateService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GovernorateService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public GovernorateService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GovernorateService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ الحصول على المحافظات ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع المحافظات
        /// </summary>
        public async Task<ResponseDto<IEnumerable<GovernorateDto>>> GetAllAsync()
        {
            try
            {
                var governorates = await _unitOfWork.Governorates.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<GovernorateDto>>(governorates);

                foreach (var dto in dtos)
                {
                    var departments = await _unitOfWork.Departments
                        .FindAsync(d => d.GovernorateId == dto.Id);
                    dto.DepartmentsCount = departments.Count();
                }

                _logger.LogInformation("تم جلب {Count} محافظة", dtos.Count());
                return ResponseDto<IEnumerable<GovernorateDto>>.Ok(dtos, "تم جلب المحافظات بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جميع المحافظات");
                return ResponseDto<IEnumerable<GovernorateDto>>.Fail("حدث خطأ أثناء جلب المحافظات", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على جميع المحافظات مع الإدارات التابعة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<GovernorateDetailsDto>>> GetAllWithDepartmentsAsync()
        {
            try
            {
                var governorates = await _unitOfWork.Governorates.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<GovernorateDetailsDto>>(governorates);

                // جلب الإدارات لكل محافظة
                foreach (var dto in dtos)
                {
                    var departments = await _unitOfWork.Departments
                        .FindAsync(d => d.GovernorateId == dto.Id);
                    dto.Departments = _mapper.Map<List<DepartmentLookupDto>>(departments);
                    dto.DepartmentsCount = departments.Count();
                }

                _logger.LogInformation("تم جلب {Count} محافظة مع الإدارات", dtos.Count());
                return ResponseDto<IEnumerable<GovernorateDetailsDto>>.Ok(dtos, "تم جلب المحافظات مع الإدارات بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المحافظات مع الإدارات");
                return ResponseDto<IEnumerable<GovernorateDetailsDto>>.Fail("حدث خطأ أثناء جلب المحافظات", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على المحافظات للقوائم المنسدلة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<GovernorateLookupDto>>> GetLookupAsync()
        {
            try
            {
                var governorates = await _unitOfWork.Governorates.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<GovernorateLookupDto>>(governorates);

                return ResponseDto<IEnumerable<GovernorateLookupDto>>.Ok(dtos, "تم جلب المحافظات للقوائم المنسدلة");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المحافظات للقوائم المنسدلة");
                return ResponseDto<IEnumerable<GovernorateLookupDto>>.Fail("حدث خطأ أثناء جلب المحافظات", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ البحث عن محافظة ════════════════════════════════════

        /// <summary>
        /// 🔍 الحصول على محافظة بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<GovernorateDetailsDto>> GetByIdAsync(int id)
        {
            try
            {
                var governorate = await _unitOfWork.Governorates.GetByIdAsync(id);
                if (governorate == null)
                {
                    return ResponseDto<GovernorateDetailsDto>.NotFound($"المحافظة برقم {id} غير موجودة");
                }

                var dto = _mapper.Map<GovernorateDetailsDto>(governorate);

                // جلب الإدارات التابعة
                var departments = await _unitOfWork.Departments
                    .FindAsync(d => d.GovernorateId == id);
                dto.Departments = _mapper.Map<List<DepartmentLookupDto>>(departments);
                dto.DepartmentsCount = departments.Count();

                return ResponseDto<GovernorateDetailsDto>.Ok(dto, "تم جلب المحافظة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المحافظة {Id}", id);
                return ResponseDto<GovernorateDetailsDto>.Fail("حدث خطأ أثناء جلب المحافظة", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔍 الحصول على محافظة بواسطة الكود
        /// </summary>
        public async Task<ResponseDto<GovernorateDto>> GetByCodeAsync(string code)
        {
            try
            {
                var governorate = await _unitOfWork.Governorates.GetByCodeAsync(code);
                if (governorate == null)
                {
                    return ResponseDto<GovernorateDto>.NotFound($"المحافظة بالكود {code} غير موجودة");
                }

                var dto = _mapper.Map<GovernorateDto>(governorate);
                return ResponseDto<GovernorateDto>.Ok(dto, "تم جلب المحافظة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المحافظة بالكود {Code}", code);
                return ResponseDto<GovernorateDto>.Fail("حدث خطأ أثناء جلب المحافظة", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ إنشاء وتحديث وحذف ════════════════════════════════════

        /// <summary>
        /// ➕ إنشاء محافظة جديدة
        /// </summary>
        public async Task<ResponseDto<GovernorateDto>> CreateAsync(CreateGovernorateDto createDto)
        {
            try
            {
                // التحقق من وجود كود مكرر
                if (await _unitOfWork.Governorates.IsCodeExistsAsync(createDto.Code))
                {
                    return ResponseDto<GovernorateDto>.Fail($"الكود {createDto.Code} موجود بالفعل");
                }

                // التحقق من وجود اسم مكرر
                if (await _unitOfWork.Governorates.IsNameExistsAsync(createDto.Name))
                {
                    return ResponseDto<GovernorateDto>.Fail($"الاسم {createDto.Name} موجود بالفعل");
                }

                var governorate = _mapper.Map<Governorate>(createDto);
                governorate.CreatedAt = DateTime.Now;
                governorate.IsActive = true;

                var created = await _unitOfWork.Governorates.AddAsync(governorate);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<GovernorateDto>(created);
                _logger.LogInformation("تم إنشاء محافظة جديدة: {Name}", created.Name);

                return ResponseDto<GovernorateDto>.Ok(dto, "تم إنشاء المحافظة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء محافظة جديدة");
                return ResponseDto<GovernorateDto>.Fail("حدث خطأ أثناء إنشاء المحافظة", statusCode: 500);
            }
        }

        /// <summary>
        /// ✏️ تحديث بيانات محافظة
        /// </summary>
        public async Task<ResponseDto<GovernorateDto>> UpdateAsync(int id, UpdateGovernorateDto updateDto)
        {
            try
            {
                var governorate = await _unitOfWork.Governorates.GetByIdAsync(id);
                if (governorate == null)
                {
                    return ResponseDto<GovernorateDto>.NotFound($"المحافظة برقم {id} غير موجودة");
                }

                // التحقق من وجود اسم مكرر (باستثناء نفس المحافظة)
                if (!string.IsNullOrEmpty(updateDto.Name) &&
                    await _unitOfWork.Governorates.IsNameExistsAsync(updateDto.Name, id))
                {
                    return ResponseDto<GovernorateDto>.Fail($"الاسم {updateDto.Name} موجود بالفعل");
                }

                // التحقق من وجود كود مكرر (باستثناء نفس المحافظة)
                if (!string.IsNullOrEmpty(updateDto.Code) &&
                    await _unitOfWork.Governorates.IsCodeExistsAsync(updateDto.Code, id))
                {
                    return ResponseDto<GovernorateDto>.Fail($"الكود {updateDto.Code} موجود بالفعل");
                }

                _mapper.Map(updateDto, governorate);
                governorate.UpdatedAt = DateTime.Now;

                await _unitOfWork.Governorates.UpdateAsync(governorate);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<GovernorateDto>(governorate);
                _logger.LogInformation("تم تحديث المحافظة: {Name}", governorate.Name);

                return ResponseDto<GovernorateDto>.Ok(dto, "تم تحديث المحافظة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تحديث المحافظة {Id}", id);
                return ResponseDto<GovernorateDto>.Fail("حدث خطأ أثناء تحديث المحافظة", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف محافظة (Soft Delete)
        /// </summary>
        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var governorate = await _unitOfWork.Governorates.GetByIdAsync(id);
                if (governorate == null)
                {
                    return ResponseDto.NotFound($"المحافظة برقم {id} غير موجودة");
                }

                // التحقق من وجود إدارات تابعة
                var departments = await _unitOfWork.Departments
                    .FindAsync(d => d.GovernorateId == id);
                if (departments.Any())
                {
                    return ResponseDto.Fail("لا يمكن حذف المحافظة لأنها تحتوي على إدارات تابعة");
                }

                // Soft Delete
                governorate.IsDeleted = true;
                governorate.IsActive = false;
                governorate.DeletedAt = DateTime.Now;
                governorate.UpdatedAt = DateTime.Now;

                await _unitOfWork.Governorates.UpdateAsync(governorate);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف المحافظة: {Name}", governorate.Name);
                return ResponseDto.Ok("تم حذف المحافظة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء حذف المحافظة {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف المحافظة", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ التحقق من الوجود ════════════════════════════════════

        /// <summary>
        /// ✅ التحقق من وجود محافظة بنفس الاسم
        /// </summary>
        public async Task<ResponseDto<bool>> IsNameExistsAsync(string name, int? excludeId = null)
        {
            try
            {
                var exists = await _unitOfWork.Governorates.IsNameExistsAsync(name, excludeId);
                return ResponseDto<bool>.Ok(exists, exists ? "الاسم موجود" : "الاسم غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء التحقق من وجود الاسم {Name}", name);
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }

        /// <summary>
        /// ✅ التحقق من وجود محافظة بنفس الكود
        /// </summary>
        public async Task<ResponseDto<bool>> IsCodeExistsAsync(string code, int? excludeId = null)
        {
            try
            {
                var exists = await _unitOfWork.Governorates.IsCodeExistsAsync(code, excludeId);
                return ResponseDto<bool>.Ok(exists, exists ? "الكود موجود" : "الكود غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء التحقق من وجود الكود {Code}", code);
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }

        #endregion
    }
}