using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolERP.Application.DTOs.ClassRooms;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏫  وحدة تحكم الفصول الدراسية (ClassRoomsController)
    /// 📌  الوظيفة: إدارة عمليات الفصول الدراسية (CRUD)
    /// 📦  الاستخدام: نقاط النهاية (Endpoints) للفصول الدراسية
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize]
    public class ClassRoomsController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IClassRoomService _classRoomService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ClassRoomsController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public ClassRoomsController(
            IClassRoomService classRoomService,
            IUnitOfWork unitOfWork,
            ILogger<ClassRoomsController> logger)
        {
            _classRoomService = classRoomService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع الفصول الدراسية
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _classRoomService.GetAllAsync();
                if (!response.Success || response.Data == null)
                {
                    _logger.LogError("خطأ في جلب الفصول: {Message}", response.Message);
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
        /// 🔍 عرض تفاصيل فصل دراسي
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _classRoomService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Details للفصل {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة إنشاء فصل جديد
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create()
        {
            // جلب الصفوف للقائمة المنسدلة
            var gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
            ViewBag.GradeLevels = gradeLevels.ToList();

            // جلب المعلمين للقائمة المنسدلة
            var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
            ViewBag.Teachers = teachers.ToList();

            return View(new CreateClassRoomDto());
        }

        /// <summary>
        /// ➕ إنشاء فصل جديد (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create(CreateClassRoomDto createDto)
        {
            if (!ModelState.IsValid)
            {
                var gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
                ViewBag.GradeLevels = gradeLevels.ToList();

                var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
                ViewBag.Teachers = teachers.ToList();

                return View(createDto);
            }

            try
            {
                var response = await _classRoomService.CreateAsync(createDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إنشاء الفصل");

                    var gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
                    ViewBag.GradeLevels = gradeLevels.ToList();

                    var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
                    ViewBag.Teachers = teachers.ToList();

                    return View(createDto);
                }

                TempData["Success"] = "تم إنشاء الفصل بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");

                var gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
                ViewBag.GradeLevels = gradeLevels.ToList();

                var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
                ViewBag.Teachers = teachers.ToList();

                return View(createDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل فصل
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _classRoomService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var updateDto = new UpdateClassRoomDto
                {
                    ClassName = response.Data.ClassName,
                    ClassCode = response.Data.ClassCode,
                    RoomNumber = response.Data.RoomNumber,
                    Capacity = response.Data.Capacity,
                    HasSmartBoard = response.Data.HasSmartBoard,
                    HasProjector = response.Data.HasProjector,
                    GradeLevelId = response.Data.GradeLevelId,
                    TeacherId = response.Data.TeacherId,
                    Notes = response.Data.Notes,
                    IsActive = response.Data.IsActive
                };

                // جلب الصفوف للقائمة المنسدلة
                var gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
                ViewBag.GradeLevels = gradeLevels.ToList();

                // جلب المعلمين للقائمة المنسدلة
                var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
                ViewBag.Teachers = teachers.ToList();

                ViewBag.Id = id;

                return View(updateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للفصل {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث فصل (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id, UpdateClassRoomDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                var gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
                ViewBag.GradeLevels = gradeLevels.ToList();

                var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
                ViewBag.Teachers = teachers.ToList();

                ViewBag.Id = id;
                return View(updateDto);
            }

            try
            {
                var response = await _classRoomService.UpdateAsync(id, updateDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث الفصل");

                    var gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
                    ViewBag.GradeLevels = gradeLevels.ToList();

                    var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
                    ViewBag.Teachers = teachers.ToList();

                    ViewBag.Id = id;
                    return View(updateDto);
                }

                TempData["Success"] = "تم تحديث الفصل بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للفصل {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");

                var gradeLevels = await _unitOfWork.GradeLevels.GetAllAsync();
                ViewBag.GradeLevels = gradeLevels.ToList();

                var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
                ViewBag.Teachers = teachers.ToList();

                ViewBag.Id = id;
                return View(updateDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Delete ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف فصل (Soft Delete)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _classRoomService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف الفصل";
                }
                else
                {
                    TempData["Success"] = "تم حذف الفصل بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Delete للفصل {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}