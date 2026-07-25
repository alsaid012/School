using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.ExamResults;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Web.ViewModels.ExamResults;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  وحدة تحكم نتائج الامتحانات (ExamResultsController)
    /// 📌  الوظيفة: إدارة عمليات نتائج الامتحانات (CRUD + عمليات إضافية)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize]
    public class ExamResultsController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IExamResultService _examResultService;
        private readonly IExamService _examService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExamResultsController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public ExamResultsController(
            IExamResultService examResultService,
            IExamService examService,
            IUnitOfWork unitOfWork,
            ILogger<ExamResultsController> logger)
        {
            _examResultService = examResultService;
            _examService = examService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع نتائج الامتحانات
        /// </summary>
        public async Task<IActionResult> Index(int? examId = null)
        {
            try
            {
                var viewModel = new ExamResultIndexViewModel
                {
                    SelectedExamId = examId
                };

                // ✅ جلب الامتحانات للفلترة
                var exams = await _unitOfWork.Exams.GetAllAsync();
                viewModel.Exams = exams.Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = $"{e.ExamName} - {e.ExamDate:yyyy/MM/dd}",
                    Selected = e.Id == examId
                }).ToList();

                // ✅ جلب النتائج
                if (examId.HasValue)
                {
                    var response = await _examResultService.GetByExamIdAsync(examId.Value);
                    if (response.Success && response.Data != null)
                    {
                        viewModel.Results = response.Data.ToList();
                    }
                }
                else
                {
                    var response = await _examResultService.GetAllAsync();
                    if (response.Success && response.Data != null)
                    {
                        viewModel.Results = response.Data.ToList();
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
        /// 🔍 عرض تفاصيل نتيجة
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _examResultService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Details للنتيجة {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة إضافة نتيجة جديدة
        /// </summary>
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> Create(int? examId = null)
        {
            var viewModel = await PrepareCreateViewModelAsync(examId);
            return View(viewModel);
        }

        /// <summary>
        /// ➕ إضافة نتيجة جديدة (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> Create(ExamResultCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await PrepareCreateViewModelAsync(viewModel.ExamResult.ExamId);
                return View(viewModel);
            }

            try
            {
                var response = await _examResultService.CreateAsync(viewModel.ExamResult);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إضافة النتيجة");
                    viewModel = await PrepareCreateViewModelAsync(viewModel.ExamResult.ExamId);
                    return View(viewModel);
                }

                TempData["Success"] = "✅ تم إضافة النتيجة بنجاح";
                return RedirectToAction(nameof(Index), new { examId = viewModel.ExamResult.ExamId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                viewModel = await PrepareCreateViewModelAsync(viewModel.ExamResult.ExamId);
                return View(viewModel);
            }
        }

        #endregion

        #region ════════════════════════════════════ CreateRange (دفعة واحدة) ════════════════════════════════════

        /// <summary>
        /// ➕➕ عرض صفحة إضافة نتائج متعددة
        /// </summary>
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> CreateRange(int? examId = null)
        {
            var viewModel = await PrepareCreateRangeViewModelAsync(examId);
            return View(viewModel);
        }

        /// <summary>
        /// ➕➕ إضافة نتائج متعددة (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> CreateRange(ExamResultCreateRangeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await PrepareCreateRangeViewModelAsync(viewModel.ExamId);
                return View(viewModel);
            }

            try
            {
                // ✅ التحقق من وجود نتائج
                if (viewModel.ExamResults == null || !viewModel.ExamResults.Any())
                {
                    ModelState.AddModelError("", "يرجى إضافة نتائج الطلاب");
                    viewModel = await PrepareCreateRangeViewModelAsync(viewModel.ExamId);
                    return View(viewModel);
                }

                var response = await _examResultService.CreateRangeAsync(viewModel.ExamResults);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إضافة النتائج");
                    viewModel = await PrepareCreateRangeViewModelAsync(viewModel.ExamId);
                    return View(viewModel);
                }

                TempData["Success"] = $"✅ تم إضافة {response.Data?.Count() ?? 0} نتيجة بنجاح";
                return RedirectToAction(nameof(Index), new { examId = viewModel.ExamId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في CreateRange");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                viewModel = await PrepareCreateRangeViewModelAsync(viewModel.ExamId);
                return View(viewModel);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل نتيجة
        /// </summary>
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _examResultService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var data = response.Data;

                var viewModel = new ExamResultEditViewModel
                {
                    Id = id,
                    ExamResult = new UpdateExamResultDto
                    {
                        Score = data.Score,
                        Remarks = data.Remarks,
                        IsActive = data.IsActive
                    },
                    DisplayInfo = new ExamResultDisplayInfo
                    {
                        StudentName = data.StudentName ?? string.Empty,
                        StudentCode = data.StudentCode ?? string.Empty,
                        ExamName = data.ExamName ?? string.Empty,
                        SubjectName = data.SubjectName ?? string.Empty,
                        ClassRoomName = data.ClassRoomName ?? string.Empty,
                        ExamDate = data.ExamDate,
                        MaxScore = data.MaxScore,
                        CurrentScore = data.Score,
                        Percentage = data.Percentage ?? 0,
                        Grade = data.Grade ?? string.Empty,
                        IsPassed = data.IsPassed
                    }
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Edit للنتيجة {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث نتيجة (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> Edit(int id, ExamResultEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Id = id;
                return View(viewModel);
            }

            try
            {
                var response = await _examResultService.UpdateAsync(id, viewModel.ExamResult);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث النتيجة");
                    viewModel.Id = id;
                    return View(viewModel);
                }

                TempData["Success"] = "✅ تم تحديث النتيجة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Edit للنتيجة {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                viewModel.Id = id;
                return View(viewModel);
            }
        }

        #endregion

        #region ════════════════════════════════════ Delete ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف نتيجة
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _examResultService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف النتيجة";
                }
                else
                {
                    TempData["Success"] = "✅ تم حذف النتيجة بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Delete للنتيجة {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ════════════════════════════════════ Statistics ════════════════════════════════════

        /// <summary>
        /// 📊 عرض إحصائيات الامتحان
        /// </summary>
        public async Task<IActionResult> Statistics(int examId)
        {
            try
            {
                var exam = await _unitOfWork.Exams.GetByIdAsync(examId);
                if (exam == null)
                {
                    return NotFound("الامتحان غير موجود");
                }

                var response = await _examResultService.GetStatisticsAsync(examId);
                if (!response.Success || response.Data == null)
                {
                    ViewBag.Error = response.Message ?? "حدث خطأ أثناء جلب الإحصائيات";
                    return View(new ExamResultStatisticsDto());
                }

                ViewBag.ExamName = exam.ExamName;
                ViewBag.ExamDate = exam.ExamDate;
                ViewBag.MaxScore = exam.MaxScore;

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Statistics للامتحان {ExamId}", examId);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        /// <summary>
        /// 🔄 تجهيز ViewModel للإنشاء
        /// </summary>
        private async Task<ExamResultCreateViewModel> PrepareCreateViewModelAsync(int? examId = null)
        {
            var viewModel = new ExamResultCreateViewModel();

            // ✅ جلب الامتحانات
            var exams = await _unitOfWork.Exams.GetAllAsync();
            viewModel.Exams = exams.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = $"{e.ExamName} - {e.ExamDate:yyyy/MM/dd}",
                Selected = e.Id == examId
            }).ToList();

            // ✅ جلب الطلاب
            var students = await _unitOfWork.Students.GetAllAsync();
            viewModel.Students = students.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.User?.FullName ?? s.StudentCode,
                Selected = false
            }).ToList();

            // ✅ إذا كان examId محدد، جلب الدرجة النهائية
            if (examId.HasValue)
            {
                var exam = await _unitOfWork.Exams.GetByIdAsync(examId.Value);
                ViewBag.MaxScore = exam?.MaxScore ?? 100;
                viewModel.ExamResult.ExamId = examId.Value;
            }
            else
            {
                ViewBag.MaxScore = 100;
            }

            return viewModel;
        }

        /// <summary>
        /// 🔄 تجهيز ViewModel لإضافة نتائج متعددة
        /// </summary>
        private async Task<ExamResultCreateRangeViewModel> PrepareCreateRangeViewModelAsync(int? examId = null)
        {
            var viewModel = new ExamResultCreateRangeViewModel
            {
                ExamId = examId ?? 0,
                ExamResults = new List<CreateExamResultDto>()
            };

            // ✅ جلب الامتحانات
            var exams = await _unitOfWork.Exams.GetAllAsync();
            viewModel.Exams = exams.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = $"{e.ExamName} - {e.ExamDate:yyyy/MM/dd}",
                Selected = e.Id == examId
            }).ToList();

            // ✅ إذا كان examId محدد، جلب الطلاب غير المسجلين
            if (examId.HasValue)
            {
                var exam = await _unitOfWork.Exams.GetByIdAsync(examId.Value);
                ViewBag.MaxScore = exam?.MaxScore ?? 100;
                ViewBag.ExamName = exam?.ExamName;

                // ✅ جلب جميع الطلاب في الفصل
                var allStudents = await _unitOfWork.Students.GetAllAsync();
                
                // ✅ جلب الطلاب المسجلين بالفعل
                var existingResults = await _unitOfWork.ExamResults
                    .FindAsync(er => er.ExamId == examId.Value);
                var existingStudentIds = existingResults.Select(r => r.StudentId).ToHashSet();

                // ✅ الطلاب غير المسجلين
                var availableStudents = allStudents
                    .Where(s => !existingStudentIds.Contains(s.Id))
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.User?.FullName ?? s.StudentCode
                    })
                    .ToList();

                viewModel.AvailableStudents = availableStudents;
                viewModel.SelectedStudentIds = new List<int>();
            }

            return viewModel;
        }

        #endregion
    }
}