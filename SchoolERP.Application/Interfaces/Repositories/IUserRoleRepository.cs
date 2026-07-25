using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// 🎭  واجهة مستودع أدوار المستخدمين (IUserRoleRepository)
    /// </summary>
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        /// <summary>
        /// 📋 الحصول على جميع أدوار مستخدم معين
        /// </summary>
        Task<IEnumerable<UserRole>> GetByUserIdAsync(int userId);

        /// <summary>
        /// 📋 الحصول على الدور الأساسي لمستخدم
        /// </summary>
        Task<UserRole?> GetPrimaryRoleAsync(int userId);

        /// <summary>
        /// 📋 الحصول على جميع المستخدمين الذين لديهم دور معين
        /// </summary>
        Task<IEnumerable<UserRole>> GetByRoleTypeAsync(UserType roleType);

        /// <summary>
        /// ✅ التحقق من وجود دور مكرر لنفس المستخدم
        /// </summary>
        Task<bool> IsExistsAsync(int userId, UserType roleType);
    }
}