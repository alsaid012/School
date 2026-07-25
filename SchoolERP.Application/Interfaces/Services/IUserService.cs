using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Users;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 👤  واجهة خدمة المستخدمين (IUserService)
    /// 📌  الوظيفة: تعريف عمليات إدارة المستخدمين
    /// 📦  الاستخدام: في UsersController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IUserService
    {

        #region -------------------------     // 📋 جلب البيانات----------------------------------
        /// <summary>
        /// 📋 الحصول على جميع المستخدمين
        /// </summary>
        Task<ResponseDto<IEnumerable<UserDto>>> GetAllAsync();

        /// <summary>
        /// 🔍 الحصول على مستخدم بواسطة المعرف
        /// </summary>
        Task<ResponseDto<UserDetailsDto>> GetByIdAsync(int id);
        /// <summary>
        /// 🔍 الحصول على مستخدم بواسطة اسم المستخدم
        /// </summary>
        Task<ResponseDto<UserDto>> GetByUsernameAsync(string username);
        /// <summary>
        /// 🔍 الحصول على مستخدم بواسطة الرقم القومي
        /// </summary>
        Task<ResponseDto<UserDto>> GetByNationalIdAsync(string nationalId);

        #endregion


        #region --------------------------  // 🔍 البحث والفلترة -------------------------------------------

        /// <summary>
        /// 📋 الحصول على المستخدمين التابعين لمدرسة معينة
        /// </summary>
        Task<ResponseDto<IEnumerable<UserDto>>> GetBySchoolIdAsync(int schoolId);

        /// <summary>
        /// 📋 الحصول على المستخدمين حسب النوع
        /// </summary>
        Task<ResponseDto<IEnumerable<UserDto>>> GetByUserTypeAsync(int userType);

        /// <summary>
        /// 📋 الحصول على المستخدمين حسب الحالة
        /// </summary>
        Task<ResponseDto<IEnumerable<UserDto>>> GetByStatusAsync(int status);

        /// <summary>
        /// 📋 الحصول على المستخدمين المعلقين
        /// </summary>
        Task<ResponseDto<IEnumerable<UserDto>>> GetPendingUsersAsync();

        /// <summary>
        /// 📋 الحصول على المستخدمين النشطين
        /// </summary>
        Task<ResponseDto<IEnumerable<UserDto>>> GetActiveUsersAsync();


        /// <summary>
        /// 📋 الحصول على المستخدمين حسب الدور
        /// </summary>
        Task<ResponseDto<IEnumerable<UserDto>>> GetByRoleAsync(int roleType);

        #endregion

        #region القوائم


        /// <summary>
        /// 📋 الحصول على المستخدمين للقوائم المنسدلة
        /// </summary>
        Task<ResponseDto<IEnumerable<UserLookupDto>>> GetLookupAsync(int? userType = null);

        /// <summary>
        /// 📊 الحصول على إحصائيات المستخدم
        /// </summary>
        Task<ResponseDto<UserStatisticsDto>> GetStatisticsAsync(int userId);
        #endregion


        #region ---------------------   // ➕✏️🗑️ العمليات الأساسية ------------------------------------------


        /// <summary>
        /// ➕ إنشاء مستخدم جديد
        /// </summary>
        Task<ResponseDto<UserDto>> CreateAsync(CreateUserDto createDto);

        /// <summary>
        /// ✏️ تحديث بيانات مستخدم
        /// </summary>
        Task<ResponseDto<UserDto>> UpdateAsync(int id, UpdateUserDto updateDto);

        /// <summary>
        /// 🗑️ حذف مستخدم (Soft Delete)
        /// </summary>
        Task<ResponseDto> DeleteAsync(int id);

        /// <summary>
        /// 🗑️ حذف نهائي للمستخدم
        /// </summary>
        Task<ResponseDto> HardDeleteAsync(int id);


        #endregion

        #region -------------------------------------        // 🔄 تغيير الحالة --------------------------


        /// <summary>
        /// 🔄 تفعيل المستخدم
        /// </summary>
        Task<ResponseDto> ActivateAsync(int id);

        /// <summary>
        /// ⏸️ تعليق المستخدم
        /// </summary>
        Task<ResponseDto> SuspendAsync(int id);

        /// <summary>
        /// 🔄 استعادة مستخدم محذوف
        /// </summary>
        Task<ResponseDto> RestoreAsync(int id);

        #endregion



        #region -------------------------  // ✅ التحقق ----------------------------

        /// <summary>
        /// ✅ التحقق من وجود اسم مستخدم
        /// </summary>
        Task<ResponseDto<bool>> IsUsernameExistsAsync(string username);

        /// <summary>
        /// ✅ التحقق من وجود رقم قومي
        /// </summary>
        Task<ResponseDto<bool>> IsNationalIdExistsAsync(string nationalId);
        #endregion

    }
}