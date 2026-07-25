using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.UserRoles
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء دور مستخدم جديد (Create UserRole DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء دور المستخدم من العميل إلى الخادم
    /// 📦  الاستخدام: في UserRolesController (POST endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateUserRoleDto
    {
        /// <summary>
        /// معرف المستخدم (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المستخدم")]
        [Required(ErrorMessage = "معرف المستخدم مطلوب")]
        public int UserId { get; set; }

        /// <summary>
        /// نوع الدور (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("نوع الدور")]
        [Required(ErrorMessage = "نوع الدور مطلوب")]
        public UserType RoleType { get; set; }

        /// <summary>
        /// هل هو الدور الأساسي للمستخدم؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("دور أساسي")]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// تاريخ بدء الدور
        /// </summary>
        /// <example>2024-01-01</example>
        [DisplayName("تاريخ البداية")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// تاريخ انتهاء الدور
        /// </summary>
        /// <example>2024-12-31</example>
        [DisplayName("تاريخ النهاية")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// ملاحظات إضافية
        /// </summary>
        /// <example>دور مؤقت</example>
        [DisplayName("ملاحظات")]
        [MaxLength(500, ErrorMessage = "الملاحظات لا تتجاوز 500 حرف")]
        public string? Notes { get; set; }
    }
}