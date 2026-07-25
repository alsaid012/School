namespace SchoolERP.Application.DTOs.Governorates
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة للمحافظات (Governorate Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات المحافظات للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي إنشاء الإدارات والمدارس
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class GovernorateLookupDto
    {
        /// <summary>
        /// معرف المحافظة
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// اسم المحافظة (المعروض للمستخدم)
        /// </summary>
        /// <example>القاهرة</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// كود المحافظة (اختياري)
        /// </summary>
        /// <example>CAI</example>
        public string? Code { get; set; }

        /// <summary>
        /// عدد الإدارات التابعة (اختياري للعرض)
        /// </summary>
        /// <example>5</example>
        public int DepartmentsCount { get; set; }
    }
}