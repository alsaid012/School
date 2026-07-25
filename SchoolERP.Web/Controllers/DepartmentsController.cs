using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Departments;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏢  وحدة تحكم الإدارات التعليمية (DepartmentsController)
    /// 📌  الوظيفة: إدارة عمليات الإدارات التعليمية (CRUD)
    /// 📦  الاستخدام: نقاط النهاية (Endpoints) للإدارات
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize]
    public class DepartmentsController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IDepartmentService _departmentService;
        private readonly IUnitOfWork _unitOfWork;  // ✅ إضافة
        private readonly ILogger<DepartmentsController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public DepartmentsController(
            IDepartmentService departmentService,
            IUnitOfWork unitOfWork,  // ✅ إضافة
            ILogger<DepartmentsController> logger)
        {
            _departmentService = departmentService;
            _unitOfWork = unitOfWork;  // ✅ إضافة
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع الإدارات
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _departmentService.GetAllAsync();
                if (!response.Success || response.Data == null)
                {
                    _logger.LogError("خطأ في جلب الإدارات: {Message}", response.Message);
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
        /// 🔍 عرض تفاصيل إدارة
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _departmentService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Details للإدارة {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة إنشاء إدارة جديدة
        /// </summary>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            // ✅ جلب المحافظات للقائمة المنسدلة
            var governorates = await _unitOfWork.Governorates.GetAllAsync();
            ViewBag.Governorates = governorates.ToList();
            return View(new CreateDepartmentDto());
        }

        /// <summary>
        /// ➕ إنشاء إدارة جديدة (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateDepartmentDto createDto)
        {
            if (!ModelState.IsValid)
            {
                var governorates = await _unitOfWork.Governorates.GetAllAsync();
                ViewBag.Governorates = governorates.ToList();
                return View(createDto);
            }

            try
            {
                var response = await _departmentService.CreateAsync(createDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إنشاء الإدارة");
                    var governorates = await _unitOfWork.Governorates.GetAllAsync();
                    ViewBag.Governorates = governorates.ToList();
                    return View(createDto);
                }

                TempData["Success"] = "تم إنشاء الإدارة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                var governorates = await _unitOfWork.Governorates.GetAllAsync();
                ViewBag.Governorates = governorates.ToList();
                return View(createDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل إدارة
        /// </summary>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _departmentService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var updateDto = new UpdateDepartmentDto
                {
                    Name = response.Data.Name,
                    Code = response.Data.Code,
                    DirectorName = response.Data.DirectorName,
                    Phone = response.Data.Phone,
                    Email = response.Data.Email,
                    Address = response.Data.Address,
                    GovernorateId = response.Data.GovernorateId,
                    IsActive = response.Data.IsActive
                };

                // ✅ جلب المحافظات للقائمة المنسدلة
                var governorates = await _unitOfWork.Governorates.GetAllAsync();
                ViewBag.Governorates = governorates.ToList();
                ViewBag.Id = id;

                return View(updateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للإدارة {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث إدارة (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, UpdateDepartmentDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                var governorates = await _unitOfWork.Governorates.GetAllAsync();
                ViewBag.Governorates = governorates.ToList();
                ViewBag.Id = id;
                return View(updateDto);
            }

            try
            {
                var response = await _departmentService.UpdateAsync(id, updateDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث الإدارة");
                    var governorates = await _unitOfWork.Governorates.GetAllAsync();
                    ViewBag.Governorates = governorates.ToList();
                    ViewBag.Id = id;
                    return View(updateDto);
                }

                TempData["Success"] = "تم تحديث الإدارة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للإدارة {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                var governorates = await _unitOfWork.Governorates.GetAllAsync();
                ViewBag.Governorates = governorates.ToList();
                ViewBag.Id = id;
                return View(updateDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Delete ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف إدارة (Soft Delete)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _departmentService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف الإدارة";
                }
                else
                {
                    TempData["Success"] = "تم حذف الإدارة بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Delete للإدارة {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}