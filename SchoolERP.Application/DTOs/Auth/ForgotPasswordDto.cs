using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Auth
{
    /// <summary>
    /// 📧  نموذج طلب إعادة تعيين كلمة المرور (Forgot Password DTO)
    /// </summary>
    public class ForgotPasswordDto
    {
        /// <summary>
        /// البريد الإلكتروني المسجل (مطلوب)
        /// </summary>
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; } = string.Empty;
    }
}