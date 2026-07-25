using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.UserContacts
{
    public class UserContactDisplayInfo
    {
        [DisplayName("اسم المستخدم")]
        public string UserName { get; set; } = string.Empty;

        [DisplayName("نوع جهة الاتصال")]
        public string ContactTypeName { get; set; } = string.Empty;

        [DisplayName("قيمة جهة الاتصال")]
        public string ContactValue { get; set; } = string.Empty;

        [DisplayName("جهة اتصال أساسية")]
        public bool IsPrimary { get; set; }

        [DisplayName("تم التحقق")]
        public bool IsVerified { get; set; }

        [DisplayName("تاريخ الإنشاء")]
        public DateTime CreatedAt { get; set; }
    }
}