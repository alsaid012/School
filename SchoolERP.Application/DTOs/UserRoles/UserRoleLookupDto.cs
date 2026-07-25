using SchoolERP.Domain.Enums;
using System.ComponentModel;

namespace SchoolERP.Application.DTOs.UserRoles
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة لأدوار المستخدمين (UserRole Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات أدوار المستخدمين للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي عمليات الربط مع جداول أخرى
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UserRoleLookupDto
    {
        /// <summary>
        /// معرف دور المستخدم
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الدور")]
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserType RoleType { get; set; }


        /// <summary>
        /// اسم المستخدم
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("اسم المستخدم")]
        public string? UserName { get; set; }

        /// <summary>
        /// نوع الدور
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
        /// هل دور المستخدم مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}