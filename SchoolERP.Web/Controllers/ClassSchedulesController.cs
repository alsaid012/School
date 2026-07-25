using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.AcademicYears;
using SchoolERP.Application.DTOs.ClassSchedules;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Web.ViewModels.ClassSchedule;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📅  وحدة تحكم جدول الحصص (ClassSchedulesController)
    /// 📌  الوظيفة: إدارة عمليات جدول الحصص (CRUD + عمليات إضافية)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize]
    public class ClassSchedulesController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IClassScheduleService _classScheduleService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ClassSchedulesController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public ClassSchedulesController(
            IClassScheduleService classScheduleService,
            IUnitOfWork unitOfWork,
            ILogger<ClassSchedulesController> logger)
        {
            _classScheduleService = classScheduleService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع جداول الحصص
        /// </summary>
       
        public async Task<IActionResult> Index(int? academicYearId = null)
        {
            try
            {
                var viewModel = new ClassScheduleIndexViewModel
                {
                    SelectedAcademicYearId = academicYearId
                };

                // ✅ جلب السنوات الدراسية للفلترة
                var academicYears = await _unitOfWork.AcademicYears
                    .FindAsync(ay => ay.IsActive);

                viewModel.AcademicYears = academicYears.Select(ay => new AcademicYearDto
                {
                    Id = ay.Id,
                    YearName = ay.YearName,
                    IsCurrent = ay.IsCurrent
                }).ToList();

                // ✅ جلب الحصص
                var response = await _classScheduleService.GetAllAsync();
                if (response.Success && response.Data != null)
                {
                    var schedules = response.Data.ToList();

                    // ✅ إعادة تعبئة اسم المعلم يدوياً إذا كان فارغاً
                    foreach (var schedule in schedules)
                    {
                        if (string.IsNullOrEmpty(schedule.TeacherName) || schedule.TeacherName == "غير معروف")
                        {
                            var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(schedule.TeacherId);
                            if (teacher != null)
                            {
                                var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
                                schedule.TeacherName = user?.FullName ?? teacher.TeacherCode ?? "غير معروف";
                            }
                        }
                    }

                    viewModel.Schedules = schedules;

                    // ✅ فلترة حسب السنة الدراسية
                    if (academicYearId.HasValue)
                    {
                        viewModel.Schedules = viewModel.Schedules
                            .Where(s => s.AcademicYearId == academicYearId.Value);
                    }
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Index");
                return View("Error");
            }
        }

        public async Task<IActionResult> Index2(int? academicYearId = null)
        {
            try
            {
                var viewModel = new ClassScheduleIndexViewModel
                {
                    SelectedAcademicYearId = academicYearId
                };

                // ✅ جلب السنوات الدراسية للفلترة
                var academicYears = await _unitOfWork.AcademicYears
                    .FindAsync(ay => ay.IsActive);

                viewModel.AcademicYears = academicYears.Select(ay => new AcademicYearDto
                {
                    Id = ay.Id,
                    YearName = ay.YearName,
                    IsCurrent = ay.IsCurrent
                }).ToList();

                // ✅ جلب الحصص
                var response = await _classScheduleService.GetAllAsync();
                if (response.Success && response.Data != null)
                {
                    viewModel.Schedules = response.Data;

                    // ✅ فلترة حسب السنة الدراسية
                    if (academicYearId.HasValue)
                    {
                        viewModel.Schedules = viewModel.Schedules
                            .Where(s => s.AcademicYearId == academicYearId.Value);
                    }
                }

                return View(viewModel);
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
        /// 🔍 عرض تفاصيل جدول حصص
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _classScheduleService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Details للجدول {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة إنشاء جدول حصص جديد
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create()
        {
            var viewModel = await PrepareCreateViewModelAsync();
            return View(viewModel);
        }

        /// <summary>
        /// ➕ إنشاء جدول حصص جديد (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create(ClassScheduleCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await PrepareCreateViewModelAsync(viewModel.ClassSchedule);
                return View(viewModel);
            }

            try
            {
                var response = await _classScheduleService.CreateAsync(viewModel.ClassSchedule);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إنشاء جدول الحصص");
                    viewModel = await PrepareCreateViewModelAsync(viewModel.ClassSchedule);
                    return View(viewModel);
                }

                TempData["Success"] = "✅ تم إنشاء جدول الحصص بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                viewModel = await PrepareCreateViewModelAsync(viewModel.ClassSchedule);
                return View(viewModel);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل جدول حصص
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _classScheduleService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var data = response.Data;

                var viewModel = new ClassScheduleEditViewModel
                {
                    Id = id,
                    ClassSchedule = new UpdateClassScheduleDto
                    {
                        AcademicYearId = data.AcademicYearId,
                        ClassRoomId = data.ClassRoomId,
                        SubjectId = data.SubjectId,
                        TeacherId = data.TeacherId,
                        DayOfWeek = data.DayOfWeek,
                        StartTime = data.StartTime,
                        EndTime = data.EndTime,
                        PeriodNumber = data.PeriodNumber,
                        IsActive = data.IsActive,
                        Notes = data.Notes
                    },
                    DisplayInfo = new ClassScheduleDisplayInfo
                    {
                        TeacherName = data.TeacherName ?? string.Empty,
                        SubjectName = data.SubjectName ?? string.Empty,
                        ClassRoomName = data.ClassRoomName ?? string.Empty,
                        AcademicYearName = data.AcademicYearName ?? string.Empty,
                        GradeLevelName = data.GradeLevelName ?? string.Empty
                    }
                };

                viewModel = await PrepareEditViewModelAsync(viewModel);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Edit للجدول {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث جدول حصص (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Edit(int id, ClassScheduleEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Id = id;
                viewModel = await PrepareEditViewModelAsync(viewModel);
                return View(viewModel);
            }

            try
            {
                var response = await _classScheduleService.UpdateAsync(id, viewModel.ClassSchedule);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث جدول الحصص");
                    viewModel.Id = id;
                    viewModel = await PrepareEditViewModelAsync(viewModel);
                    return View(viewModel);
                }

                TempData["Success"] = "✅ تم تحديث جدول الحصص بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Edit للجدول {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                viewModel.Id = id;
                viewModel = await PrepareEditViewModelAsync(viewModel);
                return View(viewModel);
            }
        }

        #endregion

        #region ════════════════════════════════════ Delete ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف جدول حصص
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _classScheduleService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف جدول الحصص";
                }
                else
                {
                    TempData["Success"] = "✅ تم حذف جدول الحصص بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Delete للجدول {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ════════════════════════════════════ ToggleStatus ════════════════════════════════════

        /// <summary>
        /// ✅ تفعيل/تعطيل جدول حصص
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var response = await _classScheduleService.ToggleStatusAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء تغيير حالة جدول الحصص";
                }
                else
                {
                    TempData["Success"] = response.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في ToggleStatus للجدول {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ════════════════════════════════════ WeeklySchedule ════════════════════════════════════

        /// <summary>
        /// 📋 عرض الجدول الأسبوعي لفصل معين
        /// </summary>
        public async Task<IActionResult> WeeklySchedule(int classRoomId, int? academicYearId = null)
        {
            try
            {
                // ✅ جلب بيانات الفصل
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(classRoomId);
                if (classRoom == null)
                {
                    return NotFound("الفصل غير موجود");
                }

                var viewModel = new ClassScheduleWeeklyViewModel
                {
                    ClassRoomName = classRoom.ClassName,
                    GradeLevelName = classRoom.GradeLevel?.GradeName ?? string.Empty,
                    AcademicYearId = academicYearId
                };

                // ✅ جلب السنوات الدراسية
                var academicYears = await _unitOfWork.AcademicYears
                    .FindAsync(ay => ay.IsActive);
                viewModel.AcademicYears = academicYears.Select(ay => new SelectListItem
                {
                    Value = ay.Id.ToString(),
                    Text = ay.YearName,
                    Selected = ay.Id == academicYearId
                }).ToList();

                // ✅ جلب الجدول الأسبوعي
                var response = await _classScheduleService.GetWeeklyScheduleAsync(classRoomId, academicYearId);
                if (response.Success && response.Data != null)
                {
                    viewModel.WeeklySchedule = response.Data;
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في WeeklySchedule للفصل {ClassRoomId}", classRoomId);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ TeacherSchedule ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جدول حصص معلم معين
        /// </summary>
        public async Task<IActionResult> TeacherSchedule(int teacherId, int? academicYearId = null)
        {
            try
            {
                // ✅ جلب بيانات المعلم
                var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(teacherId);
                if (teacher == null)
                {
                    return NotFound("المعلم غير موجود");
                }

                ViewBag.TeacherName = teacher.User?.FullName ?? "غير معروف";
                ViewBag.AcademicYearId = academicYearId;

                // ✅ جلب حصص المعلم
                var response = await _classScheduleService.GetByTeacherIdAsync(teacherId, academicYearId);
                if (!response.Success || response.Data == null)
                {
                    ViewBag.Error = response.Message ?? "حدث خطأ أثناء جلب جدول المعلم";
                    return View(new List<ClassScheduleDto>());
                }

                // ✅ ترتيب الحصص حسب اليوم والوقت
                var schedules = response.Data
                    .OrderBy(s => s.DayOfWeek)
                    .ThenBy(s => s.StartTime)
                    .ToList();

                return View(schedules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في TeacherSchedule للمعلم {TeacherId}", teacherId);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        /// <summary>
        /// 🔄 تجهيز ViewModel للإنشاء
        /// </summary>
        private async Task<ClassScheduleCreateViewModel> PrepareCreateViewModelAsync(CreateClassScheduleDto? selected = null)
        {
            var viewModel = new ClassScheduleCreateViewModel();

            if (selected != null)
            {
                viewModel.ClassSchedule = selected;
            }

            // ✅ السنوات الدراسية
            var academicYears = await _unitOfWork.AcademicYears
                .FindAsync(ay => ay.IsActive);
            viewModel.AcademicYears = academicYears.Select(ay => new SelectListItem
            {
                Value = ay.Id.ToString(),
                Text = ay.YearName,
                Selected = selected != null && ay.Id == selected.AcademicYearId
            }).ToList();

            // ✅ الفصول الدراسية
            var classRooms = await _unitOfWork.ClassRooms
                .FindAsync(cr => cr.IsActive);
            viewModel.ClassRooms = classRooms.Select(cr => new SelectListItem
            {
                Value = cr.Id.ToString(),
                Text = cr.ClassName,
                Selected = selected != null && cr.Id == selected.ClassRoomId
            }).ToList();

            // ✅ المواد الدراسية
            var subjects = await _unitOfWork.Subjects
                .FindAsync(s => s.IsActive);
            viewModel.Subjects = subjects.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.SubjectName,
                Selected = selected != null && s.Id == selected.SubjectId
            }).ToList();

            // ✅ المعلمين
            var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
            viewModel.Teachers = teachers.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.User?.FullName ?? t.TeacherCode,
                Selected = selected != null && t.Id == selected.TeacherId
            }).ToList();

            // ✅ أيام الأسبوع
            viewModel.DaysOfWeek = GetDaysOfWeekList(selected?.DayOfWeek);

            return viewModel;
        }

        /// <summary>
        /// 🔄 تجهيز ViewModel للتعديل
        /// </summary>
        private async Task<ClassScheduleEditViewModel> PrepareEditViewModelAsync(ClassScheduleEditViewModel viewModel)
        {
            // ✅ السنوات الدراسية
            var academicYears = await _unitOfWork.AcademicYears
                .FindAsync(ay => ay.IsActive);
            viewModel.AcademicYears = academicYears.Select(ay => new SelectListItem
            {
                Value = ay.Id.ToString(),
                Text = ay.YearName,
                Selected = ay.Id == viewModel.ClassSchedule.AcademicYearId
            }).ToList();

            // ✅ الفصول الدراسية
            var classRooms = await _unitOfWork.ClassRooms
                .FindAsync(cr => cr.IsActive);
            viewModel.ClassRooms = classRooms.Select(cr => new SelectListItem
            {
                Value = cr.Id.ToString(),
                Text = cr.ClassName,
                Selected = cr.Id == viewModel.ClassSchedule.ClassRoomId
            }).ToList();

            // ✅ المواد الدراسية
            var subjects = await _unitOfWork.Subjects
                .FindAsync(s => s.IsActive);
            viewModel.Subjects = subjects.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.SubjectName,
                Selected = s.Id == viewModel.ClassSchedule.SubjectId
            }).ToList();

            // ✅ المعلمين
            var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
            viewModel.Teachers = teachers.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.User?.FullName ?? t.TeacherCode,
                Selected = t.Id == viewModel.ClassSchedule.TeacherId
            }).ToList();

            // ✅ أيام الأسبوع
            viewModel.DaysOfWeek = GetDaysOfWeekList(viewModel.ClassSchedule.DayOfWeek);

            return viewModel;
        }

        /// <summary>
        /// 📅 الحصول على قائمة أيام الأسبوع
        /// </summary>
        private List<SelectListItem> GetDaysOfWeekList(DayOfWeek? selectedDay = null)
        {
            var days = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "الأحد" },
                new SelectListItem { Value = "1", Text = "الإثنين" },
                new SelectListItem { Value = "2", Text = "الثلاثاء" },
                new SelectListItem { Value = "3", Text = "الأربعاء" },
                new SelectListItem { Value = "4", Text = "الخميس" },
                new SelectListItem { Value = "5", Text = "الجمعة" },
                new SelectListItem { Value = "6", Text = "السبت" }
            };

            if (selectedDay.HasValue)
            {
                foreach (var item in days)
                {
                    item.Selected = item.Value == ((int)selectedDay.Value).ToString();
                }
            }

            return days;
        }

        #endregion
    }
}