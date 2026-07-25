using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.TeacherSubjects
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة لربط المعلم بالمادة (TeacherSubject Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات العلاقات للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي عمليات الربط مع جداول أخرى
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class TeacherSubjectLookupDto
    {
        /// <summary>
        /// معرف العلاقة
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف العلاقة")]
        public int Id { get; set; }

        /// <summary>
        /// معرف المعلم
        /// </summary>
        [DisplayName("معرف المعلم")]
        public int TeacherId { get; set; }  // ✅ إضافة

        /// <summary>
        /// معرف المادة
        /// </summary>
        [DisplayName("معرف المادة")]
        public int SubjectId { get; set; }  // ✅ إضافة



        /// <summary>
        /// اسم المعلم (المعروض للمستخدم)
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("اسم المعلم")]
        public string TeacherName { get; set; } = string.Empty;

        /// <summary>
        /// اسم المادة (المعروض للمستخدم)
        /// </summary>
        /// <example>اللغة العربية</example>
        [DisplayName("اسم المادة")]
        public string SubjectName { get; set; } = string.Empty;

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
        /// هل العلاقة مفعلة؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}