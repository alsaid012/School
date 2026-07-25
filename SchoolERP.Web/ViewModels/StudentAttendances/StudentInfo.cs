using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.StudentAttendances
{
    /// <summary>
    /// 📋 معلومات الطالب للعرض في الـ View
    /// </summary>
    public class StudentInfo
    {
        [DisplayName("معرف الطالب")]
        public int Id { get; set; }

        [DisplayName("اسم الطالب")]
        public string Name { get; set; } = string.Empty;

        [DisplayName("كود الطالب")]
        public string Code { get; set; } = string.Empty;
    }
}