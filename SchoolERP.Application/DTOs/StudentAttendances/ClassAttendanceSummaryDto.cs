using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.StudentAttendances
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج ملخص حضور الفصل (Class Attendance Summary DTO)
    /// 📌  الوظيفة: نقل بيانات ملخص حضور الفصل من الخادم إلى العميل
    /// 📦  الاستخدام: ضمن StudentAttendanceStatisticsDto
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ClassAttendanceSummaryDto
    {
        /// <summary>
        /// عدد الطلاب في الفصل
        /// </summary>
        /// <example>30</example>
        [DisplayName("عدد الطلاب")]
        public int TotalStudents { get; set; }

        /// <summary>
        /// عدد أيام الحضور
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
        /// نسبة الحضور (مئوية)
        /// </summary>
        /// <example>90.0</example>
        [DisplayName("نسبة الحضور")]
        public decimal AttendancePercentage { get; set; }
    }
}