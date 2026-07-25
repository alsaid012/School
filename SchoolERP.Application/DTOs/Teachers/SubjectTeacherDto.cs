using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Teachers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📖  نموذج المادة التي يدرسها المعلم (Teacher Subject DTO)
    /// 📌  الوظيفة: نقل بيانات المادة التي يدرسها المعلم
    /// 📦  الاستخدام: ضمن TeacherDto أو TeacherDetailsDto
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class SubjectTeacherDto
    {
        /// <summary>
        /// معرف المادة
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المادة")]
        public int SubjectId { get; set; }

        /// <summary>
        /// اسم المادة
        /// </summary>
        /// <example>اللغة العربية</example>
        [DisplayName("اسم المادة")]
        public string SubjectName { get; set; } = string.Empty;

        /// <summary>
        /// كود المادة
        /// </summary>
        /// <example>SUB-AR-001</example>
        [DisplayName("كود المادة")]
        public string? SubjectCode { get; set; }

        /// <summary>
        /// اسم الصف الدراسي
        /// </summary>
        /// <example>الصف الأول الثانوي</example>
        [DisplayName("الصف الدراسي")]
        public string? GradeLevelName { get; set; }

        /// <summary>
        /// هل هي المادة الأساسية للمعلم؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مادة أساسية")]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// عدد الحصص الأسبوعية
        /// </summary>
        /// <example>4</example>
        [DisplayName("عدد الحصص الأسبوعية")]
        public int? WeeklyHours { get; set; }
    }
}