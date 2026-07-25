using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.ExamResults
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج إحصائيات نتائج الامتحانات (ExamResult Statistics DTO)
    /// 📌  الوظيفة: نقل إحصائيات النتائج من الخادم إلى العميل
    /// 📦  الاستخدام: ضمن ExamResultDetailsDto أو في لوحة تحكم النتائج
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ExamResultStatisticsDto
    {
        /// <summary>
        /// عدد الطلاب الكلي
        /// </summary>
        /// <example>25</example>
        [DisplayName("عدد الطلاب الكلي")]
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
        /// عدد الطلاب المتفوقين (درجة >= 90)
        /// </summary>
        /// <example>8</example>
        [DisplayName("عدد المتفوقين")]
        public int ExcellentStudents { get; set; }

        /// <summary>
        /// عدد الطلاب الجيدين (درجة >= 80)
        /// </summary>
        /// <example>12</example>
        [DisplayName("عدد الجيدين")]
        public int GoodStudents { get; set; }

        /// <summary>
        /// عدد الطلاب المقبولين (درجة >= 50)
        /// </summary>
        /// <example>5</example>
        [DisplayName("عدد المقبولين")]
        public int PassedOnlyStudents { get; set; }

        /// <summary>
        /// عدد الطلاب الراسبين (درجة < 50)
        /// </summary>
        /// <example>0</example>
        [DisplayName("عدد الراسبين")]
        public int FailedOnlyStudents { get; set; }

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
        /// نسبة النجاح
        /// </summary>
        /// <example>80.0</example>
        [DisplayName("نسبة النجاح")]
        public decimal SuccessRate { get; set; }

        /// <summary>
        /// توزيع الدرجات حسب التقديرات
        /// </summary>
        [DisplayName("توزيع الدرجات")]
        public Dictionary<string, int> GradeDistribution { get; set; } = new();

        /// <summary>
        /// ترتيب الطلاب (من الأعلى إلى الأدنى)
        /// </summary>
        [DisplayName("ترتيب الطلاب")]
        public List<StudentRankDto> StudentRanks { get; set; } = new();
    }
}