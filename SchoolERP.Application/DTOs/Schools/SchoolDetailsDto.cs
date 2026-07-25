using SchoolERP.Application.DTOs.AcademicYears;
using SchoolERP.Application.DTOs.Departments;
using SchoolERP.Application.DTOs.GradeLevels;

namespace SchoolERP.Application.DTOs.Schools
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏫  نموذج تفاصيل المدرسة (School Details DTO)
    /// 📌  الوظيفة: نقل بيانات المدرسة مع جميع البيانات المرتبطة
    /// 📦  الاستخدام: في SchoolsController (GET /{id} endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class SchoolDetailsDto : SchoolDto
    {
        /// <summary>
        /// بيانات الإدارة التعليمية
        /// </summary>
        public DepartmentLookupDto? Department { get; set; }

        /// <summary>
        /// قائمة الصفوف الدراسية في المدرسة
        /// </summary>
        public List<GradeLevelLookupDto> GradeLevels { get; set; } = new();

        /// <summary>
        /// قائمة الأعوام الدراسية في المدرسة
        /// </summary>
        public List<AcademicYearLookupDto> AcademicYears { get; set; } = new();

        /// <summary>
        /// إحصائيات المدرسة
        /// </summary>
        public SchoolStatisticsDto? Statistics { get; set; }
    }
}