using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.EmployeeAttendances;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.EmployeeAttendances
{
    public class EmployeeAttendanceIndexViewModel
    {
        [DisplayName("قائمة الحضور")]
        public List<EmployeeAttendanceDto> Attendances { get; set; } = new();

        [DisplayName("الموظفين")]
        public List<SelectListItem> Employees { get; set; } = new();

        [DisplayName("الموظف")]
        public int? SelectedEmployeeId { get; set; }

        [DisplayName("التاريخ")]
        public DateTime SelectedDate { get; set; } = DateTime.Today;
    }
}