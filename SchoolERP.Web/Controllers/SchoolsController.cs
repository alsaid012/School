using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Schools;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏫  وحدة تحكم المدارس (SchoolsController)
    /// 📌  الوظيفة: إدارة عمليات المدارس (CRUD)
    /// 📦  الاستخدام: نقاط النهاية (Endpoints) للمدارس
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize]
    public class SchoolsController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly ISchoolService _schoolService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SchoolsController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public SchoolsController(
            ISchoolService schoolService,
            IUnitOfWork unitOfWork,
            ILogger<SchoolsController> logger)
        {
            _schoolService = schoolService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع المدارس
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _schoolService.GetAllAsync();
                if (!response.Success || response.Data == null)
                {
                    _logger.LogError("خطأ في جلب المدارس: {Message}", response.Message);
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
        /// 🔍 عرض تفاصيل مدرسة
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _schoolService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Details للمدرسة {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة إنشاء مدرسة جديدة
        /// </summary>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            // جلب الإدارات للقائمة المنسدلة
            var departments = await _unitOfWork.Departments.GetAllAsync();
            ViewBag.Departments = departments.ToList();
            return View(new CreateSchoolDto());
        }

        /// <summary>
        /// ➕ إنشاء مدرسة جديدة (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateSchoolDto createDto)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _unitOfWork.Departments.GetAllAsync();
                ViewBag.Departments = departments.ToList();
                return View(createDto);
            }

            try
            {
                var response = await _schoolService.CreateAsync(createDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إنشاء المدرسة");
                    var departments = await _unitOfWork.Departments.GetAllAsync();
                    ViewBag.Departments = departments.ToList();
                    return View(createDto);
                }

                TempData["Success"] = "تم إنشاء المدرسة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                var departments = await _unitOfWork.Departments.GetAllAsync();
                ViewBag.Departments = departments.ToList();
                return View(createDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل مدرسة
        /// </summary>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _schoolService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var updateDto = new UpdateSchoolDto
                {
                    SchoolName = response.Data.SchoolName,
                    SchoolCode = response.Data.SchoolCode,
                    SchoolType = response.Data.SchoolType,
                    Address = response.Data.Address,
                    Phone = response.Data.Phone,
                    Email = response.Data.Email,
                    PrincipalName = response.Data.PrincipalName,
                    EstablishedYear = response.Data.EstablishedYear,
                    DepartmentId = response.Data.DepartmentId,
                    IsActive = response.Data.IsActive
                };

                // جلب الإدارات للقائمة المنسدلة
                var departments = await _unitOfWork.Departments.GetAllAsync();
                ViewBag.Departments = departments.ToList();
                ViewBag.Id = id;

                return View(updateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للمدرسة {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث مدرسة (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, UpdateSchoolDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _unitOfWork.Departments.GetAllAsync();
                ViewBag.Departments = departments.ToList();
                ViewBag.Id = id;
                return View(updateDto);
            }

            try
            {
                var response = await _schoolService.UpdateAsync(id, updateDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث المدرسة");
                    var departments = await _unitOfWork.Departments.GetAllAsync();
                    ViewBag.Departments = departments.ToList();
                    ViewBag.Id = id;
                    return View(updateDto);
                }

                TempData["Success"] = "تم تحديث المدرسة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للمدرسة {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                var departments = await _unitOfWork.Departments.GetAllAsync();
                ViewBag.Departments = departments.ToList();
                ViewBag.Id = id;
                return View(updateDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Delete ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف مدرسة (Soft Delete)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _schoolService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف المدرسة";
                }
                else
                {
                    TempData["Success"] = "تم حذف المدرسة بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Delete للمدرسة {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}