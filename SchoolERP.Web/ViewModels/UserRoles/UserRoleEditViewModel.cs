using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.UserRoles;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.UserRoles
{
    /// <summary>
    /// ✏️  نموذج تعديل دور مستخدم (Edit ViewModel)
    /// </summary>
    public class UserRoleEditViewModel
    {
        public int Id { get; set; }

        public UpdateUserRoleDto Role { get; set; } = new();

        public UserRoleDisplayInfo DisplayInfo { get; set; } = new();
    }
}