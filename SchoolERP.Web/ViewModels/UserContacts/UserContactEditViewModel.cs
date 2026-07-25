using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.UserContacts;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.UserContacts
{
    public class UserContactEditViewModel
    {
        public int Id { get; set; }

        public UpdateUserContactDto Contact { get; set; } = new();

        public UserContactDisplayInfo DisplayInfo { get; set; } = new();
    }
}