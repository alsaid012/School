namespace SchoolERP.Application.DTOs.Departments
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏢  نموذج بيانات الإدارة التعليمية (Department DTO)
    /// 📌  الوظيفة: نقل بيانات الإدارة من الخادم إلى العميل
    /// 📦  الاستخدام: في DepartmentsController (GET endpoints)
    /// 🔄  يستخدم لعرض بيانات الإدارات في القوائم والتفاصيل
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class DepartmentDto
    {
        /// <summary>
        /// معرف الإدارة (Primary Key)
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// اسم الإدارة التعليمية
        /// </summary>
        /// <example>إدارة شمال القاهرة التعليمية</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// كود الإدارة (فريد)
        /// </summary>
        /// <example>SH-NORTH-CAIRO</example>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// اسم مدير الإدارة
        /// </summary>
        /// <example>أ. محمد أحمد</example>
        public string? DirectorName { get; set; }

        /// <summary>
        /// رقم هاتف الإدارة
        /// </summary>
        /// <example>0223456789</example>
        public string? Phone { get; set; }

        /// <summary>
        /// البريد الإلكتروني للإدارة
        /// </summary>
        /// <example>north.cairo@moedu.gov.eg</example>
        public string? Email { get; set; }

        /// <summary>
        /// عنوان الإدارة
        /// </summary>
        /// <example>شمال القاهرة - مصر الجديدة</example>
        public string? Address { get; set; }

        /// <summary>
        /// معرف المحافظة التابعة لها
        /// </summary>
        /// <example>1</example>
        public int GovernorateId { get; set; }

        /// <summary>
        /// اسم المحافظة
        /// </summary>
        /// <example>القاهرة</example>
        public string? GovernorateName { get; set; }

        /// <summary>
        /// عدد المدارس التابعة للإدارة
        /// </summary>
        /// <example>10</example>
        public int SchoolsCount { get; set; }

        /// <summary>
        /// هل الإدارة مفعلة؟
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