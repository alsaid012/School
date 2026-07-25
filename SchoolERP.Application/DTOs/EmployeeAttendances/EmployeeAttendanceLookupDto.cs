using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.EmployeeAttendances
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة لحضور الموظفين (EmployeeAttendance Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات الحضور للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي عمليات الربط مع جداول أخرى
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class EmployeeAttendanceLookupDto
    {
        /// <summary>
        /// معرف سجل الحضور
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الحضور")]
        public int Id { get; set; }

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
        /// حالة الحضور
        /// </summary>
        /// <example>حاضر</example>
        [DisplayName("حالة الحضور")]
        public string StatusName { get; set; } = string.Empty;

        /// <summary>
        /// اسم المدرسة
        /// </summary>
        /// <example>مدرسة النصر الثانوية</example>
        [DisplayName("المدرسة")]
        public string? SchoolName { get; set; }

        /// <summary>
        /// هل الحضور مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}