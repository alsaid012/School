using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.GradeLevels
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء صف دراسي جديد (Create GradeLevel DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء الصف من العميل إلى الخادم
    /// 📦  الاستخدام: في GradeLevelsController (POST endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateGradeLevelDto
    {
        public int Id { get; set; }
        /// <summary>
        /// اسم الصف الدراسي (مطلوب)
        /// </summary>
        /// <example>الصف الأول الثانوي</example>
        [DisplayName("اسم الصف")]
        [Required(ErrorMessage = "اسم الصف مطلوب")]
        [MaxLength(50, ErrorMessage = "اسم الصف لا يتجاوز 50 حرف")]
        public string GradeName { get; set; } = string.Empty;

        /// <summary>
        /// رقم الصف (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("رقم الصف")]
        [Required(ErrorMessage = "رقم الصف مطلوب")]
        [Range(1, 12, ErrorMessage = "رقم الصف يجب أن يكون بين 1 و 12")]
        public int GradeNumber { get; set; }

        /// <summary>
        /// المرحلة الدراسية (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("المرحلة الدراسية")]
        [Required(ErrorMessage = "المرحلة الدراسية مطلوبة")]
        public GradeStage GradeStage { get; set; }

        /// <summary>
        /// وصف الصف
        /// </summary>
        /// <example>المرحلة الثانوية - السنة الأولى</example>
        [DisplayName("الوصف")]
        [MaxLength(500, ErrorMessage = "الوصف لا يتجاوز 500 حرف")]
        public string? Description { get; set; }

        /// <summary>
        /// معرف المدرسة (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المدرسة")]
        [Required(ErrorMessage = "معرف المدرسة مطلوب")]
        public int SchoolId { get; set; }
    }
}