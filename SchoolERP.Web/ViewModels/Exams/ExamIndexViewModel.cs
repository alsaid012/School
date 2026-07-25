using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.Exams;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.Exams
{
    public class ExamIndexViewModel
    {
        [DisplayName("قائمة الامتحانات")]
        public List<ExamDto> Exams { get; set; } = new();

        [DisplayName("السنوات الدراسية")]
        public List<SelectListItem> AcademicYears { get; set; } = new();

        [DisplayName("السنة الدراسية")]
        public int? SelectedAcademicYearId { get; set; }
    }
}