using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.AcademicYears
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج إحصائيات العام الدراسي (AcademicYear Statistics DTO)
    /// 📌  الوظيفة: نقل إحصائيات العام الدراسي من الخادم إلى العميل
    /// 📦  الاستخدام: ضمن AcademicYearDetailsDto أو في لوحة تحكم العام الدراسي
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class AcademicYearStatisticsDto
    {
        /// <summary>
        /// إجمالي عدد الطلاب المسجلين في هذا العام
        /// </summary>
        /// <example>500</example>
        [DisplayName("إجمالي الطلاب")]
        public int TotalStudents { get; set; }

        /// <summary>
        /// إجمالي عدد المعلمين في هذا العام
        /// </summary>
        /// <example>50</example>
        [DisplayName("إجمالي المعلمين")]
        public int TotalTeachers { get; set; }

        /// <summary>
        /// إجمالي عدد الموظفين في هذا العام
        /// </summary>
        /// <example>20</example>
        [DisplayName("إجمالي الموظفين")]
        public int TotalEmployees { get; set; }

        /// <summary>
        /// عدد الفصول الدراسية في هذا العام
        /// </summary>
        /// <example>25</example>
        [DisplayName("عدد الفصول")]
        public int TotalClassRooms { get; set; }

        /// <summary>
        /// عدد المواد الدراسية في هذا العام
        /// </summary>
        /// <example>8</example>
        [DisplayName("عدد المواد")]
        public int TotalSubjects { get; set; }

        /// <summary>
        /// عدد الامتحانات في هذا العام
        /// </summary>
        /// <example>12</example>
        [DisplayName("عدد الامتحانات")]
        public int TotalExams { get; set; }

        /// <summary>
        /// عدد الحصص الأسبوعية الإجمالية
        /// </summary>
        /// <example>120</example>
        [DisplayName("عدد الحصص الأسبوعية")]
        public int TotalWeeklyHours { get; set; }

        /// <summary>
        /// نسبة الحضور الإجمالية
        /// </summary>
        /// <example>90.0</example>
        [DisplayName("نسبة الحضور الإجمالية")]
        public decimal OverallAttendanceRate { get; set; }

        /// <summary>
        /// نسبة النجاح الإجمالية
        /// </summary>
        /// <example>85.0</example>
        [DisplayName("نسبة النجاح الإجمالية")]
        public decimal OverallSuccessRate { get; set; }

        /// <summary>
        /// عدد الأيام الدراسية في هذا العام
        /// </summary>
        /// <example>180</example>
        [DisplayName("عدد الأيام الدراسية")]
        public int TotalSchoolDays { get; set; }

        /// <summary>
        /// عدد الأيام المتبقية في هذا العام
        /// </summary>
        /// <example>45</example>
        [DisplayName("عدد الأيام المتبقية")]
        public int RemainingSchoolDays { get; set; }

        /// <summary>
        /// توزيع الطلاب حسب الصفوف
        /// </summary>
        [DisplayName("توزيع الطلاب حسب الصفوف")]
        public Dictionary<string, int> StudentDistributionByGrade { get; set; } = new();
    }
}