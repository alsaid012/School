using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.EmployeeAttendances
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج إحصائيات حضور الموظفين (EmployeeAttendance Statistics DTO)
    /// 📌  الوظيفة: نقل إحصائيات الحضور من الخادم إلى العميل
    /// 📦  الاستخدام: ضمن EmployeeAttendanceDetailsDto أو في لوحة تحكم الحضور
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class EmployeeAttendanceStatisticsDto
    {
        /// <summary>
        /// إجمالي عدد أيام الحضور المسجلة
        /// </summary>
        /// <example>30</example>
        [DisplayName("إجمالي أيام الحضور")]
        public int TotalAttendanceDays { get; set; }

     
        /// <summary>
        /// عدد أيام الحضور (حاضر)
        /// </summary>
        /// <example>25</example>
        [DisplayName("أيام الحضور")]
        public int PresentDays { get; set; }

        /// <summary>
        /// عدد أيام الغياب
        /// </summary>
        /// <example>3</example>
        [DisplayName("أيام الغياب")]
        public int AbsentDays { get; set; }

        /// <summary>
        /// عدد أيام التأخير
        /// </summary>
        /// <example>2</example>
        [DisplayName("أيام التأخير")]
        public int LateDays { get; set; }

        /// <summary>
        /// عدد أيام العذر
        /// </summary>
        /// <example>1</example>
        [DisplayName("أيام العذر")]
        public int ExcusedDays { get; set; }

        /// <summary>
        /// نسبة الحضور
        /// </summary>
        /// <example>90.0</example>
        [DisplayName("نسبة الحضور")]
        public decimal AttendancePercentage { get; set; }

        /// <summary>
        /// أعلى عدد أيام حضور
        /// </summary>
        /// <example>25</example>
        [DisplayName("أعلى أيام حضور")]
        public int MaxAttendanceDays { get; set; }

        /// <summary>
        /// أدنى عدد أيام حضور
        /// </summary>
        /// <example>15</example>
        [DisplayName("أدنى أيام حضور")]
        public int MinAttendanceDays { get; set; }

        /// <summary>
        /// متوسط أيام الحضور
        /// </summary>
        /// <example>20</example>
        [DisplayName("متوسط أيام الحضور")]
        public decimal AverageAttendanceDays { get; set; }

        /// <summary>
        /// عدد الموظفين الذين لديهم حضور كامل
        /// </summary>
        /// <example>5</example>
        [DisplayName("موظفين الحضور الكامل")]
        public int FullAttendanceEmployees { get; set; }

        /// <summary>
        /// عدد الموظفين الذين لديهم غياب أكثر من 5 أيام
        /// </summary>
        /// <example>2</example>
        [DisplayName("موظفين الغياب المتكرر")]
        public int FrequentAbsentEmployees { get; set; }

        /// <summary>
        /// توزيع الحضور حسب الأقسام
        /// </summary>
        [DisplayName("توزيع الحضور حسب الأقسام")]
        public Dictionary<string, DepartmentAttendanceSummaryDto> AttendanceByDepartment { get; set; } = new();
    }
}