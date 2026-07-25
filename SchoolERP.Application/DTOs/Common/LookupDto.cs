namespace SchoolERP.Application.DTOs.Common
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة (Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات القوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في جميع الـ Dropdown Lists في الـ UI
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class LookupDto
    {
        /// <summary>
        /// المعرف (القيمة المخزنة)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// النص المعروض للمستخدم
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// نص إضافي (اختياري)
        /// </summary>
        public string? AdditionalInfo { get; set; }

        /// <summary>
        /// هل العنصر مفعل؟
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// عنصر افتراضي (اختياري)
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// إنشاء Lookup من كيان
        /// </summary>
        public static LookupDto Create<T>(T entity, Func<T, int> idSelector, Func<T, string> nameSelector)
        {
            return new LookupDto
            {
                Id = idSelector(entity),
                DisplayName = nameSelector(entity)
            };
        }
    }
}