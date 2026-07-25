using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Students
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة للطلاب (Student Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات الطلاب للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي عمليات الربط مع جداول أخرى
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class StudentLookupDto
    {
        /// <summary>
        /// معرف الطالب
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الطالب")]
        public int Id { get; set; }

        /// <summary>
        /// كود الطالب
        /// </summary>
        /// <example>STU-2024-001</example>
        [DisplayName("كود الطالب")]
        public string StudentCode { get; set; } = string.Empty;

        /// <summary>
        /// اسم الطالب (المعروض للمستخدم)
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("اسم الطالب")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// اسم الصف الدراسي
        /// </summary>
        /// <example>الصف الأول الثانوي</example>
        [DisplayName("الصف الدراسي")]
        public string? GradeLevelName { get; set; }

        /// <summary>
        /// اسم الفصل الدراسي
        /// </summary>
        /// <example>1/أ</example>
        [DisplayName("الفصل")]
        public string? ClassRoomName { get; set; }

        /// <summary>
        /// اسم ولي الأمر
        /// </summary>
        /// <example>محمد أحمد</example>
        [DisplayName("اسم ولي الأمر")]
        public string? ParentName { get; set; }

        /// <summary>
        /// تليفون ولي الأمر
        /// </summary>
        /// <example>01001234567</example>
        [DisplayName("تليفون ولي الأمر")]
        public string? ParentPhone { get; set; }

        /// <summary>
        /// هل الطالب مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}