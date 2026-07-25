using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.AcademicYears;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.AcademicYears
{
    public class AcademicYearCreateViewModel
    {
        public CreateAcademicYearDto AcademicYear { get; set; } = new();

        [DisplayName("المدارس")]
        public List<SelectListItem> Schools { get; set; } = new();
    }
}