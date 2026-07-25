using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.AcademicYears
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء عام دراسي جديد (Create AcademicYear DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء العام الدراسي من العميل إلى الخادم
    /// 📦  الاستخدام: في AcademicYearsController (POST endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateAcademicYearDto
    {
        /// <summary>
        /// اسم العام الدراسي (مطلوب)
        /// </summary>
        /// <example>2024-2025</example>
        [DisplayName("اسم العام الدراسي")]
        [Required(ErrorMessage = "اسم العام الدراسي مطلوب")]
        [MaxLength(20, ErrorMessage = "اسم العام الدراسي لا يتجاوز 20 حرف")]
        public string YearName { get; set; } = string.Empty;

        /// <summary>
        /// تاريخ بداية العام الدراسي (مطلوب)
        /// </summary>
        /// <example>2024-09-01</example>
        [DisplayName("تاريخ البداية")]
        [Required(ErrorMessage = "تاريخ البداية مطلوب")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// تاريخ نهاية العام الدراسي (مطلوب)
        /// </summary>
        /// <example>2025-06-30</example>
        [DisplayName("تاريخ النهاية")]
        [Required(ErrorMessage = "تاريخ النهاية مطلوب")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// هل هذا هو العام الدراسي الحالي؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("عام دراسي حالي")]
        public bool IsCurrent { get; set; } = false;

        /// <summary>
        /// معرف المدرسة (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المدرسة")]
        [Required(ErrorMessage = "معرف المدرسة مطلوب")]
        public int SchoolId { get; set; }
    }
}