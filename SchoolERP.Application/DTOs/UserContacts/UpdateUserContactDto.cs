using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.UserContacts
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات جهة الاتصال (Update UserContact DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث جهة الاتصال من العميل إلى الخادم
    /// 📦  الاستخدام: في UserContactsController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateUserContactDto
    {
        public int? Id { get; set; }


        /// <summary>
        /// نوع جهة الاتصال
        /// </summary>
        /// <example>1</example>
        [DisplayName("نوع جهة الاتصال")]
        public ContactType? ContactType { get; set; } 

        /// <summary>
        /// قيمة جهة الاتصال
        /// </summary>
        /// <example>01001234567</example>
        [DisplayName("قيمة جهة الاتصال")]
        [MaxLength(200, ErrorMessage = "قيمة جهة الاتصال لا تتجاوز 200 حرف")]
        public string? ContactValue { get; set; }

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
        [MaxLength(500, ErrorMessage = "الملاحظات لا تتجاوز 500 حرف")]
        public string? Notes { get; set; }

        /// <summary>
        /// هل جهة الاتصال مفعلة؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}