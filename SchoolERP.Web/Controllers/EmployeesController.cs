using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Employees;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 👨‍💼  وحدة تحكم الموظفين (EmployeesController)
    /// 📌  الوظيفة: إدارة عمليات الموظفين (CRUD)
    /// 📦  الاستخدام: نقاط النهاية (Endpoints) للموظفين
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize]
    public class EmployeesController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IEmployeeService _employeeService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EmployeesController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public EmployeesController(
            IEmployeeService employeeService,
            IUnitOfWork unitOfWork,
            ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع الموظفين
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _employeeService.GetAllAsync();
                if (!response.Success || response.Data == null)
                {
                    _logger.LogError("خطأ في جلب الموظفين: {Message}", response.Message);
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
        /// 🔍 عرض تفاصيل موظف
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _employeeService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Details للموظف {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة إنشاء موظف جديد
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create()
        {
            // جلب المستخدمين للقائمة المنسدلة
            var users = await _unitOfWork.Users.GetAllAsync();
            ViewBag.Users = users.ToList();

            return View(new CreateEmployeeDto());
        }

        /// <summary>
        /// ➕ إنشاء موظف جديد (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create(CreateEmployeeDto createDto)
        {
            if (!ModelState.IsValid)
            {
                var users = await _unitOfWork.Users.GetAllAsync();
                ViewBag.Users = users.ToList();
                return View(createDto);
            }

            try
            {
                var response = await _employeeService.CreateAsync(createDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إنشاء الموظف");
                    var users = await _unitOfWork.Users.GetAllAsync();
                    ViewBag.Users = users.ToList();
                    return View(createDto);
                }

                TempData["Success"] = "تم إنشاء الموظف بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                var users = await _unitOfWork.Users.GetAllAsync();
                ViewBag.Users = users.ToList();
                return View(createDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل موظف
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _employeeService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var updateDto = new UpdateEmployeeDto
                {
                    JobTitle = response.Data.JobTitle,
                    Department = response.Data.Department,
                    Salary = response.Data.Salary,
                    IsActive = response.Data.IsActive
                };

                ViewBag.Id = id;
                return View(updateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للموظف {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث موظف (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id, UpdateEmployeeDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(updateDto);
            }

            try
            {
                var response = await _employeeService.UpdateAsync(id, updateDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث الموظف");
                    ViewBag.Id = id;
                    return View(updateDto);
                }

                TempData["Success"] = "تم تحديث الموظف بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للموظف {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                ViewBag.Id = id;
                return View(updateDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Delete ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف موظف (Soft Delete)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _employeeService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف الموظف";
                }
                else
                {
                    TempData["Success"] = "تم حذف الموظف بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Delete للموظف {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}