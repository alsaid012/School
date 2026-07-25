using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.EmployeeAttendances;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.EmployeeAttendances
{
    public class EmployeeAttendanceEditViewModel
    {
        public int Id { get; set; }

        public UpdateEmployeeAttendanceDto Attendance { get; set; } = new();

        public EmployeeAttendanceDisplayInfo DisplayInfo { get; set; } = new();
    }
}