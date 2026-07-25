using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.StudentAttendances;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Enums;
using SchoolERP.Web.ViewModels.StudentAttendances;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✅  وحدة تحكم حضور الطلاب (StudentAttendancesController)
    /// 📌  الوظيفة: إدارة عمليات حضور الطلاب (CRUD + عمليات إضافية)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize]
    public class StudentAttendancesController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IStudentAttendanceService _attendanceService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StudentAttendancesController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public StudentAttendancesController(
            IStudentAttendanceService attendanceService,
            IUnitOfWork unitOfWork,
            ILogger<StudentAttendancesController> logger)
        {
            _attendanceService = attendanceService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع سجلات حضور الطلاب
        /// </summary>
        public async Task<IActionResult> Index(int? studentId = null, DateTime? date = null)
        {
            try
            {
                var viewModel = new StudentAttendanceIndexViewModel
                {
                    SelectedStudentId = studentId,
                    SelectedDate = date ?? DateTime.Today
                };

                // ✅ جلب الطلاب للفلترة
                var students = await _unitOfWork.Students.GetAllAsync();
                viewModel.Students = students.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.User?.FullName ?? s.StudentCode,
                    Selected = s.Id == studentId
                }).ToList();

                // ✅ جلب سجلات الحضور
                if (studentId.HasValue)
                {
                    var response = await _attendanceService.GetByStudentIdAsync(studentId.Value);
                    if (response.Success && response.Data != null)
                    {
                        viewModel.Attendances = response.Data.ToList();
                    }
                }
                else if (date.HasValue)
                {
                    // ✅ جلب جميع سجلات الحضور في تاريخ محدد
                    var allAttendances = await _attendanceService.GetAllAsync();
                    if (allAttendances.Success && allAttendances.Data != null)
                    {
                        viewModel.Attendances = allAttendances.Data
                            .Where(a => a.AttendanceDate.Date == date.Value.Date)
                            .ToList();
                    }
                }
                else
                {
                    var response = await _attendanceService.GetAllAsync();
                    if (response.Success && response.Data != null)
                    {
                        viewModel.Attendances = response.Data.ToList();
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
        /// 🔍 عرض تفاصيل سجل حضور
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _attendanceService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Details للسجل {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة تسجيل حضور طالب
        /// </summary>
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> Create(int? studentId = null)
        {
            var viewModel = await PrepareCreateViewModelAsync(studentId);
            return View(viewModel);
        }

        /// <summary>
        /// ➕ تسجيل حضور طالب (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> Create(StudentAttendanceCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await PrepareCreateViewModelAsync(viewModel.Attendance.StudentId);
                return View(viewModel);
            }

            try
            {
                var response = await _attendanceService.CreateAsync(viewModel.Attendance);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تسجيل الحضور");
                    viewModel = await PrepareCreateViewModelAsync(viewModel.Attendance.StudentId);
                    return View(viewModel);
                }

                TempData["Success"] = "✅ تم تسجيل الحضور بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                viewModel = await PrepareCreateViewModelAsync(viewModel.Attendance.StudentId);
                return View(viewModel);
            }
        }

        #endregion

        #region ════════════════════════════════════ CreateRange (تسجيل حضور جماعي) ════════════════════════════════════

        /// <summary>
        /// ➕➕ عرض صفحة تسجيل حضور جماعي
        /// </summary>
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> CreateRange(int? classRoomId = null, DateTime? date = null)
        {
            var viewModel = await PrepareCreateRangeViewModelAsync(classRoomId, date);
            return View(viewModel);
        }


        /// <summary>
        /// ➕➕ تسجيل حضور جماعي (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> CreateRange(StudentAttendanceCreateRangeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await PrepareCreateRangeViewModelAsync(viewModel.ClassRoomId, viewModel.Date);
                return View(viewModel);
            }

            try
            {
                var createdCount = 0;
                var errors = new List<string>();

                foreach (var attendanceDto in viewModel.Attendances)
                {
                    var response = await _attendanceService.CreateAsync(attendanceDto);
                    if (response.Success)
                    {
                        createdCount++;
                    }
                    else
                    {
                        errors.Add(response.Message ?? "خطأ");
                    }
                }

                if (createdCount > 0)
                {
                    TempData["Success"] = $"✅ تم تسجيل حضور {createdCount} طالب بنجاح";
                    if (errors.Any())
                    {
                        TempData["Warning"] = $"⚠️ بعض الطلاب لم يتم تسجيلهم: {string.Join("; ", errors)}";
                    }
                }
                else
                {
                    TempData["Error"] = "❌ لم يتم تسجيل أي طالب، تأكد من البيانات";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في CreateRange");
                TempData["Error"] = "حدث خطأ غير متوقع";
                viewModel = await PrepareCreateRangeViewModelAsync(viewModel.ClassRoomId, viewModel.Date);
                return View(viewModel);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل سجل حضور
        /// </summary>
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _attendanceService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var data = response.Data;

                var viewModel = new StudentAttendanceEditViewModel
                {
                    Id = id,
                    Attendance = new UpdateStudentAttendanceDto
                    {
                        CheckInTime = data.CheckInTime,
                        CheckOutTime = data.CheckOutTime,
                        Status = data.Status,
                        DelayMinutes = data.DelayMinutes,
                        Notes = data.Notes,
                        IsActive = data.IsActive
                    },
                    DisplayInfo = new StudentAttendanceDisplayInfo
                    {
                        StudentName = data.StudentName ?? string.Empty,
                        StudentCode = data.StudentCode ?? string.Empty,
                        ClassRoomName = data.ClassRoomName ?? string.Empty,
                        GradeLevelName = data.GradeLevelName ?? string.Empty,
                        AttendanceDate = data.AttendanceDate,
                        CurrentStatus = data.StatusName
                    }
                };

                // ✅ جلب حالات الحضور
                ViewBag.StatusList = GetAttendanceStatusList(data.Status);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Edit للسجل {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث سجل حضور (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> Edit(int id, StudentAttendanceEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Id = id;
                ViewBag.StatusList = GetAttendanceStatusList(viewModel.Attendance.Status ?? AttendanceStatus.Present);
                return View(viewModel);
            }

            try
            {
                var response = await _attendanceService.UpdateAsync(id, viewModel.Attendance);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث سجل الحضور");
                    viewModel.Id = id;
                    ViewBag.StatusList = GetAttendanceStatusList(viewModel.Attendance.Status ?? AttendanceStatus.Present);
                    return View(viewModel);
                }

                TempData["Success"] = "✅ تم تحديث سجل الحضور بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Edit للسجل {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                viewModel.Id = id;
                ViewBag.StatusList = GetAttendanceStatusList(viewModel.Attendance.Status ?? AttendanceStatus.Present);
                return View(viewModel);
            }
        }

        #endregion

        #region ════════════════════════════════════ Delete ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف سجل حضور
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _attendanceService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف سجل الحضور";
                }
                else
                {
                    TempData["Success"] = "✅ تم حذف سجل الحضور بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Delete للسجل {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ════════════════════════════════════ DailyReport ════════════════════════════════════

        /// <summary>
        /// 📋 عرض تقرير الحضور اليومي
        /// </summary>
        public async Task<IActionResult> DailyReport(int? schoolId = null, DateTime? date = null)
        {
            try
            {
                var viewModel = new DailyReportViewModel
                {
                    SelectedDate = date ?? DateTime.Today,
                    SelectedSchoolId = schoolId
                };

                // ✅ جلب المدارس
                var schools = await _unitOfWork.SchoolRepository.GetAllAsync();
                viewModel.Schools = schools.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.SchoolName ?? string.Empty,
                    Selected = s.Id == schoolId
                }).ToList();

                if (schoolId.HasValue)
                {
                    var response = await _attendanceService.GetDailyReportAsync(schoolId.Value, viewModel.SelectedDate);
                    if (response.Success && response.Data != null)
                    {
                        viewModel.Report = response.Data;
                    }
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في DailyReport");
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Statistics ════════════════════════════════════

        /// <summary>
        /// 📊 عرض إحصائيات حضور طالب
        /// </summary>
        public async Task<IActionResult> Statistics(int studentId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var student = await _unitOfWork.Students.GetByIdAsync(studentId);
                if (student == null)
                {
                    return NotFound("الطالب غير موجود");
                }

                fromDate ??= DateTime.Now.AddMonths(-1);
                toDate ??= DateTime.Now;

                var response = await _attendanceService.GetStatisticsAsync(studentId, fromDate.Value, toDate.Value);
                if (!response.Success || response.Data == null)
                {
                    ViewBag.Error = response.Message ?? "حدث خطأ أثناء جلب الإحصائيات";
                    return View(new StudentAttendanceStatisticsDto());
                }

                ViewBag.StudentName = student.User?.FullName;
                ViewBag.StudentCode = student.StudentCode;
                ViewBag.FromDate = fromDate.Value;
                ViewBag.ToDate = toDate.Value;

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Statistics للطالب {StudentId}", studentId);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        /// <summary>
        /// 🔄 تجهيز ViewModel للإنشاء
        /// </summary>
        private async Task<StudentAttendanceCreateViewModel> PrepareCreateViewModelAsync(int? studentId = null)
        {
            var viewModel = new StudentAttendanceCreateViewModel();

            // ✅ جلب الطلاب
            var students = await _unitOfWork.Students.GetAllAsync();
            viewModel.Students = students.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.User?.FullName ?? s.StudentCode,
                Selected = s.Id == studentId
            }).ToList();

            // ✅ حالات الحضور
            viewModel.StatusList = GetAttendanceStatusList();

            if (studentId.HasValue)
            {
                viewModel.Attendance.StudentId = studentId.Value;
                viewModel.Attendance.AttendanceDate = DateTime.Today;
            }

            return viewModel;
        }

        /// <summary>
        /// 🔄 تجهيز ViewModel للحضور الجماعي
        /// </summary>
        private async Task<StudentAttendanceCreateRangeViewModel> PrepareCreateRangeViewModelAsync(int? classRoomId = null, DateTime? date = null)
        {
            var viewModel = new StudentAttendanceCreateRangeViewModel
            {
                ClassRoomId = classRoomId ?? 0,
                Date = date ?? DateTime.Today,
                Attendances = new List<CreateStudentAttendanceDto>()
            };

            // ✅ جلب الفصول
            var classRooms = await _unitOfWork.ClassRooms.GetAllAsync();
            viewModel.ClassRooms = classRooms.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.ClassName,
                Selected = c.Id == classRoomId
            }).ToList();

            // ✅ حالات الحضور
            viewModel.StatusList = GetAttendanceStatusList();

            if (classRoomId.HasValue)
            {
                // ✅ جلب طلاب الفصل
                var students = await _unitOfWork.Students
                    .FindAsync(s => s.ClassRoomId == classRoomId.Value);

                // ✅ جلب الطلاب المسجلين بالفعل في هذا اليوم
                var existingAttendances = await _unitOfWork.StudentAttendances
                    .FindAsync(sa => sa.AttendanceDate.Date == viewModel.Date.Date);

                var existingStudentIds = existingAttendances.Select(sa => sa.StudentId).ToHashSet();

                // ✅ الطلاب غير المسجلين
                var availableStudents = students
                    .Where(s => !existingStudentIds.Contains(s.Id))
                    .ToList();

                // ✅ تخزين بيانات الطلاب في ViewBag للاستخدام في الـ View
                var studentList = new List<StudentInfo>();
                foreach (var student in availableStudents)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(student.UserId);
                    studentList.Add(new StudentInfo
                    {
                        Id = student.Id,
                        Name = user?.FullName ?? student.StudentCode,
                        Code = student.StudentCode
                    });

                    viewModel.Attendances.Add(new CreateStudentAttendanceDto
                    {
                        StudentId = student.Id,
                        AttendanceDate = viewModel.Date,
                        Status = AttendanceStatus.Present,
                        CheckInTime = viewModel.Date.Date.AddHours(8),
                        CheckOutTime = viewModel.Date.Date.AddHours(14)
                    });
                }

                ViewBag.StudentList = studentList;
                ViewBag.ClassRoomName = classRooms.FirstOrDefault(c => c.Id == classRoomId)?.ClassName;
                ViewBag.StudentCount = availableStudents.Count;
            }

            return viewModel;
        }


        /// <summary>
        /// 📝 الحصول على قائمة حالات الحضور
        /// </summary>
        private List<SelectListItem> GetAttendanceStatusList(AttendanceStatus? selected = null)
        {
            return Enum.GetValues(typeof(AttendanceStatus))
                .Cast<AttendanceStatus>()
                .Select(s => new SelectListItem
                {
                    Value = ((int)s).ToString(),
                    Text = GetAttendanceStatusName(s),
                    Selected = s == selected
                }).ToList();
        }

        /// <summary>
        /// 📝 الحصول على اسم حالة الحضور بالعربية
        /// </summary>
        private string GetAttendanceStatusName(AttendanceStatus status)
        {
            return status switch
            {
                AttendanceStatus.Present => "حاضر",
                AttendanceStatus.Absent => "غائب",
                AttendanceStatus.Late => "متأخر",
                AttendanceStatus.Excused => "معذور",
                _ => status.ToString()
            };
        }

        #endregion
    }
}