using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Exams
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج إحصائيات الامتحان (Exam Statistics DTO)
    /// 📌  الوظيفة: نقل إحصائيات الامتحان من الخادم إلى العميل
    /// 📦  الاستخدام: ضمن ExamDetailsDto أو في لوحة تحكم الامتحان
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ExamStatisticsDto
    {
        /// <summary>
        /// عدد الطلاب الذين تقدموا للامتحان
        /// </summary>
        /// <example>25</example>
        [DisplayName("عدد الطلاب")]
        public int TotalStudents { get; set; }

        /// <summary>
        /// عدد الطلاب الناجحين
        /// </summary>
        /// <example>20</example>
        [DisplayName("عدد الناجحين")]
        public int PassedStudents { get; set; }

        /// <summary>
        /// عدد الطلاب الراسبين
        /// </summary>
        /// <example>5</example>
        [DisplayName("عدد الراسبين")]
        public int FailedStudents { get; set; }

        /// <summary>
        /// نسبة النجاح
        /// </summary>
        /// <example>80.0</example>
        [DisplayName("نسبة النجاح")]
        public decimal SuccessRate { get; set; }

        /// <summary>
        /// متوسط الدرجات
        /// </summary>
        /// <example>78.5</example>
        [DisplayName("متوسط الدرجات")]
        public decimal AverageScore { get; set; }

        /// <summary>
        /// أعلى درجة
        /// </summary>
        /// <example>95</example>
        [DisplayName("أعلى درجة")]
        public int MaxScore { get; set; }

        /// <summary>
        /// أدنى درجة
        /// </summary>
        /// <example>45</example>
        [DisplayName("أدنى درجة")]
        public int MinScore { get; set; }

        /// <summary>
        /// متوسط زمن الإجابة (بالدقائق)
        /// </summary>
        /// <example>90</example>
        [DisplayName("متوسط زمن الإجابة")]
        public int AverageAnswerTime { get; set; }

        /// <summary>
        /// توزيع الدرجات
        /// </summary>
        [DisplayName("توزيع الدرجات")]
        public Dictionary<string, int> ScoreDistribution { get; set; } = new();
    }
}