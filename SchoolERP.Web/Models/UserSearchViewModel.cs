using SchoolERP.Application.DTOs.Users;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Web.Models
{
    /// <summary>
    /// 📋  نموذج البحث عن المستخدمين
    /// </summary>
    public class UserSearchViewModel
    {
        // ============================================================
        // خصائص البحث
        // ============================================================

        /// <summary>
        /// نص البحث (الاسم، البريد، اسم المستخدم)
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// نوع المستخدم (فلتر)
        /// </summary>
        public UserType? UserType { get; set; }

        /// <summary>
        /// حالة المستخدم (فلتر)
        /// </summary>
        public UserStatus? Status { get; set; }

        /// <summary>
        /// معرف المدرسة (فلتر)
        /// </summary>
        public int? SchoolId { get; set; }

        // ============================================================
        // النتائج
        // ============================================================

        /// <summary>
        /// قائمة المستخدمين بعد الفلترة
        /// </summary>
        public List<UserDto> Users { get; set; } = new();

        /// <summary>
        /// قائمة المدارس للقائمة المنسدلة
        /// </summary>
        public List<School>? Schools { get; set; }

        // ============================================================
        // الإحصائيات
        // ============================================================

        /// <summary>
        /// إجمالي عدد المستخدمين
        /// </summary>
        public int TotalCount => Users?.Count ?? 0;

        /// <summary>
        /// عدد المستخدمين النشطين
        /// </summary>
        public int ActiveCount => Users?.Count(u => u.Status == UserStatus.Active) ?? 0;

        /// <summary>
        /// عدد المستخدمين المعلقين
        /// </summary>
        public int PendingCount => Users?.Count(u => u.Status == UserStatus.Pending) ?? 0;

        /// <summary>
        /// عدد المستخدمين الموقوفين
        /// </summary>
        public int SuspendedCount => Users?.Count(u => u.Status == UserStatus.Suspended) ?? 0;

        // ============================================================
        // دوال مساعدة للفلترة
        // ============================================================

        /// <summary>
        /// تطبيق الفلاتر على قائمة المستخدمين
        /// </summary>
        public static UserSearchViewModel ApplyFilters(
            IEnumerable<UserDto> users,
            string? searchTerm = null,
            UserType? userType = null,
            UserStatus? status = null,
            int? schoolId = null)
        {
            var filteredUsers = users.AsEnumerable();

            // فلترة حسب البحث
            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredUsers = filteredUsers.Where(u =>
                    u.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (u.Email != null && u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                );
            }

            // فلترة حسب نوع المستخدم
            if (userType.HasValue)
            {
                filteredUsers = filteredUsers.Where(u => u.UserType == userType.Value);
            }

            // فلترة حسب الحالة
            if (status.HasValue)
            {
                filteredUsers = filteredUsers.Where(u => u.Status == status.Value);
            }

            // فلترة حسب المدرسة
            if (schoolId.HasValue)
            {
                filteredUsers = filteredUsers.Where(u => u.SchoolId == schoolId.Value);
            }

            return new UserSearchViewModel
            {
                SearchTerm = searchTerm,
                UserType = userType,
                Status = status,
                SchoolId = schoolId,
                Users = filteredUsers.ToList()
            };
        }
    }
}