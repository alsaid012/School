using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.Students
{
    public class StudentGradesViewModel
    {
        [DisplayName("اسم الطالب")]
        public string StudentName { get; set; } = string.Empty;

        [DisplayName("كود الطالب")]
        public string StudentCode { get; set; } = string.Empty;

        [DisplayName("الدرجات")]
        public List<StudentGradeDto> Grades { get; set; } = new();

        [DisplayName("المتوسط")]
        public decimal AverageScore { get; set; }

        [DisplayName("عدد الامتحانات")]
        public int TotalExams { get; set; }

        [DisplayName("الامتحانات الناجحة")]
        public int PassedExams { get; set; }

        [DisplayName("الامتحانات الراسبة")]
        public int FailedExams { get; set; }
    }

    public class StudentGradeDto
    {
        [DisplayName("اسم الامتحان")]
        public string ExamName { get; set; } = string.Empty;

        [DisplayName("المادة")]
        public string SubjectName { get; set; } = string.Empty;

        [DisplayName("الدرجة")]
        public int Score { get; set; }

        [DisplayName("الدرجة النهائية")]
        public int MaxScore { get; set; }

        [DisplayName("النسبة المئوية")]
        public decimal Percentage { get; set; }

        [DisplayName("التقدير")]
        public string Grade { get; set; } = string.Empty;

        [DisplayName("تاريخ الامتحان")]
        public DateTime ExamDate { get; set; }

        [DisplayName("ناجح")]
        public bool IsPassed { get; set; }
    }
}