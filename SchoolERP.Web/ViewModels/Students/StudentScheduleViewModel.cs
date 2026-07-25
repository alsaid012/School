using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.Students
{
    public class StudentScheduleViewModel
    {
        [DisplayName("اسم الطالب")]
        public string StudentName { get; set; } = string.Empty;

        [DisplayName("الفصل")]
        public string ClassRoomName { get; set; } = string.Empty;

        [DisplayName("الصف الدراسي")]
        public string GradeLevelName { get; set; } = string.Empty;

        [DisplayName("السنة الدراسية")]
        public string AcademicYearName { get; set; } = string.Empty;

        [DisplayName("الحصص")]
        public List<StudentScheduleDto> Schedules { get; set; } = new();
    }

    public class StudentScheduleDto
    {
        [DisplayName("اليوم")]
        public DayOfWeek DayOfWeek { get; set; }

        [DisplayName("اليوم")]
        public string DayName { get; set; } = string.Empty;

        [DisplayName("وقت البداية")]
        public TimeSpan StartTime { get; set; }

        [DisplayName("وقت النهاية")]
        public TimeSpan EndTime { get; set; }

        [DisplayName("المادة")]
        public string SubjectName { get; set; } = string.Empty;

        [DisplayName("المعلم")]
        public string TeacherName { get; set; } = string.Empty;

        [DisplayName("رقم الحصة")]
        public int? PeriodNumber { get; set; }
    }
}