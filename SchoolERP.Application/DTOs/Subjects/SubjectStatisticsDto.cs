using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Subjects
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج إحصائيات المادة الدراسية (Subject Statistics DTO)
    /// 📌  الوظيفة: نقل إحصائيات المادة من الخادم إلى العميل
    /// 📦  الاستخدام: ضمن SubjectDetailsDto أو في لوحة تحكم المادة
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class SubjectStatisticsDto
    {
        /// <summary>
        /// عدد المعلمين الذين يدرسون هذه المادة
        /// </summary>
        /// <example>3</example>
        [DisplayName("عدد المعلمين")]
        public int TotalTeachers { get; set; }

        /// <summary>
        /// عدد الطلاب الذين يدرسون هذه المادة
        /// </summary>
        /// <example>150</example>
        [DisplayName("عدد الطلاب")]
        public int TotalStudents { get; set; }

        /// <summary>
        /// عدد الفصول التي تدرس هذه المادة
        /// </summary>
        /// <example>5</example>
        [DisplayName("عدد الفصول")]
        public int TotalClassRooms { get; set; }

        /// <summary>
        /// عدد الحصص الأسبوعية الإجمالية
        /// </summary>
        /// <example>20</example>
        [DisplayName("إجمالي الحصص الأسبوعية")]
        public int TotalWeeklyHours { get; set; }

        /// <summary>
        /// متوسط درجات الطلاب في هذه المادة
        /// </summary>
        /// <example>78.5</example>
        [DisplayName("متوسط الدرجات")]
        public decimal AverageScore { get; set; }

        /// <summary>
        /// نسبة النجاح في هذه المادة
        /// </summary>
        /// <example>85.0</example>
        [DisplayName("نسبة النجاح")]
        public decimal SuccessRate { get; set; }

        /// <summary>
        /// عدد الامتحانات في هذه المادة
        /// </summary>
        /// <example>12</example>
        [DisplayName("عدد الامتحانات")]
        public int TotalExams { get; set; }

        /// <summary>
        /// أعلى درجة في هذه المادة
        /// </summary>
        /// <example>95</example>
        [DisplayName("أعلى درجة")]
        public int MaxScore { get; set; }

        /// <summary>
        /// أدنى درجة في هذه المادة
        /// </summary>
        /// <example>60</example>
        [DisplayName("أدنى درجة")]
        public int MinScore { get; set; }
    }
}