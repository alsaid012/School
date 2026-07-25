using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// 📞  واجهة مستودع جهات الاتصال (IUserContactRepository)
    /// </summary>
    public interface IUserContactRepository : IGenericRepository<UserContact>
    {
        /// <summary>
        /// 📋 الحصول على جميع جهات الاتصال لمستخدم معين
        /// </summary>
        Task<IEnumerable<UserContact>> GetByUserIdAsync(int userId);

        /// <summary>
        /// 📋 الحصول على جهات الاتصال حسب النوع
        /// </summary>
        Task<IEnumerable<UserContact>> GetByTypeAsync(ContactType type);

        /// <summary>
        /// 📋 الحصول على جهة الاتصال الأساسية لمستخدم
        /// </summary>
        Task<UserContact?> GetPrimaryContactAsync(int userId);

        /// <summary>
        /// ✅ التحقق من وجود جهة اتصال بنفس القيمة
        /// </summary>
        Task<bool> IsValueExistsAsync(string value, int? excludeId = null);
    }
}