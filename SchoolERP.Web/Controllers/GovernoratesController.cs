using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Governorates;
using SchoolERP.Application.Interfaces.Services;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📍  وحدة تحكم المحافظات (GovernoratesController)
    /// 📌  الوظيفة: إدارة عمليات المحافظات (CRUD)
    /// 📦  الاستخدام: نقاط النهاية (Endpoints) للمحافظات
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize]
    public class GovernoratesController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IGovernorateService _governorateService;
        private readonly ILogger<GovernoratesController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public GovernoratesController(
            IGovernorateService governorateService,
            ILogger<GovernoratesController> logger)
        {
            _governorateService = governorateService;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع المحافظات
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _governorateService.GetAllAsync();
                if (!response.Success || response.Data == null)
                {
                    _logger.LogError("خطأ في جلب المحافظات: {Message}", response.Message);
                    return View("Error");
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Index");
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Details ════════════════════════════════════

        /// <summary>
        /// 🔍 عرض تفاصيل محافظة
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _governorateService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Details للمحافظة {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة إنشاء محافظة جديدة
        /// </summary>
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new CreateGovernorateDto());
        }

        /// <summary>
        /// ➕ إنشاء محافظة جديدة (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateGovernorateDto createDto)
        {
            if (!ModelState.IsValid)
                return View(createDto);

            try
            {
                var response = await _governorateService.CreateAsync(createDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إنشاء المحافظة");
                    return View(createDto);
                }

                TempData["Success"] = "تم إنشاء المحافظة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                return View(createDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل محافظة
        /// </summary>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _governorateService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var updateDto = new UpdateGovernorateDto
                {
                    Name = response.Data.Name,
                    Code = response.Data.Code,
                    IsActive = response.Data.IsActive
                };

                ViewBag.Id = id;
                return View(updateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للمحافظة {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث محافظة (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, UpdateGovernorateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(updateDto);
            }

            try
            {
                var response = await _governorateService.UpdateAsync(id, updateDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث المحافظة");
                    ViewBag.Id = id;
                    return View(updateDto);
                }

                TempData["Success"] = "تم تحديث المحافظة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للمحافظة {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                ViewBag.Id = id;
                return View(updateDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Delete ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف محافظة (Soft Delete)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _governorateService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف المحافظة";
                }
                else
                {
                    TempData["Success"] = "تم حذف المحافظة بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Delete للمحافظة {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}