using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.UserRoles;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Enums;
using SchoolERP.Web.ViewModels.UserRoles;
using X.PagedList.Extensions;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🎭  وحدة تحكم أدوار المستخدمين (UserRolesController)
    /// 📌  الوظيفة: إدارة عمليات أدوار المستخدمين (CRUD + عمليات إضافية)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class UserRolesController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUserRoleService _roleService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserRolesController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public UserRolesController(
            IUserRoleService roleService,
            IUnitOfWork unitOfWork,
            ILogger<UserRolesController> logger)
        {
            _roleService = roleService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

      

        /// <summary>
        /// 📋 عرض جميع أدوار المستخدمين مع ترقيم صفحات
        /// </summary>
        public async Task<IActionResult> Index(int? userId = null, int? page = 1)
        {
            try
            {
                int pageNumber = page ?? 1;
                int pageSize = 20;

                var viewModel = new UserRoleIndexViewModel
                {
                    SelectedUserId = userId,
                    TotalUsers = 0
                };

                // ✅ جلب جميع المستخدمين للفلترة
                var users = await _unitOfWork.Users.GetAllAsync();
                viewModel.Users = users.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.FullName,
                    Selected = u.Id == userId
                }).ToList();

                // ✅ جلب جميع الأدوار
                var response = await _roleService.GetAllAsync();
                if (response.Success && response.Data != null)
                {
                    var allRoles = response.Data.ToList();

                    // ✅ فلترة حسب المستخدم إذا تم اختياره
                    if (userId.HasValue)
                    {
                        allRoles = allRoles.Where(r => r.UserId == userId.Value).ToList();
                    }

                    // ✅ تجميع الأدوار حسب المستخدم
                    var userGroups = allRoles
                        .GroupBy(r => new { r.UserId, r.UserName })
                        .Select(g => new UserGroupDto
                        {
                            UserId = g.Key.UserId,
                            UserName = g.Key.UserName ?? $"مستخدم {g.Key.UserId}",
                            Roles = g.ToList()
                        })
                        .OrderBy(g => g.UserName)
                        .ToList();

                    viewModel.TotalUsers = userGroups.Count;

                    // ✅ تطبيق الترقيم (Pagination)
                    viewModel.UsersWithRoles = userGroups.ToPagedList(pageNumber, pageSize);
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Index");
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Details ════════════════════════════════════

        /// <summary>
        /// 🔍 عرض تفاصيل دور مستخدم
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _roleService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Details للدور {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة إضافة دور جديد
        /// </summary>
        public async Task<IActionResult> Create(int? userId = null)
        {
            var viewModel = await PrepareCreateViewModelAsync(userId);
            return View(viewModel);
        }

        /// <summary>
        /// ➕ إضافة دور جديد (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRoleCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await PrepareCreateViewModelAsync(viewModel.Role.UserId);
                return View(viewModel);
            }

            try
            {
                var response = await _roleService.CreateAsync(viewModel.Role);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إضافة الدور");
                    viewModel = await PrepareCreateViewModelAsync(viewModel.Role.UserId);
                    return View(viewModel);
                }

                TempData["Success"] = "✅ تم إضافة الدور بنجاح";
                return RedirectToAction(nameof(Index), new { userId = viewModel.Role.UserId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                viewModel = await PrepareCreateViewModelAsync(viewModel.Role.UserId);
                return View(viewModel);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل دور
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _roleService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var data = response.Data;

                var viewModel = new UserRoleEditViewModel
                {
                    Id = id,
                    Role = new UpdateUserRoleDto
                    {
                        IsPrimary = data.IsPrimary,
                        StartDate = data.StartDate,
                        EndDate = data.EndDate,
                        Notes = data.Notes,
                        IsActive = data.IsActive
                    },
                    DisplayInfo = new UserRoleDisplayInfo
                    {
                        UserName = data.UserName ?? string.Empty,
                        RoleTypeName = data.RoleTypeName,
                        IsPrimary = data.IsPrimary,
                        StartDate = data.StartDate,
                        EndDate = data.EndDate,
                        CreatedAt = data.CreatedAt
                    }
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Edit للدور {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث دور (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserRoleEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Id = id;
                return View(viewModel);
            }

            try
            {
                var response = await _roleService.UpdateAsync(id, viewModel.Role);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث الدور");
                    viewModel.Id = id;
                    return View(viewModel);
                }

                TempData["Success"] = "✅ تم تحديث الدور بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Edit للدور {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                viewModel.Id = id;
                return View(viewModel);
            }
        }

        #endregion

        #region ════════════════════════════════════ Delete ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف دور
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _roleService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف الدور";
                }
                else
                {
                    TempData["Success"] = "✅ تم حذف الدور بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Delete للدور {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ════════════════════════════════════ SetPrimary ════════════════════════════════════

        /// <summary>
        /// 🔄 تعيين دور كأساسي
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPrimary(int id, int userId)
        {
            try
            {
                var response = await _roleService.SetPrimaryAsync(id, userId);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء تعيين الدور كأساسي";
                }
                else
                {
                    TempData["Success"] = response.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في SetPrimary للدور {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index), new { userId });
        }

        #endregion

        #region ════════════════════════════════════ Statistics ════════════════════════════════════

        /// <summary>
        /// 📊 عرض إحصائيات أدوار المستخدمين
        /// </summary>
        public async Task<IActionResult> Statistics()
        {
            try
            {
                var response = await _roleService.GetStatisticsAsync();
                if (!response.Success || response.Data == null)
                {
                    ViewBag.Error = response.Message ?? "حدث خطأ أثناء جلب الإحصائيات";
                    return View(new UserRoleStatisticsDto());
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Statistics");
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        /// <summary>
        /// 🔄 تجهيز ViewModel للإنشاء
        /// </summary>
        private async Task<UserRoleCreateViewModel> PrepareCreateViewModelAsync(int? userId = null)
        {
            var viewModel = new UserRoleCreateViewModel();

            // ✅ جلب المستخدمين
            var users = await _unitOfWork.Users.GetAllAsync();
            viewModel.Users = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.FullName,
                Selected = u.Id == userId
            }).ToList();

            // ✅ أنواع الأدوار
            viewModel.RoleTypes = GetRoleTypesList();

            if (userId.HasValue)
            {
                viewModel.Role.UserId = userId.Value;
            }

            return viewModel;
        }

        /// <summary>
        /// 📝 الحصول على قائمة أنواع الأدوار
        /// </summary>
        private List<SelectListItem> GetRoleTypesList(UserType? selected = null)
        {
            return Enum.GetValues(typeof(UserType))
                .Cast<UserType>()
                .Select(r => new SelectListItem
                {
                    Value = ((int)r).ToString(),
                    Text = GetRoleTypeName(r),
                    Selected = r == selected
                }).ToList();
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