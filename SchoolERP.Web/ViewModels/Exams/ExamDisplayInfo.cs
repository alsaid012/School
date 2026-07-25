using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.Exams
{
    public class ExamDisplayInfo
    {
        [DisplayName("اسم الامتحان")]
        public string ExamName { get; set; } = string.Empty;

        [DisplayName("نوع الامتحان")]
        public string ExamTypeName { get; set; } = string.Empty;

        [DisplayName("المادة")]
        public string SubjectName { get; set; } = string.Empty;

        [DisplayName("المعلم المشرف")]
        public string TeacherName { get; set; } = string.Empty;

        [DisplayName("الفصل")]
        public string ClassRoomName { get; set; } = string.Empty;

        [DisplayName("السنة الدراسية")]
        public string AcademicYearName { get; set; } = string.Empty;

        [DisplayName("عدد الطلاب")]
        public int StudentsCount { get; set; }

        [DisplayName("متوسط الدرجات")]
        public decimal? AverageScore { get; set; }
    }
}