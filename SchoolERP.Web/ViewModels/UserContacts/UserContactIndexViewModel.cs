using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.UserContacts;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.UserContacts
{
    public class UserContactIndexViewModel
    {
        [DisplayName("قائمة جهات الاتصال")]
        public List<UserContactDto> Contacts { get; set; } = new();

        [DisplayName("المستخدمين")]
        public List<SelectListItem> Users { get; set; } = new();

        [DisplayName("المستخدم")]
        public int? SelectedUserId { get; set; }
    }
}