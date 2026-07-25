using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.UserContacts
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📞  نموذج بيانات جهة اتصال المستخدم (UserContact DTO)
    /// 📌  الوظيفة: نقل بيانات جهة اتصال المستخدم من الخادم إلى العميل
    /// 📦  الاستخدام: في UserContactsController (GET endpoints)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UserContactDto
    {
        /// <summary>
        /// معرف جهة الاتصال (Primary Key)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف جهة الاتصال")]
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
        /// نوع جهة الاتصال (هاتف، بريد، واتساب، فيسبوك)
        /// </summary>
        /// <example>Phone</example>
        [DisplayName("نوع جهة الاتصال")]
        public ContactType ContactType { get; set; }

        /// <summary>
        /// اسم نوع جهة الاتصال (نص مترجم)
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
        /// ملاحظات إضافية
        /// </summary>
        /// <example>رقم المنزل</example>
        [DisplayName("ملاحظات")]
        public string? Notes { get; set; }

        /// <summary>
        /// هل جهة الاتصال مفعلة؟
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