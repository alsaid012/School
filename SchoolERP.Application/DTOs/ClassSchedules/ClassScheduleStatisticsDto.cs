using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.ClassSchedules
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج إحصائيات جدول الحصص (ClassScheduleStatisticsDto)
    /// 📌  الوظيفة: عرض إحصائيات الجدول الأسبوعي
    /// 📦  الاستخدام: في لوحة التحكم أو تقارير الجدول
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ClassScheduleStatisticsDto
    {
        /// <summary>
        /// عدد الحصص الأسبوعية الإجمالي
        /// </summary>
        /// <example>30</example>
        [DisplayName("إجمالي الحصص الأسبوعية")]
        public int TotalWeeklyHours { get; set; }

        /// <summary>
        /// عدد المواد التي تدرس في هذا الجدول
        /// </summary>
        /// <example>8</example>
        [DisplayName("عدد المواد")]
        public int TotalSubjects { get; set; }

        /// <summary>
        /// عدد المعلمين الذين يدرسون في هذا الجدول
        /// </summary>
        /// <example>10</example>
        [DisplayName("عدد المعلمين")]
        public int TotalTeachers { get; set; }

        /// <summary>
        /// عدد الحصص المفعلة
        /// </summary>
        /// <example>28</example>
        [DisplayName("الحصص المفعلة")]
        public int ActiveSchedules { get; set; }

        /// <summary>
        /// عدد الحصص غير المفعلة
        /// </summary>
        /// <example>2</example>
        [DisplayName("الحصص غير المفعلة")]
        public int InactiveSchedules { get; set; }

        /// <summary>
        /// توزيع الحصص حسب الأيام (الأحد -> 6 حصص، الإثنين -> 6 حصص، ...)
        /// </summary>
        [DisplayName("توزيع الحصص حسب الأيام")]
        public Dictionary<string, int> DailyDistribution { get; set; } = new();

        /// <summary>
        /// توزيع الحصص حسب المواد
        /// </summary>
        [DisplayName("توزيع الحصص حسب المواد")]
        public Dictionary<string, int> SubjectDistribution { get; set; } = new();

        /// <summary>
        /// أكثر المعلمين حصصاً
        /// </summary>
        [DisplayName("أكثر المعلمين حصصاً")]
        public string? MostBusyTeacher { get; set; }

        /// <summary>
        /// عدد حصص أكثر المعلمين
        /// </summary>
        [DisplayName("عدد حصص أكثر المعلمين")]
        public int MostBusyTeacherHours { get; set; }

        /// <summary>
        /// أقل المعلمين حصصاً
        /// </summary>
        [DisplayName("أقل المعلمين حصصاً")]
        public string? LeastBusyTeacher { get; set; }

        /// <summary>
        /// عدد حصص أقل المعلمين
        /// </summary>
        [DisplayName("عدد حصص أقل المعلمين")]
        public int LeastBusyTeacherHours { get; set; }

        /// <summary>
        /// نسبة الحصص المفعلة من الإجمالي
        /// </summary>
        [DisplayName("نسبة التفعيل")]
        public decimal ActivePercentage =>
            TotalWeeklyHours > 0 ? Math.Round((decimal)ActiveSchedules / TotalWeeklyHours * 100, 2) : 0;
    }
}