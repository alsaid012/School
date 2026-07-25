namespace SchoolERP.Application.DTOs.Auth
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🎫  نموذج رمز الدخول (Token DTO)
    /// 📌  الوظيفة: نقل رمز JWT والبيانات المرتبطة به إلى العميل
    /// 📦  الاستخدام: بعد تسجيل الدخول الناجح
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class TokenDto
    {

        public int UserId { get; set; } 


        /// <summary>
        /// رمز الدخول (JWT Token)
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// تاريخ انتهاء صلاحية الرمز
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// نوع الرمز (Bearer)
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// اسم المستخدم
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// الاسم الكامل
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// البريد الإلكتروني
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// نوع المستخدم
        /// </summary>
        public string UserType { get; set; } = string.Empty;

        /// <summary>
        /// قائمة الأدوار (للمستخدمين متعددي الأدوار)
        /// </summary>
        public List<string> Roles { get; set; } = new();

        /// <summary>
        /// معرف المدرسة
        /// </summary>
        public int? SchoolId { get; set; }

        /// <summary>
        /// اسم المدرسة
        /// </summary>
        public string? SchoolName { get; set; }
    }
}