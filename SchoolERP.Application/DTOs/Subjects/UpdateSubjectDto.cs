using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Subjects
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات المادة الدراسية (Update Subject DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث المادة من العميل إلى الخادم
    /// 📦  الاستخدام: في SubjectsController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateSubjectDto
    {
        /// <summary>
        /// اسم المادة
        /// </summary>
        /// <example>اللغة العربية</example>
        [DisplayName("اسم المادة")]
        [MaxLength(100, ErrorMessage = "اسم المادة لا يتجاوز 100 حرف")]
        public string? SubjectName { get; set; }

        /// <summary>
        /// كود المادة
        /// </summary>
        /// <example>SUB-AR-001</example>
        [DisplayName("كود المادة")]
        [MaxLength(20, ErrorMessage = "كود المادة لا يتجاوز 20 حرف")]
        public string? SubjectCode { get; set; }

        /// <summary>
        /// عدد الحصص الأسبوعية
        /// </summary>
        /// <example>4</example>
        [DisplayName("عدد الحصص الأسبوعية")]
        [Range(1, 10, ErrorMessage = "عدد الحصص يجب أن يكون بين 1 و 10")]
        public int? WeeklyHours { get; set; }

        /// <summary>
        /// هل المادة إجبارية؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مادة إجبارية")]
        public bool IsRequired { get; set; }

        /// <summary>
        /// وصف المادة
        /// </summary>
        /// <example>مادة اللغة العربية - النحو والصرف</example>
        [DisplayName("الوصف")]
        [MaxLength(500, ErrorMessage = "الوصف لا يتجاوز 500 حرف")]
        public string? Description { get; set; }

        /// <summary>
        /// معرف الصف الدراسي
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الصف")]
        public int? GradeLevelId { get; set; }

        /// <summary>
        /// هل المادة مفعلة؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool? IsActive { get; set; }
    }
}