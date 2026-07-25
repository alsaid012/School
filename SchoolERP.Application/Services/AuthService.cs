using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SchoolERP.Application.DTOs.Auth;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Users;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🔐  خدمة المصادقة (AuthService)
    /// 📌  الوظيفة: تنفيذ عمليات تسجيل الدخول والخروج وإنشاء الحسابات
    /// 📦  الاستخدام: في AuthController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class AuthService : IAuthService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ تسجيل الدخول ════════════════════════════════════

        /// <summary>
        /// 🔑 تسجيل الدخول
        /// </summary>
        public async Task<ResponseDto<TokenDto>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                // 1. البحث عن المستخدم
                var user = await _unitOfWork.Users.GetUserByUsernameAsync(loginDto.Username);
                if (user == null)
                {
                    return ResponseDto<TokenDto>.Fail("اسم المستخدم أو كلمة المرور غير صحيحة", statusCode: 401);
                }

                // 2. التحقق من كلمة المرور
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    return ResponseDto<TokenDto>.Fail("اسم المستخدم أو كلمة المرور غير صحيحة", statusCode: 401);
                }

                // 3. التحقق من حالة المستخدم
                if (user.Status == UserStatus.Suspended)
                {
                    return ResponseDto<TokenDto>.Fail("الحساب موقوف، يرجى التواصل مع الإدارة", statusCode: 403);
                }

                if (user.Status == UserStatus.Inactive)
                {
                    return ResponseDto<TokenDto>.Fail("الحساب غير نشط، يرجى التواصل مع الإدارة", statusCode: 403);
                }

                if (user.Status == UserStatus.Pending)
                {
                    return ResponseDto<TokenDto>.Fail("الحساب في انتظار التفعيل، يرجى التواصل مع الإدارة", statusCode: 403);
                }

                // 4. تحديث آخر تسجيل دخول
                user.LastLogin = DateTime.Now;
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.CompleteAsync();

                // 5. إنشاء Token
                var token = GenerateJwtToken(user);

                // 6. جلب الأدوار
                var roles = user.UserRoles.Select(r => r.RoleType.ToString()).ToList();

                // 7. جلب اسم المدرسة
                var schoolName = user.School?.SchoolName;

                // 8. إرجاع Token
                var tokenDto = new TokenDto
                {
                    UserId = user.Id,
                    Token = token,
                    ExpiresAt = DateTime.Now.AddMinutes(GetJwtExpiryMinutes()),
                    TokenType = "Bearer",
                    Username = user.Username,
                    FullName = user.FullName,
                    Email = user.Email ?? string.Empty,
                    UserType = user.UserType.ToString(),
                    Roles = roles,
                    SchoolId = user.SchoolId,
                    SchoolName = schoolName
                };

                _logger.LogInformation("تم تسجيل دخول المستخدم {Username} بنجاح", user.Username);
                return ResponseDto<TokenDto>.Ok(tokenDto, "تم تسجيل الدخول بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تسجيل الدخول للمستخدم {Username}", loginDto.Username);
                return ResponseDto<TokenDto>.Fail("حدث خطأ أثناء تسجيل الدخول، يرجى المحاولة مرة أخرى", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ التسجيل ════════════════════════════════════

        /// <summary>
        /// 📝 إنشاء حساب جديد (تسجيل)
        /// </summary>
        public async Task<ResponseDto<UserDto>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // 1. التحقق من وجود اسم المستخدم
                if (await _unitOfWork.Users.UsernameExistsAsync(registerDto.Username))
                {
                    return ResponseDto<UserDto>.Fail("اسم المستخدم موجود بالفعل");
                }

                // 2. التحقق من وجود الرقم القومي
                if (await _unitOfWork.Users.NationalIdExistsAsync(registerDto.NationalId))
                {
                    return ResponseDto<UserDto>.Fail("الرقم القومي موجود بالفعل");
                }

                // 3. التحقق من وجود المدرسة
                var school = await _unitOfWork.SchoolRepository.GetByIdAsync(registerDto.SchoolId);
                if (school == null)
                {
                    return ResponseDto<UserDto>.Fail("المدرسة غير موجودة");
                }

                // 4. إنشاء المستخدم
                var user = new User
                {
                    SchoolId = registerDto.SchoolId,
                    Username = registerDto.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                    FullName = registerDto.FullName,
                    NationalId = registerDto.NationalId,
                    Email = registerDto.Email,
                    DateOfBirth = registerDto.DateOfBirth,
                    Address = registerDto.Address,
                    UserType = registerDto.UserType,
                    Status = UserStatus.Pending, // يحتاج تفعيل من الأدمن
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                var createdUser = await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.CompleteAsync();

                // 5. إضافة الدور الأساسي
                var userRole = new UserRole
                {
                    UserId = createdUser.Id,
                    RoleType = registerDto.UserType,
                    IsPrimary = true,
                    StartDate = DateTime.Now
                };
                await _unitOfWork.UserRoles.AddAsync(userRole);
                await _unitOfWork.CompleteAsync();

                // 6. إضافة جهات الاتصال (إذا وجدت)
                if (!string.IsNullOrEmpty(registerDto.PhoneNumber))
                {
                    var contact = new UserContact
                    {
                        UserId = createdUser.Id,
                        ContactType = ContactType.Phone,
                        ContactValue = registerDto.PhoneNumber,
                        IsPrimary = true,
                        IsVerified = false
                    };
                    await _unitOfWork.UserContacts.AddAsync(contact);
                    await _unitOfWork.CompleteAsync();
                }

                if (!string.IsNullOrEmpty(registerDto.Email))
                {
                    var contact = new UserContact
                    {
                        UserId = createdUser.Id,
                        ContactType = ContactType.Email,
                        ContactValue = registerDto.Email,
                        IsPrimary = false,
                        IsVerified = false
                    };
                    await _unitOfWork.UserContacts.AddAsync(contact);
                    await _unitOfWork.CompleteAsync();
                }

                // 7. إنشاء الكيان المناسب حسب نوع المستخدم
                await CreateUserTypeEntityAsync(createdUser.Id, registerDto);

                await _unitOfWork.CompleteAsync();

                // 8. إرجاع البيانات
                var userDto = _mapper.Map<UserDto>(createdUser);
                _logger.LogInformation("تم إنشاء حساب جديد للمستخدم {Username}", createdUser.Username);

                return ResponseDto<UserDto>.Ok(userDto, "تم إنشاء الحساب بنجاح، في انتظار التفعيل من الإدارة");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء حساب جديد للمستخدم {Username}", registerDto.Username);
                return ResponseDto<UserDto>.Fail("حدث خطأ أثناء إنشاء الحساب، يرجى المحاولة مرة أخرى", statusCode: 500);
            }
        }

        /// <summary>
        /// ➕ إنشاء الكيان المناسب حسب نوع المستخدم
        /// </summary>
        private async Task CreateUserTypeEntityAsync(int userId, RegisterDto registerDto)
        {
            switch (registerDto.UserType)
            {
                case UserType.Student:
                    var student = new Student
                    {
                        UserId = userId,
                        StudentCode = registerDto.StudentCode ?? GenerateCode("STU"),
                        AcademicYearId = 1, // سيتم تحديدها لاحقاً
                        ClassRoomId = registerDto.ClassRoomId,
                        ParentName = registerDto.ParentName,
                        ParentPhone = registerDto.ParentPhone,
                        EnrollmentDate = DateTime.Now,
                        IsGraduated = false
                    };
                    await _unitOfWork.Students.AddAsync(student);
                    break;

                case UserType.Teacher:
                    var teacher = new Teacher
                    {
                        UserId = userId,
                        TeacherCode = registerDto.TeacherCode ?? GenerateCode("TCH"),
                        Qualification = registerDto.Qualification,
                        Specialization = registerDto.Specialization,
                        HireDate = DateTime.Now,
                        IsHomeroomTeacher = false
                    };
                    await _unitOfWork.TeacherRepository.AddAsync(teacher);
                    break;

                case UserType.Employee:
                    var employee = new Employee
                    {
                        UserId = userId,
                        EmployeeCode = registerDto.EmployeeCode ?? GenerateCode("EMP"),
                        JobTitle = registerDto.JobTitle ?? "موظف",
                        Department = registerDto.Department,
                        HireDate = DateTime.Now
                    };
                    await _unitOfWork.EmployeeRepository.AddAsync(employee);
                    break;

                case UserType.Principal:
                    // يمكن إضافة منطق للمدير هنا
                    break;

                case UserType.Admin:
                    // لا يحتاج كيان إضافي
                    break;
            }
        }

        #endregion

        #region ════════════════════════════════════ تسجيل الخروج ════════════════════════════════════

        /// <summary>
        /// 🚪 تسجيل الخروج
        /// </summary>
        public async Task<ResponseDto> LogoutAsync(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user != null)
                {
                    _logger.LogInformation("تم تسجيل خروج المستخدم {Username}", user.Username);
                }
                return ResponseDto.Ok("تم تسجيل الخروج بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تسجيل الخروج للمستخدم {UserId}", userId);
                return ResponseDto.Fail("حدث خطأ أثناء تسجيل الخروج", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ تحديث رمز الدخول ════════════════════════════════════

        /// <summary>
        /// 🔄 تحديث رمز الدخول (Refresh Token)
        /// </summary>
        public async Task<ResponseDto<TokenDto>> RefreshTokenAsync(string token)
        {
            try
            {
                // التحقق من صحة الرمز
                var principal = GetPrincipalFromExpiredToken(token);
                if (principal == null)
                {
                    return ResponseDto<TokenDto>.Fail("رمز الدخول غير صالح", statusCode: 401);
                }

                var userIdClaim = principal.FindFirst("userId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return ResponseDto<TokenDto>.Fail("رمز الدخول غير صالح", statusCode: 401);
                }

                var user = await _unitOfWork.Users.GetUserWithDetailsAsync(userId);
                if (user == null || user.Status != UserStatus.Active)
                {
                    return ResponseDto<TokenDto>.Fail("المستخدم غير نشط", statusCode: 401);
                }

                // إنشاء رمز جديد
                var newToken = GenerateJwtToken(user);

                var tokenDto = new TokenDto
                {
                    Token = newToken,
                    ExpiresAt = DateTime.Now.AddMinutes(GetJwtExpiryMinutes()),
                    TokenType = "Bearer",
                    Username = user.Username,
                    FullName = user.FullName,
                    Email = user.Email ?? string.Empty,
                    UserType = user.UserType.ToString(),
                    Roles = user.UserRoles.Select(r => r.RoleType.ToString()).ToList(),
                    SchoolId = user.SchoolId,
                    SchoolName = user.School?.SchoolName
                };

                return ResponseDto<TokenDto>.Ok(tokenDto, "تم تحديث رمز الدخول بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تحديث رمز الدخول");
                return ResponseDto<TokenDto>.Fail("حدث خطأ أثناء تحديث رمز الدخول", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ التحقق من الرمز ════════════════════════════════════

        /// <summary>
        /// ✅ التحقق من صحة رمز الدخول
        /// </summary>
        public async Task<ResponseDto<bool>> ValidateTokenAsync(string token)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(token);
                if (principal == null)
                {
                    return ResponseDto<bool>.Ok(false, "رمز الدخول غير صالح");
                }

                var userIdClaim = principal.FindFirst("userId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return ResponseDto<bool>.Ok(false, "رمز الدخول غير صالح");
                }

                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null || user.Status != UserStatus.Active)
                {
                    return ResponseDto<bool>.Ok(false, "المستخدم غير نشط");
                }

                return ResponseDto<bool>.Ok(true, "رمز الدخول صالح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء التحقق من رمز الدخول");
                return ResponseDto<bool>.Ok(false, "حدث خطأ أثناء التحقق من رمز الدخول");
            }
        }

        #endregion

        #region ════════════════════════════════════ تغيير كلمة المرور ════════════════════════════════════

        /// <summary>
        /// 🔐 تغيير كلمة المرور
        /// </summary>
        public async Task<ResponseDto> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ResponseDto.Fail("المستخدم غير موجود", statusCode: 404);
                }

                // التحقق من كلمة المرور الحالية
                if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
                {
                    return ResponseDto.Fail("كلمة المرور الحالية غير صحيحة", statusCode: 400);
                }

                // تحديث كلمة المرور
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
                user.UpdatedAt = DateTime.Now;

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم تغيير كلمة المرور للمستخدم {Username}", user.Username);
                return ResponseDto.Ok("تم تغيير كلمة المرور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تغيير كلمة المرور للمستخدم {UserId}", userId);
                return ResponseDto.Fail("حدث خطأ أثناء تغيير كلمة المرور", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ نسيت كلمة المرور ════════════════════════════════════

        /// <summary>
        /// 📧 إعادة تعيين كلمة المرور (نسيت كلمة المرور)
        /// </summary>
        public async Task<ResponseDto> ForgotPasswordAsync(string email)
        {
            try
            {
                // البحث عن المستخدم بالبريد الإلكتروني
                var user = await _unitOfWork.Users.FindAsync(u => u.Email == email);
                var userList = user.ToList();
                var foundUser = userList.FirstOrDefault();

                if (foundUser == null)
                {
                    // لا نكشف عن وجود المستخدم لأسباب أمنية
                    return ResponseDto.Ok("إذا كان البريد الإلكتروني مسجلاً، سيتم إرسال رابط إعادة التعيين");
                }

                // هنا سيتم إرسال بريد إلكتروني برابط إعادة التعيين
                // سيتم تنفيذ هذه الوظيفة لاحقاً باستخدام Email Service

                _logger.LogInformation("تم طلب إعادة تعيين كلمة المرور للمستخدم {Email}", email);
                return ResponseDto.Ok("إذا كان البريد الإلكتروني مسجلاً، سيتم إرسال رابط إعادة التعيين");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء طلب إعادة تعيين كلمة المرور للبريد {Email}", email);
                return ResponseDto.Fail("حدث خطأ أثناء طلب إعادة تعيين كلمة المرور", statusCode: 500);
            }
        }

        /// <summary>
        /// 🔑 إعادة تعيين كلمة المرور (بعد التحقق)
        /// </summary>
        public async Task<ResponseDto> ResetPasswordAsync(string token, string newPassword)
        {
            try
            {
                // التحقق من صحة الرمز
                var principal = GetPrincipalFromExpiredToken(token);
                if (principal == null)
                {
                    return ResponseDto.Fail("رمز إعادة التعيين غير صالح", statusCode: 400);
                }

                var userIdClaim = principal.FindFirst("userId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return ResponseDto.Fail("رمز إعادة التعيين غير صالح", statusCode: 400);
                }

                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ResponseDto.Fail("المستخدم غير موجود", statusCode: 404);
                }

                // تحديث كلمة المرور
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                user.UpdatedAt = DateTime.Now;

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم إعادة تعيين كلمة المرور للمستخدم {Username}", user.Username);
                return ResponseDto.Ok("تم إعادة تعيين كلمة المرور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إعادة تعيين كلمة المرور");
                return ResponseDto.Fail("حدث خطأ أثناء إعادة تعيين كلمة المرور", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ المستخدم الحالي ════════════════════════════════════

        /// <summary>
        /// 👤 الحصول على بيانات المستخدم الحالي
        /// </summary>
        public async Task<ResponseDto<UserDto>> GetCurrentUserAsync(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserWithDetailsAsync(userId);
                if (user == null)
                {
                    return ResponseDto<UserDto>.Fail("المستخدم غير موجود", statusCode: 404);
                }

                var userDto = _mapper.Map<UserDto>(user);
                return ResponseDto<UserDto>.Ok(userDto, "تم جلب بيانات المستخدم بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب بيانات المستخدم الحالي {UserId}", userId);
                return ResponseDto<UserDto>.Fail("حدث خطأ أثناء جلب بيانات المستخدم", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        /// <summary>
        /// 🔑 إنشاء رمز JWT
        /// </summary>
        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim("userType", user.UserType.ToString()),
                new Claim("schoolId", user.SchoolId.ToString()),
                new Claim(ClaimTypes.GivenName, user.FullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            // إضافة الأدوار كـ Claims
            foreach (var role in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleType.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "YourSuperSecretKeyHere12345678901234567890"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(GetJwtExpiryMinutes());

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "SchoolERPAPI",
                audience: _configuration["Jwt:Audience"] ?? "SchoolERPClient",
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 📖 استخراج البيانات من رمز منتهي الصلاحية
        /// </summary>
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "YourSuperSecretKeyHere12345678901234567890")),
                    ValidateLifetime = false
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// ⏱️ الحصول على مدة صلاحية الرمز (بالدقائق)
        /// </summary>
        private double GetJwtExpiryMinutes()
        {
            var expiry = _configuration["Jwt:ExpiryMinutes"];
            return double.TryParse(expiry, out var minutes) ? minutes : 60;
        }

        /// <summary>
        /// 🏷️ إنشاء كود فريد
        /// </summary>
        private string GenerateCode(string prefix)
        {
            var random = new Random();
            var number = random.Next(1000, 9999);
            return $"{prefix}-{DateTime.Now:yyyyMMdd}-{number}";
        }

        #endregion
    }
}