using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.ExamResults
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات نتيجة الامتحان (Update ExamResult DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث النتيجة من العميل إلى الخادم
    /// 📦  الاستخدام: في ExamResultsController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateExamResultDto
    {
        /// <summary>
        /// درجة الطالب
        /// </summary>
        /// <example>85</example>
        [DisplayName("الدرجة")]
        [Range(0, 1000, ErrorMessage = "الدرجة يجب أن تكون بين 0 و 1000")]
        public int? Score { get; set; }

        /// <summary>
        /// ملاحظات إضافية
        /// </summary>
        /// <example>اجاب جيداً</example>
        [DisplayName("ملاحظات")]
        [MaxLength(500, ErrorMessage = "الملاحظات لا تتجاوز 500 حرف")]
        public string? Remarks { get; set; }

        /// <summary>
        /// هل النتيجة مفعلة؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}