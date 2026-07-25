using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.UserRoles
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات دور المستخدم (Update UserRole DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث دور المستخدم من العميل إلى الخادم
    /// 📦  الاستخدام: في UserRolesController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateUserRoleDto
    {
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

        /// <summary>
        /// هل دور المستخدم مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}