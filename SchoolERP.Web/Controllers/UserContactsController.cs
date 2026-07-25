using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.UserContacts;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Enums;
using SchoolERP.Web.ViewModels.UserContacts;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📞  وحدة تحكم جهات الاتصال (UserContactsController)
    /// 📌  الوظيفة: إدارة عمليات جهات الاتصال (CRUD + عمليات إضافية)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize]
    public class UserContactsController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IUserContactService _contactService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserContactsController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public UserContactsController(
            IUserContactService contactService,
            IUnitOfWork unitOfWork,
            ILogger<UserContactsController> logger)
        {
            _contactService = contactService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع جهات الاتصال
        /// </summary>
        public async Task<IActionResult> Index(int? userId = null)
        {
            try
            {
                var viewModel = new UserContactIndexViewModel
                {
                    SelectedUserId = userId
                };

                // ✅ جلب المستخدمين للفلترة
                var users = await _unitOfWork.Users.GetAllAsync();
                viewModel.Users = users.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.FullName,
                    Selected = u.Id == userId
                }).ToList();

                // ✅ جلب جهات الاتصال
                if (userId.HasValue)
                {
                    var response = await _contactService.GetByUserIdAsync(userId.Value);
                    if (response.Success && response.Data != null)
                    {
                        viewModel.Contacts = response.Data.ToList();
                    }
                }
                else
                {
                    var response = await _contactService.GetAllAsync();
                    if (response.Success && response.Data != null)
                    {
                        viewModel.Contacts = response.Data.ToList();
                    }
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
        /// 🔍 عرض تفاصيل جهة اتصال
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _contactService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Details لجهة الاتصال {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة إضافة جهة اتصال جديدة
        /// </summary>
        [Authorize(Roles = "Admin,Principal,HR")]
        public async Task<IActionResult> Create(int? userId = null)
        {
            var viewModel = await PrepareCreateViewModelAsync(userId);
            return View(viewModel);
        }

        /// <summary>
        /// ➕ إضافة جهة اتصال جديدة (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal,HR")]
        public async Task<IActionResult> Create(UserContactCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await PrepareCreateViewModelAsync(viewModel.Contact.UserId);
                return View(viewModel);
            }

            try
            {
                var response = await _contactService.CreateAsync(viewModel.Contact);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إضافة جهة الاتصال");
                    viewModel = await PrepareCreateViewModelAsync(viewModel.Contact.UserId);
                    return View(viewModel);
                }

                TempData["Success"] = "✅ تم إضافة جهة الاتصال بنجاح";
                return RedirectToAction(nameof(Index), new { userId = viewModel.Contact.UserId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                viewModel = await PrepareCreateViewModelAsync(viewModel.Contact.UserId);
                return View(viewModel);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل جهة اتصال
        /// </summary>
        [Authorize(Roles = "Admin,Principal,HR")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _contactService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var data = response.Data;

                var viewModel = new UserContactEditViewModel
                {
                    Id = id,
                    Contact = new UpdateUserContactDto
                    {
                        Id = data.Id,
                        ContactType = data.ContactType,
                        ContactValue = data.ContactValue,
                        IsPrimary = data.IsPrimary,
                        IsVerified = data.IsVerified,
                        Notes = data.Notes,
                        IsActive = data.IsActive
                    },
                    DisplayInfo = new UserContactDisplayInfo
                    {
                        UserName = data.UserName ?? string.Empty,
                        ContactTypeName = data.ContactTypeName,
                        ContactValue = data.ContactValue,
                        IsPrimary = data.IsPrimary,
                        IsVerified = data.IsVerified,
                        CreatedAt = data.CreatedAt
                    }
                };

                // ✅ جلب أنواع جهات الاتصال
                ViewBag.ContactTypes = GetContactTypesList(data.ContactType);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Edit لجهة الاتصال {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث جهة اتصال (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal,HR")]
        public async Task<IActionResult> Edit(int id, UserContactEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Id = id;
                ViewBag.ContactTypes = GetContactTypesList(viewModel.Contact.ContactType ?? ContactType.Phone);
                return View(viewModel);
            }

            try
            {
                var response = await _contactService.UpdateAsync(id, viewModel.Contact);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث جهة الاتصال");
                    viewModel.Id = id;
                    ViewBag.ContactTypes = GetContactTypesList(viewModel.Contact.ContactType ?? ContactType.Phone);
                    return View(viewModel);
                }

                TempData["Success"] = "✅ تم تحديث جهة الاتصال بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Edit لجهة الاتصال {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                viewModel.Id = id;
                ViewBag.ContactTypes = GetContactTypesList(viewModel.Contact.ContactType ?? ContactType.Phone);
                return View(viewModel);
            }
        }

        #endregion

        #region ════════════════════════════════════ Delete ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف جهة اتصال
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _contactService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف جهة الاتصال";
                }
                else
                {
                    TempData["Success"] = "✅ تم حذف جهة الاتصال بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Delete لجهة الاتصال {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ════════════════════════════════════ SetPrimary ════════════════════════════════════

        /// <summary>
        /// 🔄 تعيين جهة اتصال كأساسية
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal,HR")]
        public async Task<IActionResult> SetPrimary(int id, int userId)
        {
            try
            {
                var response = await _contactService.SetPrimaryAsync(id, userId);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء تعيين جهة الاتصال كأساسية";
                }
                else
                {
                    TempData["Success"] = response.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في SetPrimary لجهة الاتصال {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index), new { userId });
        }

        #endregion

        #region ════════════════════════════════════ Statistics ════════════════════════════════════

        /// <summary>
        /// 📊 عرض إحصائيات جهات الاتصال
        /// </summary>
        public async Task<IActionResult> Statistics()
        {
            try
            {
                var response = await _contactService.GetStatisticsAsync();
                if (!response.Success || response.Data == null)
                {
                    ViewBag.Error = response.Message ?? "حدث خطأ أثناء جلب الإحصائيات";
                    return View(new UserContactStatisticsDto());
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
        private async Task<UserContactCreateViewModel> PrepareCreateViewModelAsync(int? userId = null)
        {
            var viewModel = new UserContactCreateViewModel();

            // ✅ جلب المستخدمين
            var users = await _unitOfWork.Users.GetAllAsync();
            viewModel.Users = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.FullName,
                Selected = u.Id == userId
            }).ToList();

            // ✅ أنواع جهات الاتصال
            viewModel.ContactTypes = GetContactTypesList();

            if (userId.HasValue)
            {
                viewModel.Contact.UserId = userId.Value;
            }

            return viewModel;
        }

        /// <summary>
        /// 📝 الحصول على قائمة أنواع جهات الاتصال
        /// </summary>
        private List<SelectListItem> GetContactTypesList(ContactType? selected = null)
        {
            return Enum.GetValues(typeof(ContactType))
                .Cast<ContactType>()
                .Select(c => new SelectListItem
                {
                    Value = ((int)c).ToString(),
                    Text = GetContactTypeName(c),
                    Selected = c == selected
                }).ToList();
        }

        /// <summary>
        /// 📝 الحصول على اسم نوع جهة الاتصال بالعربية
        /// </summary>
        private string GetContactTypeName(ContactType type)
        {
            return type switch
            {
                ContactType.Phone => "هاتف",
                ContactType.Mobile => "موبايل",
                ContactType.Email => "بريد إلكتروني",
                ContactType.WhatsApp => "واتساب",
                ContactType.Facebook => "فيسبوك",
                _ => type.ToString()
            };
        }

        #endregion
    }
}