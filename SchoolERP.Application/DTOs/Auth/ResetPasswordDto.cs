using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Auth
{
    /// <summary>
    /// 🔑  نموذج إعادة تعيين كلمة المرور (Reset Password DTO)
    /// </summary>
    public class ResetPasswordDto
    {
        /// <summary>
        /// رمز إعادة التعيين (مخفي)
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// كلمة المرور الجديدة (مطلوبة)
        /// </summary>
        [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور لا تقل عن 6 أحرف")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور الجديدة")]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// تأكيد كلمة المرور الجديدة (مطلوب)
        /// </summary>
        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين")]
        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}