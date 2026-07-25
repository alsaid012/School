using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.ClassSchedules
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🔍  نموذج فلترة جدول الحصص (ClassScheduleFilterDto)
    /// 📌  الوظيفة: تصفية الحصص حسب معايير متعددة
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ClassScheduleFilterDto
    {
        /// <summary>
        /// معرف السنة الدراسية
        /// </summary>
        [DisplayName("السنة الدراسية")]
        public int? AcademicYearId { get; set; }

        /// <summary>
        /// معرف الفصل الدراسي
        /// </summary>
        [DisplayName("الفصل الدراسي")]
        public int? ClassRoomId { get; set; }

        /// <summary>
        /// معرف المعلم
        /// </summary>
        [DisplayName("المعلم")]
        public int? TeacherId { get; set; }

        /// <summary>
        /// معرف المادة
        /// </summary>
        [DisplayName("المادة")]
        public int? SubjectId { get; set; }

        /// <summary>
        /// اليوم
        /// </summary>
        [DisplayName("اليوم")]
        public DayOfWeek? DayOfWeek { get; set; }

        /// <summary>
        /// رقم الحصة
        /// </summary>
        [DisplayName("رقم الحصة")]
        public int? PeriodNumber { get; set; }

        /// <summary>
        /// هل الحصة مفعلة؟
        /// </summary>
        [DisplayName("مفعل")]
        public bool? IsActive { get; set; }

        /// <summary>
        /// من وقت البداية
        /// </summary>
        [DisplayName("من وقت")]
        public TimeSpan? StartTimeFrom { get; set; }

        /// <summary>
        /// إلى وقت البداية
        /// </summary>
        [DisplayName("إلى وقت")]
        public TimeSpan? StartTimeTo { get; set; }

        /// <summary>
        /// كلمة البحث
        /// </summary>
        [DisplayName("بحث")]
        public string? SearchTerm { get; set; }

        /// <summary>
        /// رقم الصفحة
        /// </summary>
        [DisplayName("رقم الصفحة")]
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// حجم الصفحة
        /// </summary>
        [DisplayName("حجم الصفحة")]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// ترتيب حسب
        /// </summary>
        [DisplayName("ترتيب حسب")]
        public string? SortBy { get; set; }

        /// <summary>
        /// اتجاه الترتيب
        /// </summary>
        [DisplayName("اتجاه الترتيب")]
        public string? SortDirection { get; set; } = "ASC";
    }
}