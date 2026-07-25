using SchoolERP.Application.DTOs.AcademicYears;
using SchoolERP.Application.DTOs.ClassSchedules;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Web.ViewModels.ClassSchedule
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج عرض قائمة جدول الحصص (Index ViewModel)
    /// 📌  الوظيفة: نقل البيانات بين الـ Controller والـ View
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ClassScheduleIndexViewModel
    {
        /// <summary>
        /// قائمة الحصص
        /// </summary>
        [DisplayName("قائمة الحصص")]
        public IEnumerable<ClassScheduleDto> Schedules { get; set; } = new List<ClassScheduleDto>();

        /// <summary>
        /// قائمة السنوات الدراسية للفلترة
        /// </summary>
        [DisplayName("السنوات الدراسية")]
        public IEnumerable<AcademicYearDto> AcademicYears { get; set; } = new List<AcademicYearDto>();

        /// <summary>
        /// السنة الدراسية المحددة للفلترة
        /// </summary>
        [DisplayName("السنة الدراسية")]
        public int? SelectedAcademicYearId { get; set; }

        /// <summary>
        /// معايير الفلترة
        /// </summary>
        public ClassScheduleFilterDto Filter { get; set; } = new();
    }
}