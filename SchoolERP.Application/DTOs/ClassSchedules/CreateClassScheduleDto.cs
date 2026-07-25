using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.ClassSchedules
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء جدول حصص جديد (Create ClassSchedule DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء الجدول من العميل إلى الخادم
    /// 📦  الاستخدام: في ClassSchedulesController (POST endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateClassScheduleDto
    {
        /// <summary>
        /// معرف العام الدراسي (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف العام الدراسي")]
        [Required(ErrorMessage = "معرف العام الدراسي مطلوب")]
        public int AcademicYearId { get; set; }

        /// <summary>
        /// معرف الفصل الدراسي (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الفصل")]
        [Required(ErrorMessage = "معرف الفصل مطلوب")]
        public int ClassRoomId { get; set; }

        /// <summary>
        /// معرف المادة (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المادة")]
        [Required(ErrorMessage = "معرف المادة مطلوب")]
        public int SubjectId { get; set; }

        /// <summary>
        /// معرف المعلم (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المعلم")]
        [Required(ErrorMessage = "معرف المعلم مطلوب")]
        public int TeacherId { get; set; }

        /// <summary>
        /// معرف الصف الدراسي
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الصف")]
        public int? GradeLevelId { get; set; }

        /// <summary>
        /// اليوم (مطلوب) (0=الأحد، 1=الإثنين، ...)
        /// </summary>
        /// <example>0</example>
        [DisplayName("اليوم")]
        [Required(ErrorMessage = "اليوم مطلوب")]
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// وقت البداية (مطلوب)
        /// </summary>
        /// <example>08:00</example>
        [DisplayName("وقت البداية")]
        [Required(ErrorMessage = "وقت البداية مطلوب")]
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// وقت النهاية (مطلوب)
        /// </summary>
        /// <example>08:45</example>
        [DisplayName("وقت النهاية")]
        [Required(ErrorMessage = "وقت النهاية مطلوب")]
        public TimeSpan EndTime { get; set; }

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
    }
}