using SchoolERP.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.EmployeeAttendances
{
    /// <summary>
    /// 🔍  نموذج فلترة حضور الموظفين (EmployeeAttendanceFilterDto)
    /// </summary>
    public class EmployeeAttendanceFilterDto
    {
        [DisplayName("معرف الموظف")]
        public int? EmployeeId { get; set; }

        [DisplayName("القسم")]
        public string? Department { get; set; }

        [DisplayName("حالة الحضور")]
        public AttendanceStatus? Status { get; set; }

        [DisplayName("من تاريخ")]
        public DateTime? FromDate { get; set; }

        [DisplayName("إلى تاريخ")]
        public DateTime? ToDate { get; set; }

        [DisplayName("مفعل")]
        public bool? IsActive { get; set; }

        [DisplayName("رقم الصفحة")]
        public int PageNumber { get; set; } = 1;

        [DisplayName("حجم الصفحة")]
        public int PageSize { get; set; } = 10;
    }
}