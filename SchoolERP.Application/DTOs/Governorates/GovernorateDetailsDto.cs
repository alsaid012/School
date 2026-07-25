using SchoolERP.Application.DTOs.Departments;

namespace SchoolERP.Application.DTOs.Governorates
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📍  نموذج تفاصيل المحافظة (Governorate Details DTO)
    /// 📌  الوظيفة: نقل بيانات المحافظة مع الإدارات التابعة لها
    /// 📦  الاستخدام: في GovernoratesController (GET /{id} endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class GovernorateDetailsDto : GovernorateDto
    {
        /// <summary>
        /// قائمة الإدارات التعليمية التابعة للمحافظة
        /// </summary>
        public List<DepartmentLookupDto> Departments { get; set; } = new();
    }
}