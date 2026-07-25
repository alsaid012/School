using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.ExamResults
{
    public class ExamResultDisplayInfo
    {
        [DisplayName("اسم الطالب")]
        public string StudentName { get; set; } = string.Empty;

        [DisplayName("كود الطالب")]
        public string StudentCode { get; set; } = string.Empty;

        [DisplayName("اسم الامتحان")]
        public string ExamName { get; set; } = string.Empty;

        [DisplayName("المادة")]
        public string SubjectName { get; set; } = string.Empty;

        [DisplayName("الفصل")]
        public string ClassRoomName { get; set; } = string.Empty;

        [DisplayName("تاريخ الامتحان")]
        public DateTime ExamDate { get; set; }

        [DisplayName("الدرجة النهائية")]
        public int MaxScore { get; set; }

        [DisplayName("الدرجة الحالية")]
        public int CurrentScore { get; set; }

        [DisplayName("النسبة المئوية")]
        public decimal Percentage { get; set; }

        [DisplayName("التقدير")]
        public string Grade { get; set; } = string.Empty;

        [DisplayName("ناجح")]
        public bool IsPassed { get; set; }
    }
}