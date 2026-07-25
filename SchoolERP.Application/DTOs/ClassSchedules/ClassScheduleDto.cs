using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.ClassSchedules
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📅  نموذج بيانات جدول الحصص (ClassSchedule DTO)
    /// 📌  الوظيفة: نقل بيانات جدول الحصص من الخادم إلى العميل
    /// 📦  الاستخدام: في ClassSchedulesController (GET endpoints)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ClassScheduleDto
    {
        /// <summary>
        /// معرف الحصة (المفتاح الأساسي)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الحصة")]
        public int Id { get; set; }
        /// <summary>
        /// معرف العام الدراسي
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف العام الدراسي")]
        public int AcademicYearId { get; set; }

        /// <summary>
        /// اسم العام الدراسي
        /// </summary>
        /// <example>2024-2025</example>
        [DisplayName("العام الدراسي")]
        public string? AcademicYearName { get; set; }

        /// <summary>
        /// معرف الفصل الدراسي
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الفصل")]
        public int ClassRoomId { get; set; }

        /// <summary>
        /// اسم الفصل الدراسي
        /// </summary>
        /// <example>الصف الخامس - أ</example>
        [DisplayName("الفصل الدراسي")]
        public string ClassRoomName { get; set; } = string.Empty;


        /// <summary>
        /// معرف المادة
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المادة")]
        public int SubjectId { get; set; }

        /// <summary>
        /// اسم المادة
        /// </summary>
        /// <example>اللغة العربية</example>
        [DisplayName("المادة")]
        public string? SubjectName { get; set; }
        /// <summary>
        /// كود المادة
        /// </summary>
        /// <example>اللغة العربية</example>
        [DisplayName("كود المادة")]
        public string? SubjectCode { get; set; }

        /// <summary>
        /// معرف المعلم
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المعلم")]
        public int TeacherId { get; set; }

        /// <summary>
        /// اسم المعلم
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("المعلم")]
        public string? TeacherName { get; set; }
        /// <summary>
        /// كود المعلم
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("المعلم")]
        public string? TeacherCode { get; set; }

        /// <summary>
        /// معرف الصف الدراسي
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الصف")]
        public int? GradeLevelId { get; set; }

        /// <summary>
        /// اسم الصف الدراسي
        /// </summary>
        /// <example>الصف الأول الثانوي</example>
        [DisplayName("الصف الدراسي")]
        public string? GradeLevelName { get; set; }

        /// <summary>
        /// اليوم (0=الأحد، 1=الإثنين، ...)
        /// </summary>
        /// <example>0</example>
        [DisplayName("رقم اليوم")]
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// اسم اليوم
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
        /// ملاحظات إضافية
        /// </summary>
        /// <example>حصة النحو</example>
        [DisplayName("ملاحظات")]
        public string? Notes { get; set; }

        /// <summary>
        /// هل الجدول مفعل؟
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