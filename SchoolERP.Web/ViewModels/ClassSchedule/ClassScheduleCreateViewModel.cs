using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.ClassSchedules;

namespace SchoolERP.Web.ViewModels.ClassSchedule
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء جدول حصص (Create ViewModel)
    /// 📌  الوظيفة: نقل بيانات الإنشاء بين الـ Controller والـ View
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ClassScheduleCreateViewModel
    {
        /// <summary>
        /// بيانات الحصة الجديدة
        /// </summary>
        public CreateClassScheduleDto ClassSchedule { get; set; } = new();

        /// <summary>
        /// قائمة السنوات الدراسية
        /// </summary>
        public List<SelectListItem> AcademicYears { get; set; } = new();

        /// <summary>
        /// قائمة الفصول الدراسية
        /// </summary>
        public List<SelectListItem> ClassRooms { get; set; } = new();

        /// <summary>
        /// قائمة المواد الدراسية
        /// </summary>
        public List<SelectListItem> Subjects { get; set; } = new();

        /// <summary>
        /// قائمة المعلمين
        /// </summary>
        public List<SelectListItem> Teachers { get; set; } = new();

        /// <summary>
        /// قائمة أيام الأسبوع
        /// </summary>
        public List<SelectListItem> DaysOfWeek { get; set; } = new();
    }
}