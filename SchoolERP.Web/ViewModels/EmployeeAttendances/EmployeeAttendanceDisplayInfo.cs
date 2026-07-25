using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.EmployeeAttendances
{
    public class EmployeeAttendanceDisplayInfo
    {
        [DisplayName("اسم الموظف")]
        public string EmployeeName { get; set; } = string.Empty;

        [DisplayName("كود الموظف")]
        public string EmployeeCode { get; set; } = string.Empty;

        [DisplayName("المسمى الوظيفي")]
        public string JobTitle { get; set; } = string.Empty;

        [DisplayName("القسم")]
        public string Department { get; set; } = string.Empty;

        [DisplayName("المدرسة")]
        public string SchoolName { get; set; } = string.Empty;

        [DisplayName("تاريخ الحضور")]
        public DateTime AttendanceDate { get; set; }

        [DisplayName("الحالة الحالية")]
        public string CurrentStatus { get; set; } = string.Empty;
    }
}