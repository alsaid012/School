using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.EmployeeAttendances;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.EmployeeAttendances
{
    public class EmployeeAttendanceCreateViewModel
    {
        public CreateEmployeeAttendanceDto Attendance { get; set; } = new();

        [DisplayName("الموظفين")]
        public List<SelectListItem> Employees { get; set; } = new();

        [DisplayName("حالات الحضور")]
        public List<SelectListItem> StatusList { get; set; } = new();
    }
}