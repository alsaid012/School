using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.UserRoles;
using System.ComponentModel;
using X.PagedList;
using X.PagedList.Extensions;

namespace SchoolERP.Web.ViewModels.UserRoles
{
    public class UserRoleIndexViewModel
    {
        [DisplayName("قائمة المستخدمين مع أدوارهم")]
        public IPagedList<UserGroupDto> UsersWithRoles { get; set; } = new List<UserGroupDto>().ToPagedList(1, 20);

        [DisplayName("المستخدمين للفلترة")]
        public List<SelectListItem> Users { get; set; } = new();

        [DisplayName("المستخدم المحدد")]
        public int? SelectedUserId { get; set; }

        [DisplayName("إجمالي المستخدمين")]
        public int TotalUsers { get; set; }
    }
}