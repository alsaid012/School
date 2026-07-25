namespace SchoolERP.Application.DTOs.Departments
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة للإدارات (Department Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات الإدارات للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي إنشاء المدارس
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class DepartmentLookupDto
    {
        /// <summary>
        /// معرف الإدارة
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// اسم الإدارة (المعروض للمستخدم)
        /// </summary>
        /// <example>إدارة شمال القاهرة التعليمية</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// كود الإدارة
        /// </summary>
        /// <example>SH-NORTH-CAIRO</example>
        public string? Code { get; set; }

        /// <summary>
        /// اسم المحافظة التابعة لها
        /// </summary>
        /// <example>القاهرة</example>
        public string? GovernorateName { get; set; }

        /// <summary>
        /// عدد المدارس التابعة
        /// </summary>
        /// <example>10</example>
        public int SchoolsCount { get; set; }
    }
}