using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.ClassSchedules;

namespace SchoolERP.Web.ViewModels.ClassSchedule
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تعديل جدول حصص (Edit ViewModel)
    /// 📌  الوظيفة: نقل بيانات التعديل بين الـ Controller والـ View
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ClassScheduleEditViewModel
    {
        /// <summary>
        /// معرف الجدول
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// بيانات الحصة للتعديل
        /// </summary>
        public UpdateClassScheduleDto ClassSchedule { get; set; } = new();

        /// <summary>
        /// معلومات للعرض فقط
        /// </summary>
        public ClassScheduleDisplayInfo DisplayInfo { get; set; } = new();

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

    /// <summary>
    /// 📋 معلومات للعرض فقط (قراءة فقط)
    /// </summary>
    public class ClassScheduleDisplayInfo
    {
        [DisplayName("المعلم")]
        public string TeacherName { get; set; } = string.Empty;

        [DisplayName("المادة")]
        public string SubjectName { get; set; } = string.Empty;

        [DisplayName("الفصل")]
        public string ClassRoomName { get; set; } = string.Empty;

        [DisplayName("السنة الدراسية")]
        public string AcademicYearName { get; set; } = string.Empty;

        [DisplayName("الصف الدراسي")]
        public string GradeLevelName { get; set; } = string.Empty;
    }
}