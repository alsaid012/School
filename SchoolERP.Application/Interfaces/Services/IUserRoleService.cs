using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.UserRoles;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🎭  واجهة خدمة أدوار المستخدمين (IUserRoleService)
    /// 📌  الوظيفة: تعريف عمليات إدارة أدوار المستخدمين
    /// 📦  الاستخدام: في UserRolesController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IUserRoleService
    {
        /// <summary>
        /// 📋 الحصول على جميع أدوار المستخدمين
        /// </summary>
        Task<ResponseDto<IEnumerable<UserRoleDto>>> GetAllAsync();

        /// <summary>
        /// 📋 الحصول على أدوار مستخدم معين
        /// </summary>
        Task<ResponseDto<IEnumerable<UserRoleDto>>> GetByUserIdAsync(int userId);

        /// <summary>
        /// 📋 الحصول على الدور الأساسي لمستخدم
        /// </summary>
        Task<ResponseDto<UserRoleDto>> GetPrimaryRoleAsync(int userId);

        /// <summary>
        /// 📋 الحصول على المستخدمين الذين لديهم دور معين
        /// </summary>
        Task<ResponseDto<IEnumerable<UserRoleDto>>> GetByRoleTypeAsync(int roleType);

        /// <summary>
        /// 📋 الحصول على أدوار المستخدمين للقوائم المنسدلة
        /// </summary>
        Task<ResponseDto<IEnumerable<UserRoleLookupDto>>> GetLookupAsync(int? userId = null);

        /// <summary>
        /// 🔍 الحصول على دور مستخدم بواسطة المعرف
        /// </summary>
        Task<ResponseDto<UserRoleDto>> GetByIdAsync(int id);

        /// <summary>
        /// 📊 الحصول على إحصائيات أدوار المستخدمين
        /// </summary>
        Task<ResponseDto<UserRoleStatisticsDto>> GetStatisticsAsync();

        /// <summary>
        /// ➕ إنشاء دور مستخدم جديد
        /// </summary>
        Task<ResponseDto<UserRoleDto>> CreateAsync(CreateUserRoleDto createDto);

        /// <summary>
        /// ✏️ تحديث بيانات دور مستخدم
        /// </summary>
        Task<ResponseDto<UserRoleDto>> UpdateAsync(int id, UpdateUserRoleDto updateDto);

        /// <summary>
        /// 🗑️ حذف دور مستخدم
        /// </summary>
        Task<ResponseDto> DeleteAsync(int id);

        /// <summary>
        /// 🔄 تعيين دور كأساسي
        /// </summary>
        Task<ResponseDto> SetPrimaryAsync(int id, int userId);

        /// <summary>
        /// ✅ التحقق من وجود دور مكرر لنفس المستخدم
        /// </summary>
        Task<ResponseDto<bool>> IsExistsAsync(int userId, int roleType);
    }
}