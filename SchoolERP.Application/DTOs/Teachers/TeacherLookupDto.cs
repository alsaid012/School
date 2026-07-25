using System.ComponentModel;

namespace SchoolERP.Application.DTOs.Teachers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة للمعلمين (Teacher Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات المعلمين للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي عمليات الربط مع جداول أخرى
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class TeacherLookupDto
    {
        /// <summary>
        /// معرف المعلم
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المعلم")]
        public int Id { get; set; }

        /// <summary>
        /// كود المعلم
        /// </summary>
        /// <example>TCH-2024-001</example>
        [DisplayName("كود المعلم")]
        public string TeacherCode { get; set; } = string.Empty;

        /// <summary>
        /// اسم المعلم (المعروض للمستخدم)
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("اسم المعلم")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// التخصص
        /// </summary>
        /// <example>اللغة العربية</example>
        [DisplayName("التخصص")]
        public string? Specialization { get; set; }

        /// <summary>
        /// عدد المواد التي يدرسها
        /// </summary>
        /// <example>3</example>
        [DisplayName("عدد المواد")]
        public int SubjectsCount { get; set; }

        /// <summary>
        /// هل هو معلم فصل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("معلم فصل")]
        public bool IsHomeroomTeacher { get; set; }

        /// <summary>
        /// اسم المدرسة
        /// </summary>
        /// <example>مدرسة النصر الثانوية</example>
        [DisplayName("المدرسة")]
        public string? SchoolName { get; set; }

        /// <summary>
        /// هل المعلم مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}