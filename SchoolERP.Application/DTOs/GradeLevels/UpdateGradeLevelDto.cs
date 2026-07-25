using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.GradeLevels
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات الصف الدراسي (Update GradeLevel DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث الصف من العميل إلى الخادم
    /// 📦  الاستخدام: في GradeLevelsController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateGradeLevelDto
    {
        /// <summary>
        /// اسم الصف الدراسي
        /// </summary>
        /// <example>الصف الأول الثانوي</example>
        [DisplayName("اسم الصف")]
        [MaxLength(50, ErrorMessage = "اسم الصف لا يتجاوز 50 حرف")]
        public string? GradeName { get; set; }

        /// <summary>
        /// رقم الصف
        /// </summary>
        /// <example>1</example>
        [DisplayName("رقم الصف")]
        [Range(1, 12, ErrorMessage = "رقم الصف يجب أن يكون بين 1 و 12")]
        public int? GradeNumber { get; set; }

        /// <summary>
        /// المرحلة الدراسية
        /// </summary>
        /// <example>1</example>
        [DisplayName("المرحلة الدراسية")]
        public GradeStage? GradeStage { get; set; }

        /// <summary>
        /// وصف الصف
        /// </summary>
        /// <example>المرحلة الثانوية - السنة الأولى</example>
        [DisplayName("الوصف")]
        [MaxLength(500, ErrorMessage = "الوصف لا يتجاوز 500 حرف")]
        public string? Description { get; set; }

        /// <summary>
        /// هل الصف مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool? IsActive { get; set; }
    }
}