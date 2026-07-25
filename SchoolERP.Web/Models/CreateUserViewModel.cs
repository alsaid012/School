using SchoolERP.Application.DTOs.Users;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Web.Models
{
    public class CreateUserViewModel
    {
        public CreateUserDto User { get; set; } = new();
        public List<School> Schools { get; set; } = new();
    }
}