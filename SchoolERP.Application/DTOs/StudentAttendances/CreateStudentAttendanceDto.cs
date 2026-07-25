using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.StudentAttendances
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء سجل حضور طالب جديد (Create StudentAttendance DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء الحضور من العميل إلى الخادم
    /// 📦  الاستخدام: في StudentAttendancesController (POST endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateStudentAttendanceDto
    {
        /// <summary>
        /// معرف الطالب (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الطالب")]
        [Required(ErrorMessage = "معرف الطالب مطلوب")]
        public int StudentId { get; set; }

        /// <summary>
        /// تاريخ الحضور (مطلوب)
        /// </summary>
        /// <example>2024-01-15</example>
        [DisplayName("تاريخ الحضور")]
        [Required(ErrorMessage = "تاريخ الحضور مطلوب")]
        public DateTime AttendanceDate { get; set; }

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
        /// حالة الحضور (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("حالة الحضور")]
        [Required(ErrorMessage = "حالة الحضور مطلوبة")]
        public AttendanceStatus Status { get; set; }

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
    }
}