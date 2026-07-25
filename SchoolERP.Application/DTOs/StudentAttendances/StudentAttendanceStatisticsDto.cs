using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.StudentAttendances
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج إحصائيات حضور الطلاب (StudentAttendance Statistics DTO)
    /// 📌  الوظيفة: نقل إحصائيات الحضور من الخادم إلى العميل
    /// 📦  الاستخدام: ضمن StudentAttendanceDetailsDto أو في لوحة تحكم الحضور
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class StudentAttendanceStatisticsDto
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
        /// عدد الطلاب الذين لديهم حضور كامل
        /// </summary>
        /// <example>10</example>
        [DisplayName("طلاب الحضور الكامل")]
        public int FullAttendanceStudents { get; set; }

        /// <summary>
        /// عدد الطلاب الذين لديهم غياب أكثر من 5 أيام
        /// </summary>
        /// <example>3</example>
        [DisplayName("طلاب الغياب المتكرر")]
        public int FrequentAbsentStudents { get; set; }

        /// <summary>
        /// توزيع الحضور حسب الفصول
        /// </summary>
        [DisplayName("توزيع الحضور حسب الفصول")]
        public Dictionary<string, ClassAttendanceSummaryDto> AttendanceByClass { get; set; } = new();
    }
}