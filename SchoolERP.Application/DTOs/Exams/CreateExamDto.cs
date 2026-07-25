using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.Exams
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء امتحان جديد (Create Exam DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء الامتحان من العميل إلى الخادم
    /// 📦  الاستخدام: في ExamsController (POST endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateExamDto
    {
        /// <summary>
        /// اسم الامتحان (مطلوب)
        /// </summary>
        /// <example>امتحان اللغة العربية الشهري</example>
        [DisplayName("اسم الامتحان")]
        [Required(ErrorMessage = "اسم الامتحان مطلوب")]
        [MaxLength(100, ErrorMessage = "اسم الامتحان لا يتجاوز 100 حرف")]
        public string ExamName { get; set; } = string.Empty;

        /// <summary>
        /// نوع الامتحان (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("نوع الامتحان")]
        [Required(ErrorMessage = "نوع الامتحان مطلوب")]
        public ExamType ExamType { get; set; }

        /// <summary>
        /// تاريخ الامتحان (مطلوب)
        /// </summary>
        /// <example>2024-01-15</example>
        [DisplayName("تاريخ الامتحان")]
        [Required(ErrorMessage = "تاريخ الامتحان مطلوب")]
        public DateTime ExamDate { get; set; }

        /// <summary>
        /// وقت البداية (مطلوب)
        /// </summary>
        /// <example>10:00</example>
        [DisplayName("وقت البداية")]
        [Required(ErrorMessage = "وقت البداية مطلوب")]
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// وقت النهاية (مطلوب)
        /// </summary>
        /// <example>12:00</example>
        [DisplayName("وقت النهاية")]
        [Required(ErrorMessage = "وقت النهاية مطلوب")]
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// الدرجة النهائية للامتحان (مطلوب)
        /// </summary>
        /// <example>100</example>
        [DisplayName("الدرجة النهائية")]
        [Required(ErrorMessage = "الدرجة النهائية مطلوبة")]
        [Range(1, 1000, ErrorMessage = "الدرجة النهائية يجب أن تكون بين 1 و 1000")]
        public int MaxScore { get; set; }

        /// <summary>
        /// معرف العام الدراسي (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف العام الدراسي")]
        [Required(ErrorMessage = "معرف العام الدراسي مطلوب")]
        public int AcademicYearId { get; set; }

        /// <summary>
        /// معرف المادة (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المادة")]
        [Required(ErrorMessage = "معرف المادة مطلوب")]
        public int SubjectId { get; set; }

        /// <summary>
        /// معرف الفصل الدراسي
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الفصل")]
        public int? ClassRoomId { get; set; }

        /// <summary>
        /// معرف المعلم المشرف على الامتحان
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المعلم")]
        public int? TeacherId { get; set; }

        /// <summary>
        /// ملاحظات إضافية
        /// </summary>
        /// <example>امتحان شامل</example>
        [DisplayName("ملاحظات")]
        [MaxLength(500, ErrorMessage = "الملاحظات لا تتجاوز 500 حرف")]
        public string? Notes { get; set; }
    }
}