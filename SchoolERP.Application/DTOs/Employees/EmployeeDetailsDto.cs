using SchoolERP.Application.DTOs.EmployeeAttendances;
using SchoolERP.Application.DTOs.Users;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Employees
{
    public class EmployeeDetailsDto : EmployeeDto
    {
        [DisplayName("بيانات المستخدم")]
        public UserDto? User { get; set; }

        [DisplayName("سجلات الحضور")]
        public List<EmployeeAttendanceDto> Attendances { get; set; } = new();

        [DisplayName("إحصائيات الموظف")]
        public EmployeeStatisticsDto? Statistics { get; set; }
    }
}