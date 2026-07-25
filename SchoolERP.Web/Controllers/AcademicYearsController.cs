using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.AcademicYears;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Web.ViewModels.AcademicYears;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📆  وحدة تحكم الأعوام الدراسية (AcademicYearsController)
    /// 📌  الوظيفة: إدارة عمليات الأعوام الدراسية (CRUD + عمليات إضافية)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize]
    public class AcademicYearsController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IAcademicYearService _academicYearService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AcademicYearsController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public AcademicYearsController(
            IAcademicYearService academicYearService,
            IUnitOfWork unitOfWork,
            ILogger<AcademicYearsController> logger)
        {
            _academicYearService = academicYearService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع الأعوام الدراسية
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _academicYearService.GetAllAsync();
                if (!response.Success || response.Data == null)
                {
                    _logger.LogError("خطأ في جلب الأعوام الدراسية: {Message}", response.Message);
                    return View("Error");
                }

                return View(response.Data);
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
        /// 🔍 عرض تفاصيل عام دراسي
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _academicYearService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Details للعام الدراسي {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة إنشاء عام دراسي جديد
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create()
        {
            var viewModel = await PrepareCreateViewModelAsync();
            return View(viewModel);
        }

        /// <summary>
        /// ➕ إنشاء عام دراسي جديد (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create(CreateAcademicYearDto createDto)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = await PrepareCreateViewModelAsync(createDto);
                return View(viewModel);
            }

            try
            {
                var response = await _academicYearService.CreateAsync(createDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إنشاء العام الدراسي");
                    var viewModel = await PrepareCreateViewModelAsync(createDto);
                    return View(viewModel);
                }

                TempData["Success"] = "✅ تم إنشاء العام الدراسي بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                var viewModel = await PrepareCreateViewModelAsync(createDto);
                return View(viewModel);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل عام دراسي
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _academicYearService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var data = response.Data;

                var updateDto = new UpdateAcademicYearDto
                {
                    YearName = data.YearName,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    IsCurrent = data.IsCurrent,
                    IsActive = data.IsActive
                };

                ViewBag.Id = id;
                ViewBag.SchoolName = data.SchoolName;
                ViewBag.StudentsCount = data.StudentsCount;

                return View(updateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Edit للعام الدراسي {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث عام دراسي (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id, UpdateAcademicYearDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(updateDto);
            }

            try
            {
                var response = await _academicYearService.UpdateAsync(id, updateDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث العام الدراسي");
                    ViewBag.Id = id;
                    return View(updateDto);
                }

                TempData["Success"] = "✅ تم تحديث العام الدراسي بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Edit للعام الدراسي {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                ViewBag.Id = id;
                return View(updateDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Delete ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف عام دراسي
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _academicYearService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف العام الدراسي";
                }
                else
                {
                    TempData["Success"] = "✅ تم حذف العام الدراسي بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Delete للعام الدراسي {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ════════════════════════════════════ SetCurrent ════════════════════════════════════

        /// <summary>
        /// 🔄 تعيين عام دراسي كعام حالي
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> SetCurrent(int id)
        {
            try
            {
                var response = await _academicYearService.SetCurrentYearAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء تعيين العام الدراسي كعام حالي";
                }
                else
                {
                    TempData["Success"] = response.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في SetCurrent للعام الدراسي {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ════════════════════════════════════ Statistics ════════════════════════════════════

        /// <summary>
        /// 📊 عرض إحصائيات العام الدراسي
        /// </summary>
        public async Task<IActionResult> Statistics(int id)
        {
            try
            {
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(id);
                if (academicYear == null)
                {
                    return NotFound("العام الدراسي غير موجود");
                }

                var response = await _academicYearService.GetStatisticsAsync(id);
                if (!response.Success || response.Data == null)
                {
                    ViewBag.Error = response.Message ?? "حدث خطأ أثناء جلب الإحصائيات";
                    return View(new AcademicYearStatisticsDto());
                }

                ViewBag.YearName = academicYear.YearName;
                ViewBag.SchoolName = (await _unitOfWork.SchoolRepository.GetByIdAsync(academicYear.SchoolId))?.SchoolName;
                ViewBag.StartDate = academicYear.StartDate;
                ViewBag.EndDate = academicYear.EndDate;
                ViewBag.IsCurrent = academicYear.IsCurrent;

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Statistics للعام الدراسي {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        /// <summary>
        /// 🔄 تجهيز ViewModel للإنشاء
        /// </summary>
        private async Task<AcademicYearCreateViewModel> PrepareCreateViewModelAsync(CreateAcademicYearDto? selected = null)
        {
            var viewModel = new AcademicYearCreateViewModel();

            if (selected != null)
            {
                viewModel.AcademicYear = selected;
            }

            // ✅ جلب المدارس
            var schools = await _unitOfWork.SchoolRepository.GetAllAsync();
            viewModel.Schools = schools.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.SchoolName ?? string.Empty,
                Selected = selected != null && s.Id == selected.SchoolId
            }).ToList();

            return viewModel;
        }

        #endregion
    }
}