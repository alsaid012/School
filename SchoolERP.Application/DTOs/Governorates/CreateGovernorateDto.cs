using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Governorates
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء محافظة جديدة (Create Governorate DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء المحافظة من العميل إلى الخادم
    /// 📦  الاستخدام: في GovernoratesController (POST endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateGovernorateDto
    {
        /// <summary>
        /// اسم المحافظة (مطلوب)
        /// </summary>
        /// <example>القاهرة</example>
        [Required(ErrorMessage = "اسم المحافظة مطلوب")]
        [MaxLength(100, ErrorMessage = "اسم المحافظة لا يتجاوز 100 حرف")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// كود المحافظة (مطلوب وفريد)
        /// </summary>
        /// <example>CAI</example>
        [Required(ErrorMessage = "كود المحافظة مطلوب")]
        [MaxLength(20, ErrorMessage = "كود المحافظة لا يتجاوز 20 حرف")]
        public string Code { get; set; } = string.Empty;
    }
}