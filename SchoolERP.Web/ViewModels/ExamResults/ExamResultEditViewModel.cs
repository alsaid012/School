using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.ExamResults;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.ExamResults
{
    public class ExamResultEditViewModel
    {
        public int Id { get; set; }

        public UpdateExamResultDto ExamResult { get; set; } = new();

        public ExamResultDisplayInfo DisplayInfo { get; set; } = new();
    }
}