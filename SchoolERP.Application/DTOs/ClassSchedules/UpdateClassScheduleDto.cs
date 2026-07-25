using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.ClassSchedules
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات جدول الحصص (Update ClassSchedule DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث الجدول من العميل إلى الخادم
    /// 📦  الاستخدام: في ClassSchedulesController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateClassScheduleDto
    {
        /// <summary>
        /// معرف العام الدراسي
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف العام الدراسي")]
        public int? AcademicYearId { get; set; }

        /// <summary>
        /// معرف الفصل الدراسي
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الفصل")]
        public int? ClassRoomId { get; set; }

        /// <summary>
        /// معرف المادة
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المادة")]
        public int? SubjectId { get; set; }

        /// <summary>
        /// معرف المعلم
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المعلم")]
        public int? TeacherId { get; set; }

        /// <summary>
        /// معرف الصف الدراسي
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الصف")]
        public int? GradeLevelId { get; set; }

        /// <summary>
        /// اليوم (0=الأحد، 1=الإثنين، ...)
        /// </summary>
        /// <example>0</example>
        [DisplayName("اليوم")]
        public DayOfWeek? DayOfWeek { get; set; }

        /// <summary>
        /// وقت البداية
        /// </summary>
        /// <example>08:00</example>
        [DisplayName("وقت البداية")]
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// وقت النهاية
        /// </summary>
        /// <example>08:45</example>
        [DisplayName("وقت النهاية")]
        public TimeSpan? EndTime { get; set; }

        /// <summary>
        /// رقم الحصة
        /// </summary>
        /// <example>1</example>
        [DisplayName("رقم الحصة")]
        [Range(1, 10, ErrorMessage = "رقم الحصة يجب أن يكون بين 1 و 10")]
        public int? PeriodNumber { get; set; }

        /// <summary>
        /// ملاحظات إضافية
        /// </summary>
        /// <example>حصة النحو</example>
        [DisplayName("ملاحظات")]
        [MaxLength(500, ErrorMessage = "الملاحظات لا تتجاوز 500 حرف")]
        public string? Notes { get; set; }

        /// <summary>
        /// هل الجدول مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}