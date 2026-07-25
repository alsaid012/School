using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.EmployeeAttendances
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات حضور الموظف (Update EmployeeAttendance DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث الحضور من العميل إلى الخادم
    /// 📦  الاستخدام: في EmployeeAttendancesController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateEmployeeAttendanceDto
    {
        /// <summary>
        /// وقت الدخول
        /// </summary>
        /// <example>2024-01-15T08:00:00</example>
        [DisplayName("وقت الدخول")]
        public DateTime? CheckInTime { get; set; }

        /// <summary>
        /// وقت الخروج
        /// </summary>
        /// <example>2024-01-15T14:30:00</example>
        [DisplayName("وقت الخروج")]
        public DateTime? CheckOutTime { get; set; }

        /// <summary>
        /// حالة الحضور
        /// </summary>
        /// <example>1</example>
        [DisplayName("حالة الحضور")]
        public AttendanceStatus? Status { get; set; }

        /// <summary>
        /// عدد دقائق التأخير
        /// </summary>
        /// <example>10</example>
        [DisplayName("دقائق التأخير")]
        [Range(0, 1000, ErrorMessage = "دقائق التأخير يجب أن تكون بين 0 و 1000")]
        public int? DelayMinutes { get; set; }

        /// <summary>
        /// ملاحظات إضافية
        /// </summary>
        /// <example>تأخر بسبب مواصلات</example>
        [DisplayName("ملاحظات")]
        [MaxLength(500, ErrorMessage = "الملاحظات لا تتجاوز 500 حرف")]
        public string? Notes { get; set; }

        /// <summary>
        /// هل الحضور مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}