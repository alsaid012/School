using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.AcademicYears
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة للأعوام الدراسية (AcademicYear Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات الأعوام الدراسية للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي عمليات الربط مع جداول أخرى
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class AcademicYearLookupDto
    {
        /// <summary>
        /// معرف العام الدراسي
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف العام الدراسي")]
        public int Id { get; set; }

        public int SchoolId { get; set; }

        /// <summary>
        /// اسم العام الدراسي (المعروض للمستخدم)
        /// </summary>
        /// <example>2024-2025</example>
        [DisplayName("اسم العام الدراسي")]
        public string YearName { get; set; } = string.Empty;

        /// <summary>
        /// تاريخ بداية العام الدراسي
        /// </summary>
        /// <example>2024-09-01</example>
        [DisplayName("تاريخ البداية")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// تاريخ نهاية العام الدراسي
        /// </summary>
        /// <example>2025-06-30</example>
        [DisplayName("تاريخ النهاية")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// هل هذا هو العام الدراسي الحالي؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("عام دراسي حالي")]
        public bool IsCurrent { get; set; }

        /// <summary>
        /// اسم المدرسة
        /// </summary>
        /// <example>مدرسة النصر الثانوية</example>
        [DisplayName("المدرسة")]
        public string? SchoolName { get; set; }

        /// <summary>
        /// هل العام الدراسي مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}