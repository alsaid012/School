using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.StudentAttendances;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.StudentAttendances
{
    public class StudentAttendanceCreateRangeViewModel
    {
        public int ClassRoomId { get; set; }

        [DisplayName("الفصل")]
        public List<SelectListItem> ClassRooms { get; set; } = new();

        [DisplayName("التاريخ")]
        public DateTime Date { get; set; } = DateTime.Today;

        [DisplayName("حالات الحضور")]
        public List<SelectListItem> StatusList { get; set; } = new();

        [DisplayName("سجلات الحضور")]
        public List<CreateStudentAttendanceDto> Attendances { get; set; } = new();
    }
}