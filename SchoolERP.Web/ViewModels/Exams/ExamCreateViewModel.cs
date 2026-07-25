using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.Exams;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.Exams
{
    public class ExamCreateViewModel
    {
        public CreateExamDto Exam { get; set; } = new();

        [DisplayName("السنوات الدراسية")]
        public List<SelectListItem> AcademicYears { get; set; } = new();

        [DisplayName("المواد الدراسية")]
        public List<SelectListItem> Subjects { get; set; } = new();

        [DisplayName("الفصول الدراسية")]
        public List<SelectListItem> ClassRooms { get; set; } = new();

        [DisplayName("المعلمين")]
        public List<SelectListItem> Teachers { get; set; } = new();

        [DisplayName("أنواع الامتحانات")]
        public List<SelectListItem> ExamTypes { get; set; } = new();
    }
}