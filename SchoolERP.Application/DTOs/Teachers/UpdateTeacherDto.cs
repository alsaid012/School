using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Teachers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات المعلم (Update Teacher DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث المعلم من العميل إلى الخادم
    /// 📦  الاستخدام: في TeachersController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateTeacherDto
    {
        /// <summary>
        /// المؤهل الدراسي
        /// </summary>
        /// <example>ليسانس آداب</example>
        [DisplayName("المؤهل الدراسي")]
        [MaxLength(200, ErrorMessage = "المؤهل لا يتجاوز 200 حرف")]
        public string? Qualification { get; set; }

        /// <summary>
        /// التخصص
        /// </summary>
        /// <example>اللغة العربية</example>
        [DisplayName("التخصص")]
        [MaxLength(200, ErrorMessage = "التخصص لا يتجاوز 200 حرف")]
        public string? Specialization { get; set; }

        /// <summary>
        /// الراتب
        /// </summary>
        /// <example>5000.00</example>
        [DisplayName("الراتب")]
        [Range(0, double.MaxValue, ErrorMessage = "الراتب يجب أن يكون قيمة موجبة")]
        public decimal? Salary { get; set; }

        /// <summary>
        /// هل هو معلم فصل (Homeroom Teacher)؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("معلم فصل")]
        public bool? IsHomeroomTeacher { get; set; }

        /// <summary>
        /// قائمة معرفات المواد التي يدرسها المعلم (سيتم استبدال القائمة الحالية)
        /// </summary>
        /// <example>[1, 2, 3]</example>
        [DisplayName("المواد")]
        public List<int>? SubjectIds { get; set; }

        /// <summary>
        /// هل المعلم مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool? IsActive { get; set; }
    }
}