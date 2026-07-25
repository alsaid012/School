using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.ExamResults;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.ExamResults
{
    public class ExamResultCreateViewModel
    {
        public CreateExamResultDto ExamResult { get; set; } = new();

        [DisplayName("الامتحانات")]
        public List<SelectListItem> Exams { get; set; } = new();

        [DisplayName("الطلاب")]
        public List<SelectListItem> Students { get; set; } = new();
    }
}