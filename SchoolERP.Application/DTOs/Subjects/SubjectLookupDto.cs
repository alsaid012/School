using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Subjects
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة للمواد الدراسية (Subject Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات المواد للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي عمليات الربط مع جداول أخرى
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class SubjectLookupDto
    {
        /// <summary>
        /// معرف المادة
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المادة")]
        public int Id { get; set; }

        /// <summary>
        /// اسم المادة (المعروض للمستخدم)
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
        /// عدد الحصص الأسبوعية
        /// </summary>
        /// <example>4</example>
        [DisplayName("عدد الحصص الأسبوعية")]
        public int? WeeklyHours { get; set; }

        /// <summary>
        /// هل المادة إجبارية؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مادة إجبارية")]
        public bool IsRequired { get; set; }

        /// <summary>
        /// هل المادة مفعلة؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}