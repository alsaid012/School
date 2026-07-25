using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.EmployeeAttendances
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✅  نموذج بيانات حضور الموظف (EmployeeAttendance DTO)
    /// 📌  الوظيفة: نقل بيانات حضور الموظف من الخادم إلى العميل
    /// 📦  الاستخدام: في EmployeeAttendancesController (GET endpoints)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class EmployeeAttendanceDto
    {
        /// <summary>
        /// معرف سجل الحضور (Primary Key)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الحضور")]
        public int Id { get; set; }

        /// <summary>
        /// معرف الموظف
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الموظف")]
        public int EmployeeId { get; set; }

        /// <summary>
        /// كود الموظف
        /// </summary>
        /// <example>EMP-2024-001</example>
        [DisplayName("كود الموظف")]
        public string? EmployeeCode { get; set; }

        /// <summary>
        /// اسم الموظف
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("اسم الموظف")]
        public string? EmployeeName { get; set; }

        /// <summary>
        /// المسمى الوظيفي
        /// </summary>
        /// <example>مدير شؤون الطلاب</example>
        [DisplayName("المسمى الوظيفي")]
        public string? JobTitle { get; set; }

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
        /// اسم المدرسة
        /// </summary>
        /// <example>مدرسة النصر الثانوية</example>
        [DisplayName("المدرسة")]
        public string? SchoolName { get; set; }

        /// <summary>
        /// القسم التابع له الموظف
        /// </summary>
        /// <example>شؤون الطلاب</example>
        [DisplayName("القسم")]
        public string? Department { get; set; }

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