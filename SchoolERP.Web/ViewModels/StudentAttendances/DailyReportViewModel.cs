using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.StudentAttendances
{
    public class DailyReportViewModel
    {
        [DisplayName("المدرسة")]
        public int? SelectedSchoolId { get; set; }

        [DisplayName("المدارس")]
        public List<SelectListItem> Schools { get; set; } = new();

        [DisplayName("التاريخ")]
        public DateTime SelectedDate { get; set; } = DateTime.Today;

        [DisplayName("التقرير")]
        public object? Report { get; set; }
    }
}