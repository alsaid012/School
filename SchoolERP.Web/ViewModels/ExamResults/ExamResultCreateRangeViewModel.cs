using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.ExamResults;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.ExamResults
{
    public class ExamResultCreateRangeViewModel
    {
        public int ExamId { get; set; }

        [DisplayName("الامتحان")]
        public List<SelectListItem> Exams { get; set; } = new();

        [DisplayName("الطلاب المتاحين")]
        public List<SelectListItem> AvailableStudents { get; set; } = new();

        [DisplayName("الطلاب المختارين")]
        public List<int> SelectedStudentIds { get; set; } = new();

        [DisplayName("النتائج")]
        public List<CreateExamResultDto> ExamResults { get; set; } = new();
    }
}