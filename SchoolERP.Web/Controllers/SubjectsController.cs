using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Subjects;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📖  وحدة تحكم المواد الدراسية (SubjectsController)
    /// 📌  الوظيفة: إدارة عمليات المواد الدراسية (CRUD)
    /// 📦  الاستخدام: نقاط النهاية (Endpoints) للمواد الدراسية
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize]
    public class SubjectsController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly ISubjectService _subjectService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SubjectsController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public SubjectsController(
            ISubjectService subjectService,
            IUnitOfWork unitOfWork,
            ILogger<SubjectsController> logger)
        {
            _subjectService = subjectService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع المواد الدراسية
        /// </summary>
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string? searchTerm = null)
        {
            try
            {
                var pagination = new PaginationDto
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchTerm = searchTerm,
                    SortBy = "SubjectName",
                    SortDirection = "ASC"
                };

                var response = await _subjectService.GetPagedAsync(pagination);
                if (!response.Success || response.Data == null)
                {
                    _logger.LogError("خطأ في جلب المواد: {Message}", response.Message);
                    return View("Error");
                }

                ViewBag.SearchTerm = searchTerm;
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
        /// 🔍 عرض تفاصيل مادة دراسية
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _subjectService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Details للمادة {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة إنشاء مادة جديدة
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create()
        {
            // جلب الصفوف للقائمة المنسدلة
            var gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
            ViewBag.GradeLevels = gradeLevels.ToList();

            return View(new CreateSubjectDto());
        }

        /// <summary>
        /// ➕ إنشاء مادة جديدة (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create(CreateSubjectDto createDto)
        {
            if (!ModelState.IsValid)
            {
                var gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
                ViewBag.GradeLevels = gradeLevels.ToList();
                return View(createDto);
            }

            try
            {
                var response = await _subjectService.CreateAsync(createDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إنشاء المادة");

                    var gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
                    ViewBag.GradeLevels = gradeLevels.ToList();
                    return View(createDto);
                }

                TempData["Success"] = "تم إنشاء المادة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");

                var gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
                ViewBag.GradeLevels = gradeLevels.ToList();
                return View(createDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل مادة
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _subjectService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var updateDto = new UpdateSubjectDto
                {
                    SubjectName = response.Data.SubjectName,
                    SubjectCode = response.Data.SubjectCode,
                    WeeklyHours = response.Data.WeeklyHours,
                    IsRequired = response.Data.IsRequired,
                    Description = response.Data.Description,
                    GradeLevelId = response.Data.GradeLevelId,
                    IsActive = response.Data.IsActive
                };

                // جلب الصفوف للقائمة المنسدلة
                var gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
                ViewBag.GradeLevels = gradeLevels.ToList();

                ViewBag.Id = id;

                return View(updateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للمادة {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث مادة (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id, UpdateSubjectDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                var gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
                ViewBag.GradeLevels = gradeLevels.ToList();
                ViewBag.Id = id;
                return View(updateDto);
            }

            try
            {
                var response = await _subjectService.UpdateAsync(id, updateDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث المادة");

                    var gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
                    ViewBag.GradeLevels = gradeLevels.ToList();
                    ViewBag.Id = id;
                    return View(updateDto);
                }

                TempData["Success"] = "تم تحديث المادة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للمادة {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");

                var gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
                ViewBag.GradeLevels = gradeLevels.ToList();
                ViewBag.Id = id;
                return View(updateDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Delete ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف مادة (Soft Delete)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _subjectService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف المادة";
                }
                else
                {
                    TempData["Success"] = "تم حذف المادة بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Delete للمادة {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion



        /// <summary>
        /// 🔄 تفعيل / إلغاء تفعيل المادة
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            try
            {
                var response = await _subjectService.ToggleActiveAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ";
                }
                else
                {
                    TempData["Success"] = "تم تغيير حالة المادة بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في ToggleActive للمادة {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}