using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Departments
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات الإدارة التعليمية (Update Department DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث الإدارة من العميل إلى الخادم
    /// 📦  الاستخدام: في DepartmentsController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateDepartmentDto
    {
        /// <summary>
        /// اسم الإدارة التعليمية
        /// </summary>
        /// <example>إدارة شمال القاهرة التعليمية</example>
        [MaxLength(100, ErrorMessage = "اسم الإدارة لا يتجاوز 100 حرف")]
        public string? Name { get; set; }

        /// <summary>
        /// كود الإدارة
        /// </summary>
        /// <example>SH-NORTH-CAIRO</example>
        [MaxLength(20, ErrorMessage = "كود الإدارة لا يتجاوز 20 حرف")]
        public string? Code { get; set; }

        /// <summary>
        /// اسم مدير الإدارة
        /// </summary>
        /// <example>أ. محمد أحمد</example>
        [MaxLength(100, ErrorMessage = "اسم المدير لا يتجاوز 100 حرف")]
        public string? DirectorName { get; set; }

        /// <summary>
        /// رقم هاتف الإدارة
        /// </summary>
        /// <example>0223456789</example>
        [Phone(ErrorMessage = "صيغة رقم الهاتف غير صحيحة")]
        public string? Phone { get; set; }

        /// <summary>
        /// البريد الإلكتروني للإدارة
        /// </summary>
        /// <example>north.cairo@moedu.gov.eg</example>
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string? Email { get; set; }

        /// <summary>
        /// عنوان الإدارة
        /// </summary>
        /// <example>شمال القاهرة - مصر الجديدة</example>
        [MaxLength(500, ErrorMessage = "العنوان لا يتجاوز 500 حرف")]
        public string? Address { get; set; }

        /// <summary>
        /// معرف المحافظة التابعة لها
        /// </summary>
        /// <example>1</example>
        public int? GovernorateId { get; set; }

        /// <summary>
        /// هل الإدارة مفعلة؟
        /// </summary>
        /// <example>true</example>
        public bool? IsActive { get; set; }
    }
}