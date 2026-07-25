using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.Home
{
    public class HomeDashboardViewModel
    {
        [DisplayName("إجمالي المستخدمين")]
        public int TotalUsers { get; set; }

        [DisplayName("إجمالي الطلاب")]
        public int TotalStudents { get; set; }

        [DisplayName("إجمالي المعلمين")]
        public int TotalTeachers { get; set; }

        [DisplayName("إجمالي الموظفين")]
        public int TotalEmployees { get; set; }

        [DisplayName("إجمالي المدارس")]
        public int TotalSchools { get; set; }

        [DisplayName("إجمالي السنوات الدراسية")]
        public int TotalAcademicYears { get; set; }

        [DisplayName("السنة الدراسية الحالية")]
        public string? CurrentAcademicYear { get; set; }

        [DisplayName("طلاب السنة الحالية")]
        public int CurrentYearStudents { get; set; }

        [DisplayName("المعلمين الجدد")]
        public int NewTeachers { get; set; }

        [DisplayName("الطلاب الجدد")]
        public int NewStudents { get; set; }

        [DisplayName("المستخدمين النشطين")]
        public int ActiveUsers { get; set; }

        [DisplayName("آخر المستخدمين")]
        public List<RecentUserDto> RecentUsers { get; set; } = new();

        [DisplayName("آخر الطلاب")]
        public List<RecentStudentDto> RecentStudents { get; set; } = new();
    }
}