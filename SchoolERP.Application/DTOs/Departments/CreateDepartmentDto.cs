using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Departments
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء إدارة تعليمية جديدة (Create Department DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء الإدارة من العميل إلى الخادم
    /// 📦  الاستخدام: في DepartmentsController (POST endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateDepartmentDto
    {
        /// <summary>
        /// اسم الإدارة التعليمية (مطلوب)
        /// </summary>
        /// <example>إدارة شمال القاهرة التعليمية</example>
        [Required(ErrorMessage = "اسم الإدارة مطلوب")]
        [MaxLength(100, ErrorMessage = "اسم الإدارة لا يتجاوز 100 حرف")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// كود الإدارة (مطلوب وفريد)
        /// </summary>
        /// <example>SH-NORTH-CAIRO</example>
        [Required(ErrorMessage = "كود الإدارة مطلوب")]
        [MaxLength(20, ErrorMessage = "كود الإدارة لا يتجاوز 20 حرف")]
        public string Code { get; set; } = string.Empty;

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
        /// معرف المحافظة التابعة لها (مطلوب)
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "معرف المحافظة مطلوب")]
        public int GovernorateId { get; set; }
    }
}