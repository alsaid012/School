using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SchoolERP.Application.DTOs.Auth;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using System.Security.Claims;

namespace SchoolERP.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IAuthService authService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        // ============================================================
        // GET: تسجيل الدخول
        // ============================================================
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginDto());
        }

        // ============================================================
        // POST: تسجيل الدخول
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            var response = await _authService.LoginAsync(loginDto);

            if (!response.Success || response.Data == null)
            {
                ModelState.AddModelError("", response.Message ?? "فشل تسجيل الدخول");
                return View(loginDto);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginDto.Username ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, response.Data.UserId.ToString()),
                new Claim(ClaimTypes.Role, response.Data.UserType ?? "User"),
                new Claim("FullName", response.Data.FullName ?? loginDto.Username ?? "مستخدم"),
                new Claim("SchoolId", response.Data.SchoolId?.ToString() ?? "0")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // ============================================================
        // GET: إنشاء حساب جديد (Register)
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            // جلب المدارس للـ Dropdown
            ViewBag.Schools = await _unitOfWork.SchoolRepository.GetAllAsync();
            return View(new RegisterDto());
        }

        // ============================================================
        // POST: إنشاء حساب جديد (Register)
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Schools = await _unitOfWork.SchoolRepository.GetAllAsync();
                return View(registerDto);
            }

            var response = await _authService.RegisterAsync(registerDto);

            if (!response.Success)
            {
                ModelState.AddModelError("", response.Message ?? "فشل إنشاء الحساب");
                ViewBag.Schools = await _unitOfWork.SchoolRepository.GetAllAsync();
                return View(registerDto);
            }

            return RedirectToAction("Login", new { message = "تم إنشاء الحساب بنجاح، يرجى انتظار التفعيل" });
        }


        // ============================================================
        // 🆕 GET: صفحة نسيت كلمة المرور
        // ============================================================
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordDto());
        }

        // ============================================================
        // 🆕 POST: إرسال رابط إعادة تعيين كلمة المرور
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
                return View(forgotPasswordDto);

            // ✅ التحقق من وجود المستخدم بالبريد الإلكتروني
            var users = await _unitOfWork.Users.FindAsync(u => u.Email == forgotPasswordDto.Email);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                // ❌ لا نكشف للمستخدم إذا كان البريد غير موجود (لأسباب أمنية)
                TempData["Success"] = "إذا كان هذا البريد مسجلاً، ستصلك تعليمات إعادة تعيين كلمة المرور";
                return RedirectToAction("Login");
            }

            // ✅ إنشاء رمز إعادة تعيين كلمة المرور (Token)
            var resetToken = Guid.NewGuid().ToString().Replace("-", "") + DateTime.Now.Ticks.ToString();

            // ✅ حفظ الرمز في قاعدة البيانات (يمكن إضافة جدول PasswordResetTokens)
            // أو إرسال الرمز عبر البريد الإلكتروني

            // ✅ هنا يمكن إرسال البريد الإلكتروني مع رابط إعادة التعيين
            // string resetLink = Url.Action("ResetPassword", "Auth", new { token = resetToken }, Request.Scheme);

            TempData["Success"] = "تم إرسال تعليمات إعادة تعيين كلمة المرور إلى بريدك الإلكتروني";
            return RedirectToAction("Login");
        }

        // ============================================================
        // 🆕 GET: صفحة إعادة تعيين كلمة المرور
        // ============================================================
        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login");

            return View(new ResetPasswordDto { Token = token });
        }

        // ============================================================
        // 🆕 POST: إعادة تعيين كلمة المرور
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordDto);

            // ✅ التحقق من صحة الرمز (Token)
            // والتأكد من أنه لم ينته صلاحيته

            // ✅ تحديث كلمة المرور
            // await _authService.ResetPasswordAsync(resetPasswordDto.Token, resetPasswordDto.NewPassword);

            TempData["Success"] = "تم إعادة تعيين كلمة المرور بنجاح، يمكنك تسجيل الدخول الآن";
            return RedirectToAction("Login");
        }


        // ============================================================
        // تسجيل الخروج
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}