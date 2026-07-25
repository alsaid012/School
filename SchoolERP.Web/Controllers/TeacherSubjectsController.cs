using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.TeacherSubjects;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🔗  وحدة تحكم ربط المعلم بالمادة (TeacherSubjectsController)
    /// 📌  الوظيفة: إدارة عمليات ربط المعلم بالمادة (CRUD)
    /// 📦  الاستخدام: نقاط النهاية (Endpoints) لربط المعلم بالمادة
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize]
    public class TeacherSubjectsController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly ITeacherSubjectService _teacherSubjectService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TeacherSubjectsController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public TeacherSubjectsController(
            ITeacherSubjectService teacherSubjectService,
            IUnitOfWork unitOfWork,
            ILogger<TeacherSubjectsController> logger)
        {
            _teacherSubjectService = teacherSubjectService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع الروابط بين المعلمين والمواد
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _teacherSubjectService.GetAllAsync();
                if (!response.Success || response.Data == null)
                {
                    _logger.LogError("خطأ في جلب الروابط: {Message}", response.Message);
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
        /// 🔍 عرض تفاصيل رابط
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _teacherSubjectService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Details للرابط {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة إنشاء رابط جديد
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create()
        {
            // جلب المعلمين للقائمة المنسدلة
            var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
            ViewBag.Teachers = teachers.ToList();

            // جلب المواد للقائمة المنسدلة
            var subjects = await _unitOfWork.Subjects.GetAllAsync();
            ViewBag.Subjects = subjects.ToList();

            return View(new CreateTeacherSubjectDto());
        }

        /// <summary>
        /// ➕ إنشاء رابط جديد (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create(CreateTeacherSubjectDto createDto)
        {
            if (!ModelState.IsValid)
            {
                // ✅ جلب المعلمين مع User
                var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();

                // ✅ إنشاء SelectList مع اسم المعلم
                var teacherList = teachers.Select(t => new
                {
                    Id = t.Id,
                    Name = t.User?.FullName ?? t.TeacherCode
                }).ToList();


                ViewBag.Teachers = teachers.ToList();

                // جلب المواد
                var subjects = await _unitOfWork.Subjects.GetAllAsync();
                ViewBag.Subjects = subjects.ToList();

                return View(new CreateTeacherSubjectDto());
            }

            try
            {
                var response = await _teacherSubjectService.CreateAsync(createDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إنشاء الرابط");

                    var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
                    ViewBag.Teachers = teachers.ToList();

                    var subjects = await _unitOfWork.Subjects.GetAllAsync();
                    ViewBag.Subjects = subjects.ToList();

                    return View(createDto);
                }

                TempData["Success"] = "تم إنشاء الرابط بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");

                var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
                ViewBag.Teachers = teachers.ToList();

                var subjects = await _unitOfWork.Subjects.GetAllAsync();
                ViewBag.Subjects = subjects.ToList();

                return View(createDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════
        ///// <summary>
        ///// ✏️ عرض صفحة تعديل رابط
        ///// </summary>
        //[Authorize(Roles = "Admin,Principal")]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    try
        //    {
        //        var response = await _teacherSubjectService.GetByIdAsync(id);
        //        if (!response.Success || response.Data == null)
        //        {
        //            return NotFound();
        //        }

        //        // ✅ إضافة البيانات إلى ViewBag للعرض
        //        ViewBag.TeacherName = response.Data.TeacherName;
        //        ViewBag.SubjectName = response.Data.SubjectName;
        //        ViewBag.GradeLevelName = response.Data.GradeLevelName;

        //        var updateDto = new UpdateTeacherSubjectDto
        //        {
        //            IsPrimary = response.Data.IsPrimary,
        //            IsActive = response.Data.IsActive
        //        };

        //        ViewBag.Id = id;
        //        return View(updateDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "خطأ في Edit للرابط {Id}", id);
        //        return View("Error");
        //    }
        //}

        /// <summary>
        /// ✏️ عرض صفحة تعديل رابط
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                // ✅ جلب الرابط مع جميع البيانات
                var response = await _teacherSubjectService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var data = response.Data;

                // ✅ إنشاء UpdateDto مع جميع البيانات
                var updateDto = new UpdateTeacherSubjectDto
                {
                    TeacherId = data.TeacherId,
                    SubjectId = data.SubjectId,
                    IsPrimary = data.IsPrimary,
                    IsActive = data.IsActive,
                    WeeklyHours = data.WeeklyHours,
                    TeacherName = data.TeacherName,
                    SubjectName = data.SubjectName,
                    GradeLevelName = data.GradeLevelName
                };

                // ✅ تخزين البيانات في ViewBag
                ViewBag.Id = id;
                ViewBag.TeacherName = data.TeacherName;
                ViewBag.SubjectName = data.SubjectName;
                ViewBag.GradeLevelName = data.GradeLevelName;
                ViewBag.TeacherId = data.TeacherId;
                ViewBag.SubjectId = data.SubjectId;

                return View(updateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للرابط {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث رابط (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id, UpdateTeacherSubjectDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                // ✅ في حالة الخطأ، نرجع البيانات تاني
                var response = await _teacherSubjectService.GetByIdAsync(id);
                if (response.Success && response.Data != null)
                {
                    ViewBag.TeacherName = response.Data.TeacherName;
                    ViewBag.SubjectName = response.Data.SubjectName;
                    ViewBag.GradeLevelName = response.Data.GradeLevelName;
                }
                ViewBag.Id = id;
                return View(updateDto);
            }

            try
            {
                var response = await _teacherSubjectService.UpdateAsync(id, updateDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث الرابط");

                    // ✅ نرجع البيانات تاني
                    var getResponse = await _teacherSubjectService.GetByIdAsync(id);
                    if (getResponse.Success && getResponse.Data != null)
                    {
                        ViewBag.TeacherName = getResponse.Data.TeacherName;
                        ViewBag.SubjectName = getResponse.Data.SubjectName;
                        ViewBag.GradeLevelName = getResponse.Data.GradeLevelName;
                    }
                    ViewBag.Id = id;
                    return View(updateDto);
                }

                TempData["Success"] = "تم تحديث الرابط بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للرابط {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");

                // ✅ نرجع البيانات تاني
                var getResponse = await _teacherSubjectService.GetByIdAsync(id);
                if (getResponse.Success && getResponse.Data != null)
                {
                    ViewBag.TeacherName = getResponse.Data.TeacherName;
                    ViewBag.SubjectName = getResponse.Data.SubjectName;
                    ViewBag.GradeLevelName = getResponse.Data.GradeLevelName;
                }
                ViewBag.Id = id;
                return View(updateDto);
            }
        }
        ///// <summary>
        ///// ✏️ عرض صفحة تعديل رابط
        ///// </summary>
        //[Authorize(Roles = "Admin,Principal")]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    try
        //    {
        //        var response = await _teacherSubjectService.GetByIdAsync(id);
        //        if (!response.Success || response.Data == null)
        //        {
        //            return NotFound();
        //        }

        //        var updateDto = new UpdateTeacherSubjectDto
        //        {
        //            IsPrimary = response.Data.IsPrimary,
        //            IsActive = response.Data.IsActive
        //        };

        //        ViewBag.Id = id;
        //        return View(updateDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "خطأ في Edit للرابط {Id}", id);
        //        return View("Error");
        //    }
        //}

        ///// <summary>
        ///// ✏️ تحديث رابط (POST)
        ///// </summary>
        //[HttpPost]
        //[Authorize(Roles = "Admin,Principal")]
        //public async Task<IActionResult> Edit(int id, UpdateTeacherSubjectDto updateDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ViewBag.Id = id;
        //        return View(updateDto);
        //    }

        //    try
        //    {
        //        var response = await _teacherSubjectService.UpdateAsync(id, updateDto);
        //        if (!response.Success)
        //        {
        //            ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث الرابط");
        //            ViewBag.Id = id;
        //            return View(updateDto);
        //        }

        //        TempData["Success"] = "تم تحديث الرابط بنجاح";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "خطأ في Edit للرابط {Id}", id);
        //        ModelState.AddModelError("", "حدث خطأ غير متوقع");
        //        ViewBag.Id = id;
        //        return View(updateDto);
        //    }
        //}

        #endregion

        #region ════════════════════════════════════ Delete ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف رابط
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _teacherSubjectService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف الرابط";
                }
                else
                {
                    TempData["Success"] = "تم حذف الرابط بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Delete للرابط {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}