using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.GradeLevels
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج إحصائيات الصف الدراسي (GradeLevel Statistics DTO)
    /// 📌  الوظيفة: نقل إحصائيات الصف من الخادم إلى العميل
    /// 📦  الاستخدام: ضمن GradeLevelDetailsDto أو في لوحة تحكم الصف
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class GradeLevelStatisticsDto
    {
        /// <summary>
        /// عدد الفصول في هذا الصف
        /// </summary>
        /// <example>5</example>
        [DisplayName("عدد الفصول")]
        public int TotalClassRooms { get; set; }

        /// <summary>
        /// عدد المواد في هذا الصف
        /// </summary>
        /// <example>8</example>
        [DisplayName("عدد المواد")]
        public int TotalSubjects { get; set; }

        /// <summary>
        /// عدد الطلاب في هذا الصف
        /// </summary>
        /// <example>150</example>
        [DisplayName("عدد الطلاب")]
        public int TotalStudents { get; set; }

        /// <summary>
        /// عدد المعلمين في هذا الصف
        /// </summary>
        /// <example>10</example>
        [DisplayName("عدد المعلمين")]
        public int TotalTeachers { get; set; }

        /// <summary>
        /// عدد الحصص الأسبوعية في هذا الصف
        /// </summary>
        /// <example>30</example>
        [DisplayName("عدد الحصص الأسبوعية")]
        public int WeeklyHours { get; set; }

        /// <summary>
        /// متوسط عدد الطلاب في الفصل
        /// </summary>
        /// <example>30</example>
        [DisplayName("متوسط عدد الطلاب في الفصل")]
        public int AverageStudentsPerClass { get; set; }

        /// <summary>
        /// عدد الامتحانات في هذا الصف
        /// </summary>
        /// <example>12</example>
        [DisplayName("عدد الامتحانات")]
        public int TotalExams { get; set; }

        /// <summary>
        /// نسبة النجاح في هذا الصف
        /// </summary>
        /// <example>85.0</example>
        [DisplayName("نسبة النجاح")]
        public decimal SuccessRate { get; set; }

        /// <summary>
        /// نسبة الحضور في هذا الصف
        /// </summary>
        /// <example>90.0</example>
        [DisplayName("نسبة الحضور")]
        public decimal AttendanceRate { get; set; }
    }
}