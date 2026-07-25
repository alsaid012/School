using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.TeacherSubjects
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء ربط جديد بين المعلم والمادة (Create TeacherSubject DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء العلاقة من العميل إلى الخادم
    /// 📦  الاستخدام: في TeacherSubjectsController (POST endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateTeacherSubjectDto
    {
        /// <summary>
        /// معرف المعلم (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المعلم")]
        [Required(ErrorMessage = "معرف المعلم مطلوب")]
        public int TeacherId { get; set; }

        /// <summary>
        /// معرف المادة (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المادة")]
        [Required(ErrorMessage = "معرف المادة مطلوب")]
        public int SubjectId { get; set; }

        /// <summary>
        /// هل هي المادة الأساسية للمعلم؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مادة أساسية")]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// عدد الحصص الأسبوعية لهذه المادة مع هذا المعلم
        /// </summary>
        /// <example>4</example>
        [DisplayName("عدد الحصص الأسبوعية")]
        [Range(1, 10, ErrorMessage = "عدد الحصص يجب أن يكون بين 1 و 10")]
        public int? WeeklyHours { get; set; }
    }
}