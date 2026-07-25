using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.UserContacts;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.UserContacts
{
    public class UserContactCreateViewModel
    {
        public CreateUserContactDto Contact { get; set; } = new();

        [DisplayName("المستخدمين")]
        public List<SelectListItem> Users { get; set; } = new();

        [DisplayName("أنواع جهات الاتصال")]
        public List<SelectListItem> ContactTypes { get; set; } = new();
    }
}