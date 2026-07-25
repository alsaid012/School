using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.EmployeeAttendances;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.EmployeeAttendances
{
    public class EmployeeAttendanceCreateRangeViewModel
    {
        public string Department { get; set; } = string.Empty;

        [DisplayName("الأقسام")]
        public List<SelectListItem> Departments { get; set; } = new();

        [DisplayName("التاريخ")]
        public DateTime Date { get; set; } = DateTime.Today;

        [DisplayName("حالات الحضور")]
        public List<SelectListItem> StatusList { get; set; } = new();

        [DisplayName("سجلات الحضور")]
        public List<CreateEmployeeAttendanceDto> Attendances { get; set; } = new();
    }
}