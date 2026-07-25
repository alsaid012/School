using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Teachers;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 👨‍🏫  وحدة تحكم المعلمين (TeachersController)
    /// 📌  الوظيفة: إدارة عمليات المعلمين (CRUD)
    /// 📦  الاستخدام: نقاط النهاية (Endpoints) للمعلمين
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize]
    public class TeachersController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly ITeacherService _teacherService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TeachersController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public TeachersController(
            ITeacherService teacherService,
            IUnitOfWork unitOfWork,
            ILogger<TeachersController> logger)
        {
            _teacherService = teacherService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع المعلمين
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _teacherService.GetAllAsync();
                if (!response.Success || response.Data == null)
                {
                    _logger.LogError("خطأ في جلب المعلمين: {Message}", response.Message);
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
        /// 🔍 عرض تفاصيل معلم
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _teacherService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Details للمعلم {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة إنشاء معلم جديد
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create()
        {
            // جلب المستخدمين للقائمة المنسدلة
            var users = await _unitOfWork.Users.GetAllAsync();
            ViewBag.Users = users.ToList();

            // جلب المواد للقائمة المنسدلة
            var subjects = await _unitOfWork.Subjects.GetAllAsync();
            ViewBag.Subjects = subjects.ToList();

            return View(new CreateTeacherDto());
        }

        /// <summary>
        /// ➕ إنشاء معلم جديد (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create(CreateTeacherDto createDto)
        {
            if (!ModelState.IsValid)
            {
                var users = await _unitOfWork.Users.GetAllAsync();
                ViewBag.Users = users.ToList();

                var subjects = await _unitOfWork.Subjects.GetAllAsync();
                ViewBag.Subjects = subjects.ToList();

                return View(createDto);
            }

            try
            {
                var response = await _teacherService.CreateAsync(createDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إنشاء المعلم");

                    var users = await _unitOfWork.Users.GetAllAsync();
                    ViewBag.Users = users.ToList();

                    var subjects = await _unitOfWork.Subjects.GetAllAsync();
                    ViewBag.Subjects = subjects.ToList();

                    return View(createDto);
                }

                TempData["Success"] = "تم إنشاء المعلم بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");

                var users = await _unitOfWork.Users.GetAllAsync();
                ViewBag.Users = users.ToList();

                var subjects = await _unitOfWork.Subjects.GetAllAsync();
                ViewBag.Subjects = subjects.ToList();

                return View(createDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل معلم
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _teacherService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var updateDto = new UpdateTeacherDto
                {
                    Qualification = response.Data.Qualification,
                    Specialization = response.Data.Specialization,
                    Salary = response.Data.Salary,
                    IsHomeroomTeacher = response.Data.IsHomeroomTeacher,
                    IsActive = response.Data.IsActive
                };

                // جلب المواد التي يدرسها
                var teacherSubjects = await _unitOfWork.TeacherSubjects
                    .FindAsync(ts => ts.TeacherId == id);
                updateDto.SubjectIds = teacherSubjects.Select(ts => ts.SubjectId).ToList();

                // جلب المواد للقائمة المنسدلة
                var subjects = await _unitOfWork.Subjects.GetAllAsync();
                ViewBag.Subjects = subjects.ToList();

                // جلب المواد المختارة
                ViewBag.SelectedSubjectIds = updateDto.SubjectIds;

                ViewBag.Id = id;

                return View(updateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للمعلم {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث معلم (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id, UpdateTeacherDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                var subjects = await _unitOfWork.Subjects.GetAllAsync();
                ViewBag.Subjects = subjects.ToList();
                ViewBag.SelectedSubjectIds = updateDto.SubjectIds;
                ViewBag.Id = id;
                return View(updateDto);
            }

            try
            {
                var response = await _teacherService.UpdateAsync(id, updateDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث المعلم");

                    var subjects = await _unitOfWork.Subjects.GetAllAsync();
                    ViewBag.Subjects = subjects.ToList();
                    ViewBag.SelectedSubjectIds = updateDto.SubjectIds;
                    ViewBag.Id = id;
                    return View(updateDto);
                }

                TempData["Success"] = "تم تحديث المعلم بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للمعلم {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");

                var subjects = await _unitOfWork.Subjects.GetAllAsync();
                ViewBag.Subjects = subjects.ToList();
                ViewBag.SelectedSubjectIds = updateDto.SubjectIds;
                ViewBag.Id = id;
                return View(updateDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Delete ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف معلم (Soft Delete)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _teacherService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف المعلم";
                }
                else
                {
                    TempData["Success"] = "تم حذف المعلم بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Delete للمعلم {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}