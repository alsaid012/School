using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.UserRoles;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🎭  خدمة أدوار المستخدمين (UserRoleService)
    /// 📌  الوظيفة: تنفيذ عمليات إدارة أدوار المستخدمين
    /// 📦  الاستخدام: في UserRolesController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UserRoleService : IUserRoleService
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserRoleService> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public UserRoleService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<UserRoleService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ الحصول على الأدوار ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع أدوار المستخدمين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<UserRoleDto>>> GetAllAsync()
        {
            try
            {
                var roles = await _unitOfWork.UserRoles.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<UserRoleDto>>(roles);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.UserName = user?.FullName;
                    dto.RoleTypeName = GetRoleTypeName(dto.RoleType);
                }

                _logger.LogInformation("تم جلب {Count} دور مستخدم", dtos.Count());
                return ResponseDto<IEnumerable<UserRoleDto>>.Ok(dtos, "تم جلب أدوار المستخدمين بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب جميع أدوار المستخدمين");
                return ResponseDto<IEnumerable<UserRoleDto>>.Fail("حدث خطأ أثناء جلب الأدوار", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على أدوار مستخدم معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<UserRoleDto>>> GetByUserIdAsync(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ResponseDto<IEnumerable<UserRoleDto>>.NotFound("المستخدم غير موجود");
                }

                var roles = await _unitOfWork.UserRoles
                    .FindAsync(ur => ur.UserId == userId);
                var dtos = _mapper.Map<IEnumerable<UserRoleDto>>(roles);

                foreach (var dto in dtos)
                {
                    dto.UserName = user.FullName;
                    dto.RoleTypeName = GetRoleTypeName(dto.RoleType);
                }

                return ResponseDto<IEnumerable<UserRoleDto>>.Ok(dtos, "تم جلب أدوار المستخدم بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب أدوار المستخدم {UserId}", userId);
                return ResponseDto<IEnumerable<UserRoleDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على الدور الأساسي لمستخدم
        /// </summary>
        public async Task<ResponseDto<UserRoleDto>> GetPrimaryRoleAsync(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return ResponseDto<UserRoleDto>.NotFound("المستخدم غير موجود");
                }

                var roles = await _unitOfWork.UserRoles
                    .FindAsync(ur => ur.UserId == userId && ur.IsPrimary);
                var role = roles.FirstOrDefault();

                if (role == null)
                {
                    return ResponseDto<UserRoleDto>.NotFound("لا يوجد دور أساسي للمستخدم");
                }

                var dto = _mapper.Map<UserRoleDto>(role);
                dto.UserName = user.FullName;
                dto.RoleTypeName = GetRoleTypeName(role.RoleType);

                return ResponseDto<UserRoleDto>.Ok(dto, "تم جلب الدور الأساسي");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الدور الأساسي للمستخدم {UserId}", userId);
                return ResponseDto<UserRoleDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على المستخدمين الذين لديهم دور معين
        /// </summary>
        public async Task<ResponseDto<IEnumerable<UserRoleDto>>> GetByRoleTypeAsync(int roleType)
        {
            try
            {
                var type = (UserType)roleType;
                var roles = await _unitOfWork.UserRoles
                    .FindAsync(ur => ur.RoleType == type);
                var dtos = _mapper.Map<IEnumerable<UserRoleDto>>(roles);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.UserName = user?.FullName;
                    dto.RoleTypeName = GetRoleTypeName(dto.RoleType);
                }

                return ResponseDto<IEnumerable<UserRoleDto>>.Ok(dtos, "تم جلب المستخدمين حسب الدور");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب المستخدمين حسب الدور {RoleType}", roleType);
                return ResponseDto<IEnumerable<UserRoleDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        /// <summary>
        /// 📋 الحصول على أدوار المستخدمين للقوائم المنسدلة
        /// </summary>
        public async Task<ResponseDto<IEnumerable<UserRoleLookupDto>>> GetLookupAsync(int? userId = null)
        {
            try
            {
                IEnumerable<UserRole> roles;

                if (userId.HasValue)
                {
                    roles = await _unitOfWork.UserRoles
                        .FindAsync(ur => ur.UserId == userId.Value);
                }
                else
                {
                    roles = await _unitOfWork.UserRoles.GetAllAsync();
                }

                var dtos = _mapper.Map<IEnumerable<UserRoleLookupDto>>(roles);

                foreach (var dto in dtos)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                    dto.UserName = user?.FullName;
                    dto.RoleTypeName = GetRoleTypeName(dto.RoleType);
                }

                return ResponseDto<IEnumerable<UserRoleLookupDto>>.Ok(dtos, "تم جلب الأدوار للقوائم");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الأدوار للقوائم");
                return ResponseDto<IEnumerable<UserRoleLookupDto>>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ البحث عن دور ════════════════════════════════════

        /// <summary>
        /// 🔍 الحصول على دور مستخدم بواسطة المعرف
        /// </summary>
        public async Task<ResponseDto<UserRoleDto>> GetByIdAsync(int id)
        {
            try
            {
                var role = await _unitOfWork.UserRoles.GetByIdAsync(id);
                if (role == null)
                {
                    return ResponseDto<UserRoleDto>.NotFound("الدور غير موجود");
                }

                var dto = _mapper.Map<UserRoleDto>(role);

                var user = await _unitOfWork.Users.GetByIdAsync(role.UserId);
                dto.UserName = user?.FullName;
                dto.RoleTypeName = GetRoleTypeName(role.RoleType);

                return ResponseDto<UserRoleDto>.Ok(dto, "تم جلب الدور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب الدور {Id}", id);
                return ResponseDto<UserRoleDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ الإحصائيات ════════════════════════════════════

        /// <summary>
        /// 📊 الحصول على إحصائيات أدوار المستخدمين
        /// </summary>
        public async Task<ResponseDto<UserRoleStatisticsDto>> GetStatisticsAsync()
        {
            try
            {
                var roles = await _unitOfWork.UserRoles.GetAllAsync();

                var statistics = new UserRoleStatisticsDto
                {
                    TotalRoles = roles.Count(),
                    ActiveUsersWithRoles = roles.Select(r => r.UserId).Distinct().Count(),
                    StudentRoles = roles.Count(r => r.RoleType == UserType.Student),
                    TeacherRoles = roles.Count(r => r.RoleType == UserType.Teacher),
                    EmployeeRoles = roles.Count(r => r.RoleType == UserType.Employee),
                    PrincipalRoles = roles.Count(r => r.RoleType == UserType.Principal),
                    AdminRoles = roles.Count(r => r.RoleType == UserType.Admin),
                    PrimaryRoles = roles.Count(r => r.IsPrimary),
                    TemporaryRoles = roles.Count(r => r.EndDate.HasValue),
                    RolesByType = new Dictionary<string, int>(),
                    TopUsersWithRoles = new List<TopUserRolesDto>()
                };

                // حساب المستخدمين متعددي الأدوار
                var userRoleCounts = roles
                    .GroupBy(r => r.UserId)
                    .Select(g => new { UserId = g.Key, Count = g.Count() });
                statistics.MultiRoleUsers = userRoleCounts.Count(u => u.Count > 1);

                // توزيع الأدوار حسب النوع
                var typeDistribution = new Dictionary<string, int>
                {
                    { "طالب", roles.Count(r => r.RoleType == UserType.Student) },
                    { "معلم", roles.Count(r => r.RoleType == UserType.Teacher) },
                    { "موظف", roles.Count(r => r.RoleType == UserType.Employee) },
                    { "مدير", roles.Count(r => r.RoleType == UserType.Principal) },
                    { "أدمن", roles.Count(r => r.RoleType == UserType.Admin) }
                };
                statistics.RolesByType = typeDistribution;

                // أكثر المستخدمين أدواراً
                var topUsers = roles
                    .GroupBy(r => r.UserId)
                    .Select(g => new
                    {
                        UserId = g.Key,
                        Count = g.Count(),
                        RoleNames = g.Select(r => GetRoleTypeName(r.RoleType)).ToList(),
                        HasPrimary = g.Any(r => r.IsPrimary)
                    })
                    .OrderByDescending(x => x.Count)
                    .Take(5)
                    .ToList();

                foreach (var user in topUsers)
                {
                    var userEntity = await _unitOfWork.Users.GetByIdAsync(user.UserId);
                    statistics.TopUsersWithRoles.Add(new TopUserRolesDto
                    {
                        UserId = user.UserId,
                        UserName = userEntity?.FullName ?? string.Empty,
                        RolesCount = user.Count,
                        RoleNames = user.RoleNames,
                        HasPrimaryRole = user.HasPrimary
                    });
                }

                return ResponseDto<UserRoleStatisticsDto>.Ok(statistics, "تم جلب إحصائيات الأدوار");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء جلب إحصائيات الأدوار");
                return ResponseDto<UserRoleStatisticsDto>.Fail("حدث خطأ", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ إنشاء وتحديث وحذف ════════════════════════════════════

        /// <summary>
        /// ➕ إنشاء دور مستخدم جديد
        /// </summary>
        public async Task<ResponseDto<UserRoleDto>> CreateAsync(CreateUserRoleDto createDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(createDto.UserId);
                if (user == null)
                {
                    return ResponseDto<UserRoleDto>.Fail("المستخدم غير موجود");
                }

                // التحقق من وجود دور مكرر
                if (await _unitOfWork.UserRoles
                    .AnyAsync(ur => ur.UserId == createDto.UserId && ur.RoleType == createDto.RoleType))
                {
                    return ResponseDto<UserRoleDto>.Fail($"الدور {GetRoleTypeName(createDto.RoleType)} موجود بالفعل للمستخدم");
                }

                // إذا كان دور أساسي، إلغاء التحديد من البقية
                if (createDto.IsPrimary)
                {
                    await UnsetPrimaryRoleAsync(createDto.UserId);
                }

                var role = _mapper.Map<UserRole>(createDto);
                role.CreatedAt = DateTime.Now;
                role.IsActive = true;

                var created = await _unitOfWork.UserRoles.AddAsync(role);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<UserRoleDto>(created);
                dto.UserName = user.FullName;
                dto.RoleTypeName = GetRoleTypeName(createDto.RoleType);

                _logger.LogInformation("تم إنشاء دور جديد للمستخدم {UserId}", createDto.UserId);

                return ResponseDto<UserRoleDto>.Ok(dto, "تم إنشاء الدور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء إنشاء دور جديد");
                return ResponseDto<UserRoleDto>.Fail("حدث خطأ أثناء إنشاء الدور", statusCode: 500);
            }
        }

        /// <summary>
        /// ✏️ تحديث بيانات دور مستخدم
        /// </summary>
        public async Task<ResponseDto<UserRoleDto>> UpdateAsync(int id, UpdateUserRoleDto updateDto)
        {
            try
            {
                var role = await _unitOfWork.UserRoles.GetByIdAsync(id);
                if (role == null)
                {
                    return ResponseDto<UserRoleDto>.NotFound("الدور غير موجود");
                }

                // إذا كان دور أساسي، إلغاء التحديد من البقية
                if (updateDto.IsPrimary && updateDto.IsPrimary)
                {
                    await UnsetPrimaryRoleAsync(role.UserId, id);
                }

                _mapper.Map(updateDto, role);
                role.UpdatedAt = DateTime.Now;

                await _unitOfWork.UserRoles.UpdateAsync(role);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<UserRoleDto>(role);

                var user = await _unitOfWork.Users.GetByIdAsync(role.UserId);
                dto.UserName = user?.FullName;
                dto.RoleTypeName = GetRoleTypeName(role.RoleType);

                _logger.LogInformation("تم تحديث الدور {Id}", id);
                return ResponseDto<UserRoleDto>.Ok(dto, "تم تحديث الدور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تحديث الدور {Id}", id);
                return ResponseDto<UserRoleDto>.Fail("حدث خطأ أثناء تحديث الدور", statusCode: 500);
            }
        }

        /// <summary>
        /// 🗑️ حذف دور مستخدم
        /// </summary>
        public async Task<ResponseDto> DeleteAsync(int id)
        {
            try
            {
                var role = await _unitOfWork.UserRoles.GetByIdAsync(id);
                if (role == null)
                {
                    return ResponseDto.NotFound("الدور غير موجود");
                }

                await _unitOfWork.UserRoles.DeleteAsync(role);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم حذف الدور {Id}", id);
                return ResponseDto.Ok("تم حذف الدور بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء حذف الدور {Id}", id);
                return ResponseDto.Fail("حدث خطأ أثناء حذف الدور", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ تعيين دور أساسي ════════════════════════════════════

        /// <summary>
        /// 🔄 تعيين دور كأساسي
        /// </summary>
        public async Task<ResponseDto> SetPrimaryAsync(int id, int userId)
        {
            try
            {
                var role = await _unitOfWork.UserRoles.GetByIdAsync(id);
                if (role == null || role.UserId != userId)
                {
                    return ResponseDto.NotFound("الدور غير موجود");
                }

                // إلغاء التحديد من البقية
                await UnsetPrimaryRoleAsync(userId, id);

                // تعيين الحالي كأساسي
                role.IsPrimary = true;
                role.UpdatedAt = DateTime.Now;

                await _unitOfWork.UserRoles.UpdateAsync(role);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("تم تعيين الدور {Id} كأساسي للمستخدم {UserId}", id, userId);
                return ResponseDto.Ok("تم تعيين الدور كأساسي بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء تعيين الدور {Id} كأساسي", id);
                return ResponseDto.Fail("حدث خطأ أثناء تعيين الدور", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ التحقق من الوجود ════════════════════════════════════

        /// <summary>
        /// ✅ التحقق من وجود دور مكرر لنفس المستخدم
        /// </summary>
        public async Task<ResponseDto<bool>> IsExistsAsync(int userId, int roleType)
        {
            try
            {
                var type = (UserType)roleType;
                var exists = await _unitOfWork.UserRoles
                    .AnyAsync(ur => ur.UserId == userId && ur.RoleType == type);
                return ResponseDto<bool>.Ok(exists, exists ? "الدور موجود" : "الدور غير موجود");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء التحقق من وجود الدور");
                return ResponseDto<bool>.Fail("حدث خطأ أثناء التحقق", statusCode: 500);
            }
        }

        #endregion

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        /// <summary>
        /// 🔄 إلغاء تحديد الدور الأساسي
        /// </summary>
        private async Task UnsetPrimaryRoleAsync(int userId, int? excludeId = null)
        {
            var roles = await _unitOfWork.UserRoles
                .FindAsync(ur => ur.UserId == userId && ur.IsPrimary && (excludeId == null || ur.Id != excludeId));

            foreach (var role in roles)
            {
                role.IsPrimary = false;
                role.UpdatedAt = DateTime.Now;
                await _unitOfWork.UserRoles.UpdateAsync(role);
            }
            await _unitOfWork.CompleteAsync();
        }

        /// <summary>
        /// 📝 الحصول على اسم نوع الدور بالعربية
        /// </summary>
        private string GetRoleTypeName(UserType type)
        {
            return type switch
            {
                UserType.Student => "طالب",
                UserType.Teacher => "معلم",
                UserType.Employee => "موظف",
                UserType.Principal => "مدير",
                UserType.Admin => "أدمن",
                _ => type.ToString()
            };
        }

        #endregion
    }
}