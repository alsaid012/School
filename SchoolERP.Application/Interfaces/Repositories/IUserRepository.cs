using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 👤  واجهة مستودع المستخدمين (IUserRepository)
    /// 📌  الوظيفة: تعريف العمليات الخاصة بالمستخدمين
    /// 🔄  الوراثة: ترث من IGenericRepository
    /// 📦  الاستخدام: تستخدم في طبقة الخدمات (Services)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IUserRepository : IGenericRepository<User>
    {
        #region ════════════════════════════════════ البحث عن مستخدم ════════════════════════════════════

        /// <summary>
        /// 🔍 البحث عن مستخدم بواسطة اسم المستخدم
        /// </summary>
        /// <param name="username">اسم المستخدم</param>
        /// <returns>المستخدم أو null إذا لم يوجد</returns>
        Task<User?> GetUserByUsernameAsync(string username);

        /// <summary>
        /// 🔍 البحث عن مستخدم بواسطة الرقم القومي
        /// </summary>
        /// <param name="nationalId">الرقم القومي</param>
        /// <returns>المستخدم أو null إذا لم يوجد</returns>
        Task<User?> GetUserByNationalIdAsync(string nationalId);

        /// <summary>
        /// 📋 الحصول على مستخدم مع جميع بياناته المرتبطة
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>المستخدم مع البيانات المرتبطة (School, Roles, Contacts, ...)</returns>
        Task<User?> GetUserWithDetailsAsync(int userId);

        #endregion

        #region ════════════════════════════════════ جلب قوائم المستخدمين ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع المستخدمين التابعين لمدرسة معينة
        /// </summary>
        /// <param name="schoolId">معرف المدرسة</param>
        /// <returns>قائمة المستخدمين</returns>
        Task<IEnumerable<User>> GetUsersBySchoolAsync(int schoolId);

        /// <summary>
        /// 📋 الحصول على المستخدمين حسب نوع المستخدم (طالب/معلم/موظف)
        /// </summary>
        /// <param name="userType">نوع المستخدم</param>
        /// <returns>قائمة المستخدمين</returns>
        Task<IEnumerable<User>> GetUsersByTypeAsync(int userType);

        /// <summary>
        /// 📋 الحصول على المستخدمين حسب الحالة (نشط/موقوف/معلق)
        /// </summary>
        /// <param name="status">حالة المستخدم</param>
        /// <returns>قائمة المستخدمين</returns>
        Task<IEnumerable<User>> GetUsersByStatusAsync(int status);

        /// <summary>
        /// ⏳ الحصول على جميع المستخدمين المعلقين (في انتظار التفعيل)
        /// </summary>
        /// <returns>قائمة المستخدمين المعلقين</returns>
        Task<IEnumerable<User>> GetPendingUsersAsync();

        /// <summary>
        /// ✅ الحصول على جميع المستخدمين النشطين
        /// </summary>
        /// <returns>قائمة المستخدمين النشطين</returns>
        Task<IEnumerable<User>> GetActiveUsersAsync();

        /// <summary>
        /// 📋 الحصول على المستخدمين حسب دور معين
        /// </summary>
        /// <param name="roleType">نوع الدور</param>
        /// <returns>قائمة المستخدمين</returns>
        Task<IEnumerable<User>> GetUsersByRoleAsync(int roleType);

        #endregion

        #region ════════════════════════════════════ التحقق من الوجود ════════════════════════════════════

        /// <summary>
        /// ✅ التحقق من وجود اسم مستخدم مكرر
        /// </summary>
        /// <param name="username">اسم المستخدم</param>
        /// <returns>true إذا كان موجود، false إذا لم يوجد</returns>
        Task<bool> UsernameExistsAsync(string username);

        /// <summary>
        /// ✅ التحقق من وجود رقم قومي مكرر
        /// </summary>
        /// <param name="nationalId">الرقم القومي</param>
        /// <returns>true إذا كان موجود، false إذا لم يوجد</returns>
        Task<bool> NationalIdExistsAsync(string nationalId);

        #endregion
    }
}