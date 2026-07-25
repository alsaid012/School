using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.AcademicYears
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات العام الدراسي (Update AcademicYear DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث العام الدراسي من العميل إلى الخادم
    /// 📦  الاستخدام: في AcademicYearsController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateAcademicYearDto
    {
        /// <summary>
        /// اسم العام الدراسي
        /// </summary>
        /// <example>2024-2025</example>
        [DisplayName("اسم العام الدراسي")]
        [MaxLength(20, ErrorMessage = "اسم العام الدراسي لا يتجاوز 20 حرف")]
        public string? YearName { get; set; }

        /// <summary>
        /// تاريخ بداية العام الدراسي
        /// </summary>
        /// <example>2024-09-01</example>
        [DisplayName("تاريخ البداية")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// تاريخ نهاية العام الدراسي
        /// </summary>
        /// <example>2025-06-30</example>
        [DisplayName("تاريخ النهاية")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// هل هذا هو العام الدراسي الحالي؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("عام دراسي حالي")]
        public bool IsCurrent { get; set; }

        /// <summary>
        /// هل العام الدراسي مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}