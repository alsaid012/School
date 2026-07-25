using SchoolERP.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.UserContacts
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة لجهات الاتصال (UserContact Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات جهات الاتصال للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي عمليات الربط مع جداول أخرى
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UserContactLookupDto
    {
        /// <summary>
        /// معرف جهة الاتصال
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف جهة الاتصال")]
        public int Id { get; set; }

        public int UserId { get; set; }
        public ContactType ContactType { get; set; }


        /// <summary>
        /// اسم المستخدم
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("اسم المستخدم")]
        public string? UserName { get; set; }

        /// <summary>
        /// نوع جهة الاتصال
        /// </summary>
        /// <example>هاتف</example>
        [DisplayName("نوع جهة الاتصال")]
        public string ContactTypeName { get; set; } = string.Empty;

        /// <summary>
        /// قيمة جهة الاتصال
        /// </summary>
        /// <example>01001234567</example>
        [DisplayName("قيمة جهة الاتصال")]
        public string ContactValue { get; set; } = string.Empty;

        /// <summary>
        /// هل هي جهة الاتصال الأساسية؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("جهة اتصال أساسية")]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// هل تم التحقق من جهة الاتصال؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("تم التحقق")]
        public bool IsVerified { get; set; }

        /// <summary>
        /// هل جهة الاتصال مفعلة؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}