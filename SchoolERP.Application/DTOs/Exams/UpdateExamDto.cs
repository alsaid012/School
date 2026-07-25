using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.Exams
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات الامتحان (Update Exam DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث الامتحان من العميل إلى الخادم
    /// 📦  الاستخدام: في ExamsController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateExamDto
    {
        /// <summary>
        /// اسم الامتحان
        /// </summary>
        /// <example>امتحان اللغة العربية الشهري</example>
        [DisplayName("اسم الامتحان")]
        [MaxLength(100, ErrorMessage = "اسم الامتحان لا يتجاوز 100 حرف")]
        public string? ExamName { get; set; }

        /// <summary>
        /// نوع الامتحان
        /// </summary>
        /// <example>1</example>
        [DisplayName("نوع الامتحان")]
        public ExamType? ExamType { get; set; }

        /// <summary>
        /// تاريخ الامتحان
        /// </summary>
        /// <example>2024-01-15</example>
        [DisplayName("تاريخ الامتحان")]
        public DateTime? ExamDate { get; set; }

        /// <summary>
        /// وقت البداية
        /// </summary>
        /// <example>10:00</example>
        [DisplayName("وقت البداية")]
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// وقت النهاية
        /// </summary>
        /// <example>12:00</example>
        [DisplayName("وقت النهاية")]
        public TimeSpan? EndTime { get; set; }

        /// <summary>
        /// الدرجة النهائية للامتحان
        /// </summary>
        /// <example>100</example>
        [DisplayName("الدرجة النهائية")]
        [Range(1, 1000, ErrorMessage = "الدرجة النهائية يجب أن تكون بين 1 و 1000")]
        public int? MaxScore { get; set; }

        /// <summary>
        /// معرف العام الدراسي
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف العام الدراسي")]
        public int? AcademicYearId { get; set; }

        /// <summary>
        /// معرف المادة
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المادة")]
        public int? SubjectId { get; set; }

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

        /// <summary>
        /// هل الامتحان مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}