using SchoolERP.Application.DTOs.UserRoles;

namespace SchoolERP.Web.ViewModels.UserRoles
{
    public class UserGroupDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public List<UserRoleDto> Roles { get; set; } = new();
        public bool HasPrimaryRole => Roles.Any(r => r.IsPrimary);
        public int RolesCount => Roles.Count;
    }
}