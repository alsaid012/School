using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Governorates
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات المحافظة (Update Governorate DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث المحافظة من العميل إلى الخادم
    /// 📦  الاستخدام: في GovernoratesController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateGovernorateDto
    {
        /// <summary>
        /// اسم المحافظة
        /// </summary>
        /// <example>القاهرة الجديدة</example>
        [MaxLength(100, ErrorMessage = "اسم المحافظة لا يتجاوز 100 حرف")]
        public string? Name { get; set; }

        /// <summary>
        /// كود المحافظة
        /// </summary>
        /// <example>CAI</example>
        [MaxLength(20, ErrorMessage = "كود المحافظة لا يتجاوز 20 حرف")]
        public string? Code { get; set; }

        /// <summary>
        /// هل المحافظة مفعلة؟
        /// </summary>
        /// <example>true</example>
        public bool? IsActive { get; set; }
    }
}