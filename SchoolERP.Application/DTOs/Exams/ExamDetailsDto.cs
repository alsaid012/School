using SchoolERP.Application.DTOs.ExamResults;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Exams
{
    public class ExamDetailsDto : ExamDto
    {
        [DisplayName("نتائج الامتحان")]
        public List<ExamResultDto> Results { get; set; } = new();

        [DisplayName("إحصائيات الامتحان")]
        public ExamStatisticsDto? Statistics { get; set; }
    }
}