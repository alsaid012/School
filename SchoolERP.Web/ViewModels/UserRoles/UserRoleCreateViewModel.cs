using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.UserRoles;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.UserRoles
{
    /// <summary>
    /// ➕  نموذج إنشاء دور مستخدم جديد (Create ViewModel)
    /// </summary>
    public class UserRoleCreateViewModel
    {
        public CreateUserRoleDto Role { get; set; } = new();

        [DisplayName("المستخدمين")]
        public List<SelectListItem> Users { get; set; } = new();

        [DisplayName("أنواع الأدوار")]
        public List<SelectListItem> RoleTypes { get; set; } = new();
    }
}