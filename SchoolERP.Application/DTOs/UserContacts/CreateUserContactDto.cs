using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.UserContacts
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء جهة اتصال جديدة (Create UserContact DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء جهة الاتصال من العميل إلى الخادم
    /// 📦  الاستخدام: في UserContactsController (POST endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateUserContactDto
    {
        /// <summary>
        /// معرف المستخدم (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المستخدم")]
        [Required(ErrorMessage = "معرف المستخدم مطلوب")]
        public int UserId { get; set; }

        /// <summary>
        /// نوع جهة الاتصال (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("نوع جهة الاتصال")]
        [Required(ErrorMessage = "نوع جهة الاتصال مطلوب")]
        public ContactType ContactType { get; set; }

        /// <summary>
        /// قيمة جهة الاتصال (مطلوبة)
        /// </summary>
        /// <example>01001234567</example>
        [DisplayName("قيمة جهة الاتصال")]
        [Required(ErrorMessage = "قيمة جهة الاتصال مطلوبة")]
        [MaxLength(200, ErrorMessage = "قيمة جهة الاتصال لا تتجاوز 200 حرف")]
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
        [MaxLength(500, ErrorMessage = "الملاحظات لا تتجاوز 500 حرف")]
        public string? Notes { get; set; }
    }
}