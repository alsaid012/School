using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.UserContacts
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج إحصائيات جهات الاتصال (UserContact Statistics DTO)
    /// 📌  الوظيفة: نقل إحصائيات جهات الاتصال من الخادم إلى العميل
    /// 📦  الاستخدام: ضمن UserContactDetailsDto أو في لوحة تحكم جهات الاتصال
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UserContactStatisticsDto
    {
        /// <summary>
        /// إجمالي عدد جهات الاتصال
        /// </summary>
        /// <example>100</example>
        [DisplayName("إجمالي جهات الاتصال")]
        public int TotalContacts { get; set; }

        /// <summary>
        /// عدد جهات الاتصال من نوع هاتف
        /// </summary>
        /// <example>40</example>
        [DisplayName("عدد الهواتف")]
        public int PhoneContacts { get; set; }

        /// <summary>
        /// عدد جهات الاتصال من نوع بريد إلكتروني
        /// </summary>
        /// <example>30</example>
        [DisplayName("عدد البريد الإلكتروني")]
        public int EmailContacts { get; set; }

        /// <summary>
        /// عدد جهات الاتصال من نوع واتساب
        /// </summary>
        /// <example>20</example>
        [DisplayName("عدد واتساب")]
        public int WhatsAppContacts { get; set; }

        /// <summary>
        /// عدد جهات الاتصال من نوع فيسبوك
        /// </summary>
        /// <example>10</example>
        [DisplayName("عدد فيسبوك")]
        public int FacebookContacts { get; set; }

        /// <summary>
        /// عدد جهات الاتصال الأساسية
        /// </summary>
        /// <example>50</example>
        [DisplayName("جهات الاتصال الأساسية")]
        public int PrimaryContacts { get; set; }

        /// <summary>
        /// عدد جهات الاتصال الموثقة
        /// </summary>
        /// <example>80</example>
        [DisplayName("جهات الاتصال الموثقة")]
        public int VerifiedContacts { get; set; }

        /// <summary>
        /// عدد المستخدمين الذين لديهم جهات اتصال
        /// </summary>
        /// <example>30</example>
        [DisplayName("المستخدمين النشطين")]
        public int ActiveUsersWithContacts { get; set; }

        /// <summary>
        /// متوسط عدد جهات الاتصال لكل مستخدم
        /// </summary>
        /// <example>3.5</example>
        [DisplayName("متوسط جهات الاتصال لكل مستخدم")]
        public decimal AverageContactsPerUser { get; set; }

        /// <summary>
        /// توزيع جهات الاتصال حسب النوع
        /// </summary>
        [DisplayName("توزيع جهات الاتصال حسب النوع")]
        public Dictionary<string, int> ContactsByType { get; set; } = new();

        /// <summary>
        /// أكثر 5 مستخدمين لديهم جهات اتصال
        /// </summary>
        [DisplayName("أكثر المستخدمين جهات اتصال")]
        public List<TopUserContactsDto> TopUsersWithContacts { get; set; } = new();
    }
}