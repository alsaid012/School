using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.UserContacts;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📞  واجهة خدمة جهات الاتصال (IUserContactService)
    /// 📌  الوظيفة: تعريف عمليات إدارة جهات الاتصال
    /// 📦  الاستخدام: في UserContactsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IUserContactService
    {
        /// <summary>
        /// 📋 الحصول على جميع جهات الاتصال
        /// </summary>
        Task<ResponseDto<IEnumerable<UserContactDto>>> GetAllAsync();

        /// <summary>
        /// 📋 الحصول على جهات اتصال مستخدم معين
        /// </summary>
        Task<ResponseDto<IEnumerable<UserContactDto>>> GetByUserIdAsync(int userId);

        /// <summary>
        /// 📋 الحصول على جهات الاتصال حسب النوع
        /// </summary>
        Task<ResponseDto<IEnumerable<UserContactDto>>> GetByTypeAsync(int contactType);

        /// <summary>
        /// 📋 الحصول على جهة الاتصال الأساسية لمستخدم
        /// </summary>
        Task<ResponseDto<UserContactDto>> GetPrimaryContactAsync(int userId);

        /// <summary>
        /// 📋 الحصول على جهات الاتصال للقوائم المنسدلة
        /// </summary>
        Task<ResponseDto<IEnumerable<UserContactLookupDto>>> GetLookupAsync(int? userId = null);

        /// <summary>
        /// 🔍 الحصول على جهة اتصال بواسطة المعرف
        /// </summary>
        Task<ResponseDto<UserContactDto>> GetByIdAsync(int id);

        /// <summary>
        /// 📊 الحصول على إحصائيات جهات الاتصال
        /// </summary>
        Task<ResponseDto<UserContactStatisticsDto>> GetStatisticsAsync();

        /// <summary>
        /// ➕ إنشاء جهة اتصال جديدة
        /// </summary>
        Task<ResponseDto<UserContactDto>> CreateAsync(CreateUserContactDto createDto);

        /// <summary>
        /// ✏️ تحديث بيانات جهة اتصال
        /// </summary>
        Task<ResponseDto<UserContactDto>> UpdateAsync(int id, UpdateUserContactDto updateDto);

        /// <summary>
        /// 🗑️ حذف جهة اتصال
        /// </summary>
        Task<ResponseDto> DeleteAsync(int id);

        /// <summary>
        /// 🔄 تعيين جهة اتصال كأساسية
        /// </summary>
        Task<ResponseDto> SetPrimaryAsync(int id, int userId);

        /// <summary>
        /// ✅ التحقق من وجود جهة اتصال بنفس القيمة
        /// </summary>
        Task<ResponseDto<bool>> IsValueExistsAsync(string value, int? excludeId = null);
    }
}