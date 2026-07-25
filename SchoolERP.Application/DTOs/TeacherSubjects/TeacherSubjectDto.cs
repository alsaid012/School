using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.TeacherSubjects
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🔗  نموذج بيانات ربط المعلم بالمادة (TeacherSubject DTO)
    /// 📌  الوظيفة: نقل بيانات العلاقة بين المعلم والمادة من الخادم إلى العميل
    /// 📦  الاستخدام: في TeacherSubjectsController (GET endpoints)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class TeacherSubjectDto
    {
        /// <summary>
        /// معرف العلاقة (Primary Key)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف العلاقة")]
        public int Id { get; set; }

        /// <summary>
        /// معرف المعلم
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المعلم")]
        public int TeacherId { get; set; }

        /// <summary>
        /// اسم المعلم
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("اسم المعلم")]
        public string TeacherName { get; set; } = string.Empty;

        /// <summary>
        /// كود المعلم
        /// </summary>
        /// <example>TCH-2024-001</example>
        [DisplayName("كود المعلم")]
        public string TeacherCode { get; set; } = string.Empty;

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
        /// عدد الحصص الأسبوعية لهذه المادة مع هذا المعلم
        /// </summary>
        /// <example>4</example>
        [DisplayName("عدد الحصص الأسبوعية")]
        public int? WeeklyHours { get; set; }

        /// <summary>
        /// هل العلاقة مفعلة؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }

        /// <summary>
        /// تاريخ الإنشاء
        /// </summary>
        /// <example>2024-01-01T12:00:00</example>
        [DisplayName("تاريخ الإنشاء")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// تاريخ آخر تحديث
        /// </summary>
        /// <example>2024-01-15T10:30:00</example>
        [DisplayName("تاريخ التحديث")]
        public DateTime? UpdatedAt { get; set; }
    }
}