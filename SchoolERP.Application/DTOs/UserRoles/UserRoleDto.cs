using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.UserRoles
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🎭  نموذج بيانات دور المستخدم (UserRole DTO)
    /// 📌  الوظيفة: نقل بيانات دور المستخدم من الخادم إلى العميل
    /// 📦  الاستخدام: في UserRolesController (GET endpoints)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UserRoleDto
    {
        /// <summary>
        /// معرف دور المستخدم (Primary Key)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الدور")]
        public int Id { get; set; }

        /// <summary>
        /// معرف المستخدم
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المستخدم")]
        public int UserId { get; set; }

        /// <summary>
        /// اسم المستخدم
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("اسم المستخدم")]
        public string? UserName { get; set; }

        /// <summary>
        /// نوع الدور (طالب، معلم، موظف، مدير، أدمن)
        /// </summary>
        /// <example>Teacher</example>
        [DisplayName("نوع الدور")]
        public UserType RoleType { get; set; }

        /// <summary>
        /// اسم نوع الدور (نص مترجم)
        /// </summary>
        /// <example>معلم</example>
        [DisplayName("نوع الدور")]
        public string RoleTypeName { get; set; } = string.Empty;

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
        public string? Notes { get; set; }

        /// <summary>
        /// هل دور المستخدم مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }

        /// <summary>
        /// تاريخ الإنشاء
        /// </summary>
        /// <example>2024-01-01T12:00:00</example>
        [DisplayName("تاريخ الإنشاء")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// تاريخ آخر تحديث
        /// </summary>
        /// <example>2024-01-15T10:30:00</example>
        [DisplayName("تاريخ التحديث")]
        public DateTime? UpdatedAt { get; set; }
    }
}