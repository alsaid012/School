using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.Students
{
    public class StudentProfileViewModel
    {
        [DisplayName("معرف الطالب")]
        public int Id { get; set; }

        [DisplayName("كود الطالب")]
        public string StudentCode { get; set; } = string.Empty;

        [DisplayName("الاسم الكامل")]
        public string FullName { get; set; } = string.Empty;

        [DisplayName("البريد الإلكتروني")]
        public string? Email { get; set; }

        [DisplayName("اسم المستخدم")]
        public string? Username { get; set; }

        [DisplayName("تاريخ الميلاد")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("الجنس")]
        public string? Gender { get; set; }

        [DisplayName("العنوان")]
        public string? Address { get; set; }

        [DisplayName("رقم الهاتف")]
        public string? Phone { get; set; }

        [DisplayName("الرقم القومي")]
        public string? NationalId { get; set; }

        [DisplayName("السنة الدراسية")]
        public string? AcademicYear { get; set; }

        [DisplayName("الفصل")]
        public string? ClassRoomName { get; set; }

        [DisplayName("الصف الدراسي")]
        public string? GradeLevelName { get; set; }

        [DisplayName("تاريخ التسجيل")]
        public DateTime EnrollmentDate { get; set; }

        [DisplayName("تخرج")]
        public bool IsGraduated { get; set; }

        [DisplayName("اسم ولي الأمر")]
        public string? ParentName { get; set; }

        [DisplayName("هاتف ولي الأمر")]
        public string? ParentPhone { get; set; }

        [DisplayName("بريد ولي الأمر")]
        public string? ParentEmail { get; set; }
    }
}