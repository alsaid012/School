using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Auth
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🔑  نموذج تغيير كلمة المرور (Change Password DTO)
    /// 📌  الوظيفة: نقل بيانات تغيير كلمة المرور من العميل إلى الخادم
    /// 📦  الاستخدام: في AuthController (ChangePassword endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ChangePasswordDto
    {
        /// <summary>
        /// كلمة المرور الحالية (مطلوبة)
        /// </summary>
        /// <example>OldPassword@123</example>
        [Required(ErrorMessage = "كلمة المرور الحالية مطلوبة")]
        public string CurrentPassword { get; set; } = string.Empty;

        /// <summary>
        /// كلمة المرور الجديدة (مطلوبة)
        /// </summary>
        /// <example>NewPassword@123</example>
        [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور لا تقل عن 6 أحرف")]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// تأكيد كلمة المرور الجديدة (مطلوب)
        /// </summary>
        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}