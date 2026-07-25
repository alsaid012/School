using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Governorates;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📍  واجهة خدمة المحافظات (IGovernorateService)
    /// 📌  الوظيفة: تعريف عمليات إدارة المحافظات
    /// 📦  الاستخدام: في GovernoratesController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IGovernorateService
    {
        /// <summary>
        /// 📋 الحصول على جميع المحافظات
        /// </summary>
        Task<ResponseDto<IEnumerable<GovernorateDto>>> GetAllAsync();
        /// <summary>
        /// 🔍 الحصول على محافظة بواسطة المعرف
        /// </summary>
        Task<ResponseDto<GovernorateDetailsDto>> GetByIdAsync(int id);

        /// <summary>
        /// 🔍 الحصول على محافظة بواسطة الكود
        /// </summary>
        Task<ResponseDto<GovernorateDto>> GetByCodeAsync(string code);

        /// <summary>
        /// 📋 الحصول على جميع المحافظات مع الإدارات التابعة
        /// </summary>
        Task<ResponseDto<IEnumerable<GovernorateDetailsDto>>> GetAllWithDepartmentsAsync();

        /// <summary>
        /// 📋 الحصول على المحافظات للقوائم المنسدلة
        /// </summary>
        Task<ResponseDto<IEnumerable<GovernorateLookupDto>>> GetLookupAsync();

   

        /// <summary>
        /// ➕ إنشاء محافظة جديدة
        /// </summary>
        Task<ResponseDto<GovernorateDto>> CreateAsync(CreateGovernorateDto createDto);

        /// <summary>
        /// ✏️ تحديث بيانات محافظة
        /// </summary>
        Task<ResponseDto<GovernorateDto>> UpdateAsync(int id, UpdateGovernorateDto updateDto);

        /// <summary>
        /// 🗑️ حذف محافظة (Soft Delete)
        /// </summary>
        Task<ResponseDto> DeleteAsync(int id);

        /// <summary>
        /// ✅ التحقق من وجود محافظة بنفس الاسم
        /// </summary>
        Task<ResponseDto<bool>> IsNameExistsAsync(string name, int? excludeId = null);

        /// <summary>
        /// ✅ التحقق من وجود محافظة بنفس الكود
        /// </summary>
        Task<ResponseDto<bool>> IsCodeExistsAsync(string code, int? excludeId = null);
    }
}