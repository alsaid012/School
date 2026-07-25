using SchoolERP.Application.DTOs.Schools;

namespace SchoolERP.Application.DTOs.Departments
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏢  نموذج تفاصيل الإدارة التعليمية (Department Details DTO)
    /// 📌  الوظيفة: نقل بيانات الإدارة مع المدارس التابعة لها
    /// 📦  الاستخدام: في DepartmentsController (GET /{id} endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class DepartmentDetailsDto : DepartmentDto
    {
        /// <summary>
        /// قائمة المدارس التابعة للإدارة
        /// </summary>
        public List<SchoolLookupDto> Schools { get; set; } = new();

        /// <summary>
        /// إحصائيات الإدارة
        /// </summary>
        public DepartmentStatisticsDto? Statistics { get; set; }
    }
}