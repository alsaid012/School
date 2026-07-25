using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.StudentAttendances;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.StudentAttendances
{
    public class StudentAttendanceCreateViewModel
    {
        public CreateStudentAttendanceDto Attendance { get; set; } = new();

        [DisplayName("الطلاب")]
        public List<SelectListItem> Students { get; set; } = new();

        [DisplayName("حالات الحضور")]
        public List<SelectListItem> StatusList { get; set; } = new();
    }
}