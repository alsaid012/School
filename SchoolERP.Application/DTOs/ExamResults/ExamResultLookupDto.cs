using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.ExamResults
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة لنتائج الامتحانات (ExamResult Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات النتائج للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي عمليات الربط مع جداول أخرى
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ExamResultLookupDto
    {
        /// <summary>
        /// معرف النتيجة
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف النتيجة")]
        public int Id { get; set; }


        public int ExamId { get; set; }

        public int StudentId { get; set; }

        /// <summary>
        /// اسم الطالب
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("اسم الطالب")]
        public string? StudentName { get; set; }

        /// <summary>
        /// اسم الامتحان
        /// </summary>
        /// <example>امتحان اللغة العربية الشهري</example>
        [DisplayName("اسم الامتحان")]
        public string? ExamName { get; set; }

        /// <summary>
        /// الدرجة
        /// </summary>
        /// <example>85</example>
        [DisplayName("الدرجة")]
        public int Score { get; set; }

        /// <summary>
        /// النسبة المئوية
        /// </summary>
        /// <example>85.0</example>
        [DisplayName("النسبة المئوية")]
        public decimal? Percentage { get; set; }

        /// <summary>
        /// التقدير
        /// </summary>
        /// <example>B</example>
        [DisplayName("التقدير")]
        public string? Grade { get; set; }

        /// <summary>
        /// هل الطالب ناجح؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("ناجح")]
        public bool IsPassed { get; set; }

        /// <summary>
        /// هل النتيجة مفعلة؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}