using System.ComponentModel;

namespace SchoolERP.Application.DTOs.UserRoles
{
    /// <summary>
    /// 🏆  نموذج ترتيب المستخدمين حسب عدد الأدوار
    /// </summary>
    public class TopUserRolesDto
    {
        /// <summary>
        /// معرف المستخدم
        /// </summary>
        [DisplayName("معرف المستخدم")]
        public int UserId { get; set; }

        /// <summary>
        /// اسم المستخدم
        /// </summary>
        [DisplayName("اسم المستخدم")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// عدد الأدوار
        /// </summary>
        [DisplayName("عدد الأدوار")]
        public int RolesCount { get; set; }

        /// <summary>
        /// قائمة الأدوار
        /// </summary>
        [DisplayName("قائمة الأدوار")]
        public List<string> RoleNames { get; set; } = new();

        /// <summary>
        /// هل لديه دور أساسي؟
        /// </summary>
        [DisplayName("دور أساسي")]
        public bool HasPrimaryRole { get; set; }
    }
}