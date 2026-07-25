using System.ComponentModel;

namespace SchoolERP.Application.DTOs.EmployeeAttendances
{
    /// <summary>
    /// 📊  نموذج ملخص حضور القسم
    /// </summary>
    public class DepartmentAttendanceSummaryDto
    {
        /// <summary>
        /// عدد الموظفين في القسم
        /// </summary>
        [DisplayName("عدد الموظفين")]
        public int TotalEmployees { get; set; }

        /// <summary>
        /// عدد أيام الحضور
        /// </summary>
        [DisplayName("أيام الحضور")]
        public int PresentDays { get; set; }

        /// <summary>
        /// عدد أيام الغياب
        /// </summary>
        [DisplayName("أيام الغياب")]
        public int AbsentDays { get; set; }

        /// <summary>
        /// عدد أيام التأخير
        /// </summary>
        [DisplayName("أيام التأخير")]
        public int LateDays { get; set; }

        /// <summary>
        /// نسبة الحضور
        /// </summary>
        [DisplayName("نسبة الحضور")]
        public decimal AttendancePercentage { get; set; }
    }
}