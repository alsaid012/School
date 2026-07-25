using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.StudentAttendances
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة لحضور الطلاب (StudentAttendance Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات الحضور للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي عمليات الربط مع جداول أخرى
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class StudentAttendanceLookupDto
    {
        /// <summary>
        /// معرف سجل الحضور
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الحضور")]
        public int Id { get; set; }

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
        /// حالة الحضور
        /// </summary>
        /// <example>حاضر</example>
        [DisplayName("حالة الحضور")]
        public string StatusName { get; set; } = string.Empty;

        /// <summary>
        /// اسم الفصل
        /// </summary>
        /// <example>1/أ</example>
        [DisplayName("الفصل")]
        public string? ClassRoomName { get; set; }

        /// <summary>
        /// هل الحضور مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}