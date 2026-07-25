using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.TeacherSubjects
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات ربط المعلم بالمادة (Update TeacherSubject DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث العلاقة من العميل إلى الخادم
    /// 📦  الاستخدام: في TeacherSubjectsController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateTeacherSubjectDto
    {
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

        /// <summary>
        /// هل العلاقة مفعلة؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }


        public int TeacherId { get; set; }           // ✅ إضافة
        public int SubjectId { get; set; }           // ✅ إضافة
        public string? TeacherName { get; set; }      // ✅ إضافة
        public string? SubjectName { get; set; }      // ✅ إضافة
        public string? GradeLevelName { get; set; }   // ✅ إضافة
    }
}