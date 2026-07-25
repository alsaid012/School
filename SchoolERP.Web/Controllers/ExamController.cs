using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.Exams;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Enums;
using SchoolERP.Web.ViewModels.Exams;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📝  وحدة تحكم الامتحانات (ExamsController)
    /// 📌  الوظيفة: إدارة عمليات الامتحانات (CRUD + عمليات إضافية)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize]
    public class ExamsController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IExamService _examService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExamsController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public ExamsController(
            IExamService examService,
            IUnitOfWork unitOfWork,
            ILogger<ExamsController> logger)
        {
            _examService = examService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع الامتحانات
        /// </summary>
        public async Task<IActionResult> Index(int? academicYearId = null)
        {
            try
            {
                var viewModel = new ExamIndexViewModel
                {
                    SelectedAcademicYearId = academicYearId
                };

                // ✅ جلب السنوات الدراسية للفلترة
                var academicYears = await _unitOfWork.AcademicYears
                    .FindAsync(ay => ay.IsActive);
                viewModel.AcademicYears = academicYears.Select(ay => new SelectListItem
                {
                    Value = ay.Id.ToString(),
                    Text = ay.YearName,
                    Selected = ay.Id == academicYearId
                }).ToList();

                // ✅ جلب الامتحانات
                var response = await _examService.GetAllAsync();
                if (response.Success && response.Data != null)
                {
                    viewModel.Exams = response.Data.ToList();

                    if (academicYearId.HasValue)
                    {
                        viewModel.Exams = viewModel.Exams
                            .Where(e => e.AcademicYearId == academicYearId.Value)
                            .ToList();
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
        /// 🔍 عرض تفاصيل امتحان
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _examService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Details للامتحان {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة إنشاء امتحان جديد
        /// </summary>
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> Create()
        {
            var viewModel = await PrepareCreateViewModelAsync();
            return View(viewModel);
        }

        /// <summary>
        /// ➕ إنشاء امتحان جديد (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> Create(ExamCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await PrepareCreateViewModelAsync(viewModel.Exam);
                return View(viewModel);
            }

            try
            {
                var response = await _examService.CreateAsync(viewModel.Exam);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إنشاء الامتحان");
                    viewModel = await PrepareCreateViewModelAsync(viewModel.Exam);
                    return View(viewModel);
                }

                TempData["Success"] = "✅ تم إنشاء الامتحان بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                viewModel = await PrepareCreateViewModelAsync(viewModel.Exam);
                return View(viewModel);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل امتحان
        /// </summary>
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _examService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var data = response.Data;

                var viewModel = new ExamEditViewModel
                {
                    Id = id,
                    Exam = new UpdateExamDto
                    {
                        ExamName = data.ExamName,
                        ExamType = data.ExamType,
                        ExamDate = data.ExamDate,
                        StartTime = data.StartTime,
                        EndTime = data.EndTime,
                        MaxScore = data.MaxScore,
                        AcademicYearId = data.AcademicYearId,
                        SubjectId = data.SubjectId,
                        ClassRoomId = data.ClassRoomId,
                        TeacherId = data.TeacherId,
                        Notes = data.Notes,
                        IsActive = data.IsActive
                    },
                    DisplayInfo = new ExamDisplayInfo
                    {
                        ExamName = data.ExamName ?? string.Empty,
                        ExamTypeName = data.ExamTypeName ?? string.Empty,
                        SubjectName = data.SubjectName ?? string.Empty,
                        TeacherName = data.TeacherName ?? string.Empty,
                        ClassRoomName = data.ClassRoomName ?? string.Empty,
                        AcademicYearName = data.AcademicYearName ?? string.Empty,
                        StudentsCount = data.StudentsCount,
                        AverageScore = data.AverageScore
                    }
                };

                viewModel = await PrepareEditViewModelAsync(viewModel);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Edit للامتحان {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث امتحان (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> Edit(int id, ExamEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Id = id;
                viewModel = await PrepareEditViewModelAsync(viewModel);
                return View(viewModel);
            }

            try
            {
                var response = await _examService.UpdateAsync(id, viewModel.Exam);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث الامتحان");
                    viewModel.Id = id;
                    viewModel = await PrepareEditViewModelAsync(viewModel);
                    return View(viewModel);
                }

                TempData["Success"] = "✅ تم تحديث الامتحان بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Edit للامتحان {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                viewModel.Id = id;
                viewModel = await PrepareEditViewModelAsync(viewModel);
                return View(viewModel);
            }
        }

        #endregion

        #region ════════════════════════════════════ Delete ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف امتحان
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _examService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف الامتحان";
                }
                else
                {
                    TempData["Success"] = "✅ تم حذف الامتحان بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Delete للامتحان {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        /// <summary>
        /// 🔄 تجهيز ViewModel للإنشاء
        /// </summary>
        private async Task<ExamCreateViewModel> PrepareCreateViewModelAsync(CreateExamDto? selected = null)
        {
            var viewModel = new ExamCreateViewModel();

            if (selected != null)
            {
                viewModel.Exam = selected;
            }

            // ✅ السنوات الدراسية
            var academicYears = await _unitOfWork.AcademicYears
                .FindAsync(ay => ay.IsActive);
            viewModel.AcademicYears = academicYears.Select(ay => new SelectListItem
            {
                Value = ay.Id.ToString(),
                Text = ay.YearName ?? string.Empty,
                Selected = selected != null && ay.Id == selected.AcademicYearId
            }).ToList();

            // ✅ المواد الدراسية
            var subjects = await _unitOfWork.Subjects
                .FindAsync(s => s.IsActive);
            viewModel.Subjects = subjects.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.SubjectName ?? string.Empty,
                Selected = selected != null && s.Id == selected.SubjectId
            }).ToList();

            // ✅ الفصول الدراسية
            var classRooms = await _unitOfWork.ClassRooms
                .FindAsync(cr => cr.IsActive);
            viewModel.ClassRooms = classRooms.Select(cr => new SelectListItem
            {
                Value = cr.Id.ToString(),
                Text = cr.ClassName ?? string.Empty,
                Selected = selected != null && cr.Id == selected.ClassRoomId
            }).ToList();

            // ✅ المعلمين
            var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
            viewModel.Teachers = teachers.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.User?.FullName ?? t.TeacherCode ?? string.Empty,
                Selected = selected != null && t.Id == selected.TeacherId
            }).ToList();

            // ✅ أنواع الامتحانات
            viewModel.ExamTypes = Enum.GetValues(typeof(ExamType))
                .Cast<ExamType>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = GetExamTypeName(e),
                    Selected = selected != null && e == selected.ExamType
                }).ToList();

            return viewModel;
        }

        /// <summary>
        /// 🔄 تجهيز ViewModel للتعديل
        /// </summary>
        private async Task<ExamEditViewModel> PrepareEditViewModelAsync(ExamEditViewModel viewModel)
        {
            // ✅ السنوات الدراسية
            var academicYears = await _unitOfWork.AcademicYears
                .FindAsync(ay => ay.IsActive);
            viewModel.AcademicYears = academicYears.Select(ay => new SelectListItem
            {
                Value = ay.Id.ToString(),
                Text = ay.YearName ?? string.Empty,
                Selected = ay.Id == viewModel.Exam.AcademicYearId
            }).ToList();

            // ✅ المواد الدراسية
            var subjects = await _unitOfWork.Subjects
                .FindAsync(s => s.IsActive);
            viewModel.Subjects = subjects.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.SubjectName ?? string.Empty,
                Selected = s.Id == viewModel.Exam.SubjectId
            }).ToList();

            // ✅ الفصول الدراسية
            var classRooms = await _unitOfWork.ClassRooms
                .FindAsync(cr => cr.IsActive);
            viewModel.ClassRooms = classRooms.Select(cr => new SelectListItem
            {
                Value = cr.Id.ToString(),
                Text = cr.ClassName ?? string.Empty,
                Selected = cr.Id == viewModel.Exam.ClassRoomId
            }).ToList();

            // ✅ المعلمين
            var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();
            viewModel.Teachers = teachers.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.User?.FullName ?? t.TeacherCode ?? string.Empty,
                Selected = t.Id == viewModel.Exam.TeacherId
            }).ToList();

            // ✅ أنواع الامتحانات
            viewModel.ExamTypes = Enum.GetValues(typeof(ExamType))
                .Cast<ExamType>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = GetExamTypeName(e),
                    Selected = e == viewModel.Exam.ExamType
                }).ToList();

            return viewModel;
        }

        /// <summary>
        /// 📝 الحصول على اسم نوع الامتحان بالعربية
        /// </summary>
        private string GetExamTypeName(ExamType examType)
        {
            return examType switch
            {
                ExamType.Monthly => "شهري",
                ExamType.MidTerm => "نصفي",
                ExamType.Final => "نهائي",
                ExamType.Quiz => "اختبار قصير",
                ExamType.Assessment => "تقييم",
                _ => examType.ToString()
            };
        }

        #endregion
    }
}