using SchoolERP.Application.DTOs.ClassSchedules;
using SchoolERP.Application.DTOs.Exams;
using SchoolERP.Application.DTOs.Students;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.ClassRooms
{
    public class ClassRoomDetailsDto : ClassRoomDto
    {
        [DisplayName("الطلاب")]
        public List<StudentDto> Students { get; set; } = new();

        [DisplayName("جدول الحصص")]
        public List<ClassScheduleDto> Schedules { get; set; } = new();

        [DisplayName("الامتحانات")]
        public List<ExamDto> Exams { get; set; } = new();

        [DisplayName("إحصائيات الفصل")]
        public ClassRoomStatisticsDto? Statistics { get; set; }
    }
}