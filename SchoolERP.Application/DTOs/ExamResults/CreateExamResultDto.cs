using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.ExamResults
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء نتيجة امتحان جديدة (Create ExamResult DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء النتيجة من العميل إلى الخادم
    /// 📦  الاستخدام: في ExamResultsController (POST endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateExamResultDto
    {
        /// <summary>
        /// معرف الامتحان (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الامتحان")]
        [Required(ErrorMessage = "معرف الامتحان مطلوب")]
        public int ExamId { get; set; }

        /// <summary>
        /// معرف الطالب (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الطالب")]
        [Required(ErrorMessage = "معرف الطالب مطلوب")]
        public int StudentId { get; set; }

        /// <summary>
        /// درجة الطالب (مطلوبة)
        /// </summary>
        /// <example>85</example>
        [DisplayName("الدرجة")]
        [Required(ErrorMessage = "الدرجة مطلوبة")]
        [Range(0, 1000, ErrorMessage = "الدرجة يجب أن تكون بين 0 و 1000")]
        public int Score { get; set; }

        /// <summary>
        /// ملاحظات إضافية
        /// </summary>
        /// <example>اجاب جيداً</example>
        [DisplayName("ملاحظات")]
        [MaxLength(500, ErrorMessage = "الملاحظات لا تتجاوز 500 حرف")]
        public string? Remarks { get; set; }
    }
}