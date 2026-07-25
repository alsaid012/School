using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.ExamResults
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏆  نموذج ترتيب الطالب (Student Rank DTO)
    /// 📌  الوظيفة: نقل بيانات ترتيب الطالب في الامتحان
    /// 📦  الاستخدام: ضمن ExamResultStatisticsDto
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class StudentRankDto
    {
        /// <summary>
        /// معرف الطالب
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الطالب")]
        public int StudentId { get; set; }

        /// <summary>
        /// اسم الطالب
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("اسم الطالب")]
        public string StudentName { get; set; } = string.Empty;

        /// <summary>
        /// درجة الطالب
        /// </summary>
        /// <example>95</example>
        [DisplayName("الدرجة")]
        public int Score { get; set; }

        /// <summary>
        /// النسبة المئوية
        /// </summary>
        /// <example>95.0</example>
        [DisplayName("النسبة المئوية")]
        public decimal Percentage { get; set; }

        /// <summary>
        /// الترتيب (الأول، الثاني، ...)
        /// </summary>
        /// <example>1</example>
        [DisplayName("الترتيب")]
        public int Rank { get; set; }

        /// <summary>
        /// التقدير (A, B, C, D, F)
        /// </summary>
        /// <example>A</example>
        [DisplayName("التقدير")]
        public string? Grade { get; set; }

        /// <summary>
        /// هل الطالب ناجح؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("ناجح")]
        public bool IsPassed { get; set; }
    }
}