using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.StudentAttendances
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✅  نموذج بيانات حضور الطالب (StudentAttendance DTO)
    /// 📌  الوظيفة: نقل بيانات حضور الطالب من الخادم إلى العميل
    /// 📦  الاستخدام: في StudentAttendancesController (GET endpoints)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class StudentAttendanceDto
    {
        /// <summary>
        /// معرف سجل الحضور (Primary Key)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الحضور")]
        public int Id { get; set; }

        /// <summary>
        /// معرف الطالب
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الطالب")]
        public int StudentId { get; set; }

        /// <summary>
        /// كود الطالب
        /// </summary>
        /// <example>STU-2024-001</example>
        [DisplayName("كود الطالب")]
        public string? StudentCode { get; set; }

        /// <summary>
        /// اسم الطالب
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("اسم الطالب")]
        public string? StudentName { get; set; }

        /// <summary>
        /// تاريخ الحضور
        /// </summary>
        /// <example>2024-01-15</example>
        [DisplayName("تاريخ الحضور")]
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
        /// حالة الحضور (حاضر، غائب، متأخر، معذور)
        /// </summary>
        /// <example>Present</example>
        [DisplayName("حالة الحضور")]
        public AttendanceStatus Status { get; set; }

        /// <summary>
        /// اسم حالة الحضور (نص مترجم)
        /// </summary>
        /// <example>حاضر</example>
        [DisplayName("حالة الحضور")]
        public string StatusName { get; set; } = string.Empty;

        /// <summary>
        /// عدد دقائق التأخير
        /// </summary>
        /// <example>10</example>
        [DisplayName("دقائق التأخير")]
        public int? DelayMinutes { get; set; }

        /// <summary>
        /// ملاحظات إضافية
        /// </summary>
        /// <example>تأخر بسبب مواصلات</example>
        [DisplayName("ملاحظات")]
        public string? Notes { get; set; }

        /// <summary>
        /// اسم الفصل الدراسي
        /// </summary>
        /// <example>1/أ</example>
        [DisplayName("الفصل")]
        public string? ClassRoomName { get; set; }

        /// <summary>
        /// اسم الصف الدراسي
        /// </summary>
        /// <example>الصف الأول الثانوي</example>
        [DisplayName("الصف الدراسي")]
        public string? GradeLevelName { get; set; }

        /// <summary>
        /// هل الحضور مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }

        /// <summary>
        /// تاريخ الإنشاء
        /// </summary>
        /// <example>2024-01-01T12:00:00</example>
        [DisplayName("تاريخ الإنشاء")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// تاريخ آخر تحديث
        /// </summary>
        /// <example>2024-01-15T10:30:00</example>
        [DisplayName("تاريخ التحديث")]
        public DateTime? UpdatedAt { get; set; }
    }
}