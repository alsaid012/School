using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.StudentAttendances;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.StudentAttendances
{
    public class StudentAttendanceEditViewModel
    {
        public int Id { get; set; }

        public UpdateStudentAttendanceDto Attendance { get; set; } = new();

        public StudentAttendanceDisplayInfo DisplayInfo { get; set; } = new();
    }

    public class StudentAttendanceDisplayInfo
    {
        [DisplayName("اسم الطالب")]
        public string StudentName { get; set; } = string.Empty;

        [DisplayName("كود الطالب")]
        public string StudentCode { get; set; } = string.Empty;

        [DisplayName("الفصل")]
        public string ClassRoomName { get; set; } = string.Empty;

        [DisplayName("الصف")]
        public string GradeLevelName { get; set; } = string.Empty;

        [DisplayName("تاريخ الحضور")]
        public DateTime AttendanceDate { get; set; }

        [DisplayName("الحالة الحالية")]
        public string CurrentStatus { get; set; } = string.Empty;
    }
}