using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.Users.Contacts
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📞  نموذج جهة اتصال المستخدم (Contact DTO)
    /// 📌  الوظيفة: نقل بيانات جهة اتصال المستخدم من الخادم إلى العميل
    /// 📦  الاستخدام: ضمن UserDetailsDto
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ContactDto
    {
        /// <summary>
        /// معرف جهة الاتصال
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// نوع جهة الاتصال (هاتف، بريد، واتساب، ...)
        /// </summary>
        /// <example>Phone</example>
        public ContactType ContactType { get; set; }

        /// <summary>
        /// قيمة جهة الاتصال
        /// </summary>
        /// <example>01001234567</example>
        public string ContactValue { get; set; } = string.Empty;

        /// <summary>
        /// هل هي جهة الاتصال الأساسية؟
        /// </summary>
        /// <example>true</example>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// هل تم التحقق من جهة الاتصال؟
        /// </summary>
        /// <example>true</example>
        public bool IsVerified { get; set; }

        /// <summary>
        /// ملاحظات إضافية
        /// </summary>
        /// <example>رقم المنزل</example>
        public string? Notes { get; set; }
    }
}