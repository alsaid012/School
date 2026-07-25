using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 👤  مستودع المستخدمين (UserRepository)
    /// 📌  الوظيفة: تنفيذ عمليات قاعدة البيانات الخاصة بالمستخدمين
    /// 🔄  الوراثة: ترث من GenericRepository وتطبق IUserRepository
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        #region ════════════════════════════════════ البناء ════════════════════════════════════

        /// <summary>
        /// المُنشئ - يستقبل قاعدة البيانات ويمررها إلى القاعدة
        /// </summary>
        /// <param name="context">قاعدة البيانات (ApplicationDbContext)</param>
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        #endregion

        #region ════════════════════════════════════ البحث عن مستخدم ════════════════════════════════════

        /// <summary>
        /// 🔍 البحث عن مستخدم بواسطة اسم المستخدم
        /// </summary>
        /// <param name="username">اسم المستخدم</param>
        /// <returns>المستخدم أو null إذا لم يوجد</returns>
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        /// <summary>
        /// 🔍 البحث عن مستخدم بواسطة الرقم القومي
        /// </summary>
        /// <param name="nationalId">الرقم القومي</param>
        /// <returns>المستخدم أو null إذا لم يوجد</returns>
        public async Task<User?> GetUserByNationalIdAsync(string nationalId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.NationalId == nationalId);
        }

        /// <summary>
        /// 📋 الحصول على مستخدم مع جميع بياناته المرتبطة
        /// </summary>
        /// <remarks>
        /// يتم جلب البيانات التالية مع المستخدم:
        /// - School (المدرسة) ← Department (الإدارة) ← Governorate (المحافظة)
        /// - UserRoles (الأدوار)
        /// - Students (بيانات الطالب إن وجدت) ← ClassRoom ← GradeLevel
        /// - Teachers (بيانات المعلم إن وجدت)
        /// - Employees (بيانات الموظف إن وجدت)
        /// - Contacts (جهات الاتصال)
        /// </remarks>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>المستخدم مع البيانات المرتبطة أو null</returns>
        public async Task<User?> GetUserWithDetailsAsync(int userId)
        {
            return await _dbSet
                .Include(u => u.School)
                    //.ThenInclude(s => s != null ? s.Department : null!)
                    //    .ThenInclude(d => d != null ? d.Governorate : null!)
                .Include(u => u.UserRoles)
                .Include(u => u.Students)
                    //.ThenInclude(s => s != null ? s.ClassRoom : null!)
                    //    .ThenInclude(c => c != null ? c.GradeLevel : null!)
                .Include(u => u.Teachers)
                .Include(u => u.Employees)
                .Include(u => u.Contacts)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        #endregion

        // جلب المدارسه الخاصة بي المستخدم // يوجد دالة ايضا في UserService غير مستخدمة وهي تعمل 
        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet
                .Include(u => u.School)  // ✅ جلب المدرسة مع المستخدم
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }
        #region ════════════════════════════════════ جلب قوائم المستخدمين ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع المستخدمين التابعين لمدرسة معينة
        /// </summary>
        /// <param name="schoolId">معرف المدرسة</param>
        /// <returns>قائمة المستخدمين</returns>
        public async Task<IEnumerable<User>> GetUsersBySchoolAsync(int schoolId)
        {
            return await _dbSet
                .Where(u => u.SchoolId == schoolId)
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

        /// <summary>
        /// 📋 الحصول على المستخدمين حسب نوع المستخدم
        /// </summary>
        /// <param name="userType">نوع المستخدم (1:طالب, 2:معلم, 3:موظف, 4:مدير, 5:أدمن)</param>
        /// <returns>قائمة المستخدمين</returns>
        public async Task<IEnumerable<User>> GetUsersByTypeAsync(int userType)
        {
            return await _dbSet
                .Where(u => (int)u.UserType == userType)
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

        /// <summary>
        /// 📋 الحصول على المستخدمين حسب الحالة
        /// </summary>
        /// <param name="status">حالة المستخدم (0:معلق, 1:نشط, 2:موقوف, 3:غير نشط)</param>
        /// <returns>قائمة المستخدمين</returns>
        public async Task<IEnumerable<User>> GetUsersByStatusAsync(int status)
        {
            return await _dbSet
                .Where(u => (int)u.Status == status)
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

        /// <summary>
        /// ⏳ الحصول على جميع المستخدمين المعلقين (في انتظار التفعيل)
        /// </summary>
        /// <remarks>
        /// يتم ترتيبهم حسب تاريخ الإنشاء (الأقدم أولاً)
        /// </remarks>
        /// <returns>قائمة المستخدمين المعلقين</returns>
        public async Task<IEnumerable<User>> GetPendingUsersAsync()
        {
            return await _dbSet
                .Where(u => u.Status == UserStatus.Pending)
                .OrderBy(u => u.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// ✅ الحصول على جميع المستخدمين النشطين
        /// </summary>
        /// <returns>قائمة المستخدمين النشطين</returns>
        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _dbSet
                .Where(u => u.Status == UserStatus.Active && u.IsActive)
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

        /// <summary>
        /// 📋 الحصول على المستخدمين حسب دور معين
        /// </summary>
        /// <remarks>
        /// يتم البحث في جدول UserRoles للعثور على المستخدمين الذين لديهم دور معين
        /// </remarks>
        /// <param name="roleType">نوع الدور</param>
        /// <returns>قائمة المستخدمين</returns>
        public async Task<IEnumerable<User>> GetUsersByRoleAsync(int roleType)
        {
            return await _dbSet
                .Where(u => u.UserRoles.Any(r => (int)r.RoleType == roleType))
                .Include(u => u.UserRoles)
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

        #endregion

        #region ════════════════════════════════════ التحقق من الوجود ════════════════════════════════════

        /// <summary>
        /// ✅ التحقق من وجود اسم مستخدم مكرر
        /// </summary>
        /// <param name="username">اسم المستخدم</param>
        /// <returns>true إذا كان موجود، false إذا لم يوجد</returns>
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _dbSet
                .AnyAsync(u => u.Username == username);
        }

        /// <summary>
        /// ✅ التحقق من وجود رقم قومي مكرر
        /// </summary>
        /// <param name="nationalId">الرقم القومي</param>
        /// <returns>true إذا كان موجود، false إذا لم يوجد</returns>
        public async Task<bool> NationalIdExistsAsync(string nationalId)
        {
            return await _dbSet
                .AnyAsync(u => u.NationalId == nationalId);
        }

        #endregion
    }
}