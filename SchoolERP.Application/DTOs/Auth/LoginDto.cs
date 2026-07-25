using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Auth
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🔐  نموذج تسجيل الدخول (Login DTO)
    /// 📌  الوظيفة: نقل بيانات تسجيل الدخول من العميل إلى الخادم
    /// 📦  الاستخدام: في AuthController (Login endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// اسم المستخدم (البريد الإلكتروني أو username)
        /// </summary>
        /// <example>ahmed.hassan</example>
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [MaxLength(50, ErrorMessage = "اسم المستخدم لا يتجاوز 50 حرف")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// كلمة المرور
        /// </summary>
        /// <example>Password@123</example>
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور لا تقل عن 6 أحرف")]
        public string Password { get; set; } = string.Empty;
    }
}