namespace SchoolERP.Application.DTOs.Schools
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة للمدارس (School Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات المدارس للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي Departments/Details
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class SchoolLookupDto
    {
        /// <summary>
        /// معرف المدرسة
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// اسم المدرسة (المعروض للمستخدم)
        /// </summary>
        /// <example>مدرسة النصر الثانوية</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// كود المدرسة
        /// </summary>
        /// <example>SCH-001</example>
        public string? Code { get; set; }

        /// <summary>
        /// نوع المدرسة
        /// </summary>
        /// <example>عامة</example>
        public string? SchoolType { get; set; }

        /// <summary>
        /// اسم مدير المدرسة
        /// </summary>
        /// <example>أ. حسين علي</example>
        public string? PrincipalName { get; set; }

        /// <summary>
        /// هل المدرسة مفعلة؟
        /// </summary>
        /// <example>true</example>
        public bool IsActive { get; set; }
    }
}