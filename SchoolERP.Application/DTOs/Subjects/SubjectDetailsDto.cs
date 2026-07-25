using SchoolERP.Application.DTOs.Students;
using SchoolERP.Application.DTOs.Teachers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Subjects
{
    public class SubjectDetailsDto : SubjectDto
    {
        [DisplayName("المعلمون")]
        public List<TeacherDto> Teachers { get; set; } = new();

        [DisplayName("الطلاب")]
        public List<StudentDto> Students { get; set; } = new();

        [DisplayName("إحصائيات المادة")]
        public SubjectStatisticsDto? Statistics { get; set; }
    }
}