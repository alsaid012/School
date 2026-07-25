namespace SchoolERP.Application.DTOs.Governorates
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📍  نموذج بيانات المحافظة (Governorate DTO)
    /// 📌  الوظيفة: نقل بيانات المحافظة من الخادم إلى العميل
    /// 📦  الاستخدام: في GovernoratesController (GET endpoints)
    /// 🔄  يستخدم لعرض بيانات المحافظات في القوائم والتفاصيل
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class GovernorateDto
    {
        /// <summary>
        /// معرف المحافظة (Primary Key)
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// اسم المحافظة
        /// </summary>
        /// <example>القاهرة</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// كود المحافظة (فريد)
        /// </summary>
        /// <example>CAI</example>
        public string? Code { get; set; }

        /// <summary>
        /// عدد الإدارات التعليمية التابعة للمحافظة
        /// </summary>
        /// <example>5</example>
        public int DepartmentsCount { get; set; }

        /// <summary>
        /// هل المحافظة مفعلة؟
        /// </summary>
        /// <example>true</example>
        public bool IsActive { get; set; }

        /// <summary>
        /// تاريخ الإنشاء
        /// </summary>
        /// <example>2024-01-01T12:00:00</example>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// تاريخ آخر تحديث
        /// </summary>
        /// <example>2024-01-15T10:30:00</example>
        public DateTime? UpdatedAt { get; set; }
    }
}