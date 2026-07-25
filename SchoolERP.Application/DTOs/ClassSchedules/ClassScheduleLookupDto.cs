using System.ComponentModel;

namespace SchoolERP.Application.DTOs.ClassSchedules
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة لجداول الحصص (ClassSchedule Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات جداول الحصص للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي عمليات الربط مع جداول أخرى
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ClassScheduleLookupDto
    {
        /// <summary>
        /// معرف الجدول
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الجدول")]
        public int Id { get; set; }
        [DisplayName("معرف الفصل")]
        public int ClassRoomId { get; set; }
        [DisplayName("معرف المادة")]
        public int SubjectId { get; set; }
        [DisplayName("معرف المعلم")]
        public int TeacherId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// اسم الفصل
        /// </summary>
        /// <example>1/أ</example>
        [DisplayName("الفصل")]
        public string? ClassRoomName { get; set; }

        /// <summary>
        /// اسم المادة
        /// </summary>
        /// <example>اللغة العربية</example>
        [DisplayName("المادة")]
        public string? SubjectName { get; set; }

        /// <summary>
        /// اسم المعلم
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("المعلم")]
        public string? TeacherName { get; set; }

        /// <summary>
        /// اليوم
        /// </summary>
        /// <example>الأحد</example>
        [DisplayName("اليوم")]
        public string DayName { get; set; } = string.Empty;

        /// <summary>
        /// وقت البداية
        /// </summary>
        /// <example>08:00</example>
        [DisplayName("وقت البداية")]
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// وقت النهاية
        /// </summary>
        /// <example>08:45</example>
        [DisplayName("وقت النهاية")]
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// رقم الحصة
        /// </summary>
        /// <example>1</example>
        [DisplayName("رقم الحصة")]
        public int? PeriodNumber { get; set; }

        /// <summary>
        /// هل الجدول مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}