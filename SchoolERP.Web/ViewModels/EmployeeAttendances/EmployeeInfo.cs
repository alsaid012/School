using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.EmployeeAttendances
{
    /// <summary>
    /// 📋 معلومات الموظف للعرض في الـ View
    /// </summary>
    public class EmployeeInfo
    {
        [DisplayName("معرف الموظف")]
        public int Id { get; set; }

        [DisplayName("اسم الموظف")]
        public string Name { get; set; } = string.Empty;

        [DisplayName("كود الموظف")]
        public string Code { get; set; } = string.Empty;

        [DisplayName("المسمى الوظيفي")]
        public string JobTitle { get; set; } = string.Empty;
    }
}