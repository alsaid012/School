using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.AcademicYears
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📆  نموذج بيانات العام الدراسي (AcademicYear DTO)
    /// 📌  الوظيفة: نقل بيانات العام الدراسي من الخادم إلى العميل
    /// 📦  الاستخدام: في AcademicYearsController (GET endpoints)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class AcademicYearDto
    {
        /// <summary>
        /// معرف العام الدراسي (Primary Key)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف العام الدراسي")]
        public int Id { get; set; }

        /// <summary>
        /// اسم العام الدراسي
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
        /// معرف المدرسة
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المدرسة")]
        public int SchoolId { get; set; }

        /// <summary>
        /// اسم المدرسة
        /// </summary>
        /// <example>مدرسة النصر الثانوية</example>
        [DisplayName("المدرسة")]
        public string? SchoolName { get; set; }

        /// <summary>
        /// عدد الطلاب المسجلين في هذا العام
        /// </summary>
        /// <example>500</example>
        [DisplayName("عدد الطلاب")]
        public int StudentsCount { get; set; }

        /// <summary>
        /// عدد الفصول الدراسية في هذا العام
        /// </summary>
        /// <example>25</example>
        [DisplayName("عدد الفصول")]
        public int ClassRoomsCount { get; set; }

        /// <summary>
        /// عدد المواد الدراسية في هذا العام
        /// </summary>
        /// <example>8</example>
        [DisplayName("عدد المواد")]
        public int SubjectsCount { get; set; }

        /// <summary>
        /// هل العام الدراسي مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }

        /// <summary>
        /// تاريخ الإنشاء
        /// </summary>
        /// <example>2024-01-01T12:00:00</example>
        [DisplayName("تاريخ الإنشاء")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// تاريخ آخر تحديث
        /// </summary>
        /// <example>2024-01-15T10:30:00</example>
        [DisplayName("تاريخ التحديث")]
        public DateTime? UpdatedAt { get; set; }
    }
}