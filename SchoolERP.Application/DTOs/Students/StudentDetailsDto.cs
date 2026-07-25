using SchoolERP.Application.DTOs.ExamResults;
using SchoolERP.Application.DTOs.StudentAttendances;
using SchoolERP.Application.DTOs.Users;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Students
{
    public class StudentDetailsDto : StudentDto
    {
        [DisplayName("بيانات المستخدم")]
        public UserDetailsDto? User { get; set; }

        [DisplayName("نتائج الامتحانات")]
        public List<ExamResultDto> ExamResults { get; set; } = new();

        [DisplayName("سجلات الحضور")]
        public List<StudentAttendanceDto> Attendances { get; set; } = new();

        [DisplayName("إحصائيات الطالب")]
        public StudentStatisticsDto? Statistics { get; set; }
    }
}