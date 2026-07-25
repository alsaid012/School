using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolERP.Application.DTOs.GradeLevels;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📚  وحدة تحكم الصفوف الدراسية (GradeLevelsController)
    /// 📌  الوظيفة: إدارة عمليات الصفوف الدراسية (CRUD)
    /// 📦  الاستخدام: نقاط النهاية (Endpoints) للصفوف الدراسية
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize]
    public class GradeLevelsController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IGradeLevelService _gradeLevelService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GradeLevelsController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public GradeLevelsController(
            IGradeLevelService gradeLevelService,
            IUnitOfWork unitOfWork,
            ILogger<GradeLevelsController> logger)
        {
            _gradeLevelService = gradeLevelService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع الصفوف الدراسية
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _gradeLevelService.GetAllAsync();
                if (!response.Success || response.Data == null)
                {
                    _logger.LogError("خطأ في جلب الصفوف: {Message}", response.Message);
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
        /// 🔍 عرض تفاصيل صف دراسي
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _gradeLevelService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Details للصف {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة إنشاء صف جديد
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create()
        {
            // جلب المدارس للقائمة المنسدلة
            var schools = await _unitOfWork.SchoolRepository.GetAllAsync();
            ViewBag.Schools = schools.ToList();

            return View(new CreateGradeLevelDto());
        }

        /// <summary>
        /// ➕ إنشاء صف جديد (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create(CreateGradeLevelDto createDto)
        {
            if (!ModelState.IsValid)
            {
                var schools = await _unitOfWork.SchoolRepository.GetAllAsync();
                ViewBag.Schools = schools.ToList();
                return View(createDto);
            }

            try
            {
                var response = await _gradeLevelService.CreateAsync(createDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إنشاء الصف");
                    var schools = await _unitOfWork.SchoolRepository.GetAllAsync();
                    ViewBag.Schools = schools.ToList();
                    return View(createDto);
                }

                TempData["Success"] = "تم إنشاء الصف بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                var schools = await _unitOfWork.SchoolRepository.GetAllAsync();
                ViewBag.Schools = schools.ToList();
                return View(createDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل صف
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _gradeLevelService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var updateDto = new UpdateGradeLevelDto
                {
                    GradeName = response.Data.GradeName,
                    GradeNumber = response.Data.GradeNumber,
                    GradeStage = response.Data.GradeStage,
                    Description = response.Data.Description,
                    IsActive = response.Data.IsActive
                };

                ViewBag.Id = id;
                return View(updateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للصف {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث صف (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id, UpdateGradeLevelDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(updateDto);
            }

            try
            {
                var response = await _gradeLevelService.UpdateAsync(id, updateDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث الصف");
                    ViewBag.Id = id;
                    return View(updateDto);
                }

                TempData["Success"] = "تم تحديث الصف بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للصف {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                ViewBag.Id = id;
                return View(updateDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Delete ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف صف (Soft Delete)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _gradeLevelService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف الصف";
                }
                else
                {
                    TempData["Success"] = "تم حذف الصف بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Delete للصف {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}