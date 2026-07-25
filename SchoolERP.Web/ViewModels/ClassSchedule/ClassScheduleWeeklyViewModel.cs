using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.ClassSchedules;
using System.ComponentModel;

namespace SchoolERP.Web.ViewModels.ClassSchedule
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج عرض الجدول الأسبوعي (Weekly ViewModel)
    /// 📌  الوظيفة: نقل بيانات الجدول الأسبوعي بين الـ Controller والـ View
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ClassScheduleWeeklyViewModel
    {
        /// <summary>
        /// اسم الفصل
        /// </summary>
        [DisplayName("الفصل")]
        public string ClassRoomName { get; set; } = string.Empty;

        /// <summary>
        /// اسم الصف الدراسي
        /// </summary>
        [DisplayName("الصف الدراسي")]
        public string GradeLevelName { get; set; } = string.Empty;

        /// <summary>
        /// الجدول الأسبوعي (اليوم -> قائمة الحصص)
        /// </summary>
        public Dictionary<string, IEnumerable<ClassScheduleDto>> WeeklySchedule { get; set; } = new();

        /// <summary>
        /// السنة الدراسية المحددة
        /// </summary>
        [DisplayName("السنة الدراسية")]
        public int? AcademicYearId { get; set; }

        /// <summary>
        /// قائمة السنوات الدراسية للفلترة
        /// </summary>
        public List<SelectListItem> AcademicYears { get; set; } = new();
    }
}