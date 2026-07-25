using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.StudentAttendances;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.StudentAttendances
{
    public class StudentAttendanceIndexViewModel
    {
        [DisplayName("قائمة الحضور")]
        public List<StudentAttendanceDto> Attendances { get; set; } = new();

        [DisplayName("الطلاب")]
        public List<SelectListItem> Students { get; set; } = new();

        [DisplayName("الطالب")]
        public int? SelectedStudentId { get; set; }

        [DisplayName("التاريخ")]
        public DateTime SelectedDate { get; set; } = DateTime.Today;
    }
}