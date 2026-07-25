using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.UserRoles
{
    /// <summary>
    /// 📋  معلومات العرض للدور (للقراءة فقط)
    /// </summary>
    public class UserRoleDisplayInfo
    {
        [DisplayName("اسم المستخدم")]
        public string UserName { get; set; } = string.Empty;

        [DisplayName("نوع الدور")]
        public string RoleTypeName { get; set; } = string.Empty;

        [DisplayName("دور أساسي")]
        public bool IsPrimary { get; set; }

        [DisplayName("تاريخ البداية")]
        public DateTime? StartDate { get; set; }

        [DisplayName("تاريخ النهاية")]
        public DateTime? EndDate { get; set; }

        [DisplayName("تاريخ الإنشاء")]
        public DateTime CreatedAt { get; set; }
    }
}