using SchoolERP.Application.DTOs.Auth;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Users;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🔐  واجهة خدمة المصادقة (IAuthService)
    /// 📌  الوظيفة: تعريف عمليات تسجيل الدخول والخروج وإنشاء الحسابات
    /// 📦  الاستخدام: في AuthController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 🔑 تسجيل الدخول
        /// </summary>
        /// <param name="loginDto">بيانات تسجيل الدخول</param>
        /// <returns>رمز الدخول (Token) مع بيانات المستخدم</returns>
        Task<ResponseDto<TokenDto>> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// 📝 إنشاء حساب جديد (تسجيل)
        /// </summary>
        /// <param name="registerDto">بيانات التسجيل</param>
        /// <returns>بيانات المستخدم الجديد</returns>
        Task<ResponseDto<UserDto>> RegisterAsync(RegisterDto registerDto);

        /// <summary>
        /// 🚪 تسجيل الخروج
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        Task<ResponseDto> LogoutAsync(int userId);

        /// <summary>
        /// 🔄 تحديث رمز الدخول (Refresh Token)
        /// </summary>
        /// <param name="token">رمز الدخول الحالي</param>
        /// <returns>رمز دخول جديد</returns>
        Task<ResponseDto<TokenDto>> RefreshTokenAsync(string token);

        /// <summary>
        /// ✅ التحقق من صحة رمز الدخول
        /// </summary>
        /// <param name="token">رمز الدخول</param>
        /// <returns>هل الرمز صالح؟</returns>
        Task<ResponseDto<bool>> ValidateTokenAsync(string token);

        /// <summary>
        /// 🔐 تغيير كلمة المرور
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <param name="changePasswordDto">بيانات تغيير كلمة المرور</param>
        Task<ResponseDto> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);

        /// <summary>
        /// 📧 إعادة تعيين كلمة المرور (نسيت كلمة المرور)
        /// </summary>
        /// <param name="email">البريد الإلكتروني</param>
        Task<ResponseDto> ForgotPasswordAsync(string email);

        /// <summary>
        /// 🔑 إعادة تعيين كلمة المرور (بعد التحقق)
        /// </summary>
        /// <param name="token">رمز التحقق</param>
        /// <param name="newPassword">كلمة المرور الجديدة</param>
        Task<ResponseDto> ResetPasswordAsync(string token, string newPassword);

        /// <summary>
        /// 👤 الحصول على بيانات المستخدم الحالي
        /// </summary>
        /// <param name="userId">معرف المستخدم</param>
        /// <returns>بيانات المستخدم</returns>
        Task<ResponseDto<UserDto>> GetCurrentUserAsync(int userId);
    }
}