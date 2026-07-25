using System.ComponentModel;

namespace SchoolERP.Application.DTOs.UserContacts
{
    /// <summary>
    /// 🏆  نموذج ترتيب المستخدمين حسب عدد جهات الاتصال
    /// </summary>
    public class TopUserContactsDto
    {
        /// <summary>
        /// معرف المستخدم
        /// </summary>
        [DisplayName("معرف المستخدم")]
        public int UserId { get; set; }

        /// <summary>
        /// اسم المستخدم
        /// </summary>
        [DisplayName("اسم المستخدم")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// عدد جهات الاتصال
        /// </summary>
        [DisplayName("عدد جهات الاتصال")]
        public int ContactsCount { get; set; }

        /// <summary>
        /// عدد جهات الاتصال الأساسية
        /// </summary>
        [DisplayName("جهات الاتصال الأساسية")]
        public int PrimaryContactsCount { get; set; }

        /// <summary>
        /// عدد جهات الاتصال الموثقة
        /// </summary>
        [DisplayName("جهات الاتصال الموثقة")]
        public int VerifiedContactsCount { get; set; }
    }
}