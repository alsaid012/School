using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.ExamResults;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.ExamResults
{
    public class ExamResultIndexViewModel
    {
        [DisplayName("قائمة النتائج")]
        public List<ExamResultDto> Results { get; set; } = new();

        [DisplayName("الامتحانات")]
        public List<SelectListItem> Exams { get; set; } = new();

        [DisplayName("الامتحان")]
        public int? SelectedExamId { get; set; }
    }
}