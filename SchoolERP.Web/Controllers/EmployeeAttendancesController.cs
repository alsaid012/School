using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolERP.Application.DTOs.EmployeeAttendances;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Domain.Enums;
using SchoolERP.Web.ViewModels.EmployeeAttendances;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✅  وحدة تحكم حضور الموظفين (EmployeeAttendancesController)
    /// 📌  الوظيفة: إدارة عمليات حضور الموظفين (CRUD + عمليات إضافية)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize]
    public class EmployeeAttendancesController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IEmployeeAttendanceService _attendanceService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EmployeeAttendancesController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public EmployeeAttendancesController(
            IEmployeeAttendanceService attendanceService,
            IUnitOfWork unitOfWork,
            ILogger<EmployeeAttendancesController> logger)
        {
            _attendanceService = attendanceService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع سجلات حضور الموظفين
        /// </summary>
        public async Task<IActionResult> Index(int? employeeId = null, DateTime? date = null)
        {
            try
            {
                var viewModel = new EmployeeAttendanceIndexViewModel
                {
                    SelectedEmployeeId = employeeId,
                    SelectedDate = date ?? DateTime.Today
                };

                // ✅ جلب الموظفين للفلترة
                var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                viewModel.Employees = employees.Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.User?.FullName ?? e.EmployeeCode,
                    Selected = e.Id == employeeId
                }).ToList();

                // ✅ جلب سجلات الحضور
                if (employeeId.HasValue)
                {
                    var response = await _attendanceService.GetByEmployeeIdAsync(employeeId.Value);
                    if (response.Success && response.Data != null)
                    {
                        viewModel.Attendances = response.Data.ToList();
                    }
                }
                else if (date.HasValue)
                {
                    var response = await _attendanceService.GetByDateAsync(date.Value);
                    if (response.Success && response.Data != null)
                    {
                        viewModel.Attendances = response.Data.ToList();
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
        /// 🔍 عرض تفاصيل سجل حضور موظف
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
        /// ➕ عرض صفحة تسجيل حضور موظف
        /// </summary>
        [Authorize(Roles = "Admin,Principal,HR")]
        public async Task<IActionResult> Create(int? employeeId = null)
        {
            var viewModel = await PrepareCreateViewModelAsync(employeeId);
            return View(viewModel);
        }

        /// <summary>
        /// ➕ تسجيل حضور موظف (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal,HR")]
        public async Task<IActionResult> Create(EmployeeAttendanceCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await PrepareCreateViewModelAsync(viewModel.Attendance.EmployeeId);
                return View(viewModel);
            }

            try
            {
                var response = await _attendanceService.CreateAsync(viewModel.Attendance);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تسجيل الحضور");
                    viewModel = await PrepareCreateViewModelAsync(viewModel.Attendance.EmployeeId);
                    return View(viewModel);
                }

                TempData["Success"] = "✅ تم تسجيل الحضور بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");
                viewModel = await PrepareCreateViewModelAsync(viewModel.Attendance.EmployeeId);
                return View(viewModel);
            }
        }

        #endregion

        #region ════════════════════════════════════ CreateRange (تسجيل حضور جماعي) ════════════════════════════════════

        /// <summary>
        /// ➕➕ عرض صفحة تسجيل حضور جماعي للموظفين
        /// </summary>
        [Authorize(Roles = "Admin,Principal,HR")]
        public async Task<IActionResult> CreateRange(string? department = null, DateTime? date = null)
        {
            var viewModel = await PrepareCreateRangeViewModelAsync(department, date);
            return View(viewModel);
        }

        /// <summary>
        /// ➕➕ تسجيل حضور جماعي للموظفين (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal,HR")]
        public async Task<IActionResult> CreateRange(EmployeeAttendanceCreateRangeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await PrepareCreateRangeViewModelAsync(viewModel.Department, viewModel.Date);
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
                    TempData["Success"] = $"✅ تم تسجيل حضور {createdCount} موظف بنجاح";
                    if (errors.Any())
                    {
                        TempData["Warning"] = $"⚠️ بعض الموظفين لم يتم تسجيلهم: {string.Join("; ", errors)}";
                    }
                }
                else
                {
                    TempData["Error"] = "❌ لم يتم تسجيل أي موظف، تأكد من البيانات";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في CreateRange");
                TempData["Error"] = "حدث خطأ غير متوقع";
                viewModel = await PrepareCreateRangeViewModelAsync(viewModel.Department, viewModel.Date);
                return View(viewModel);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل سجل حضور موظف
        /// </summary>
        [Authorize(Roles = "Admin,Principal,HR")]
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

                var viewModel = new EmployeeAttendanceEditViewModel
                {
                    Id = id,
                    Attendance = new UpdateEmployeeAttendanceDto
                    {
                        CheckInTime = data.CheckInTime,
                        CheckOutTime = data.CheckOutTime,
                        Status = data.Status,
                        DelayMinutes = data.DelayMinutes,
                        Notes = data.Notes,
                        IsActive = data.IsActive
                    },
                    DisplayInfo = new EmployeeAttendanceDisplayInfo
                    {
                        EmployeeName = data.EmployeeName ?? string.Empty,
                        EmployeeCode = data.EmployeeCode ?? string.Empty,
                        JobTitle = data.JobTitle ?? string.Empty,
                        Department = data.Department ?? string.Empty,
                        SchoolName = data.SchoolName ?? string.Empty,
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
        /// ✏️ تحديث سجل حضور موظف (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Principal,HR")]
        public async Task<IActionResult> Edit(int id, EmployeeAttendanceEditViewModel viewModel)
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
        /// 🗑️ حذف سجل حضور موظف
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
        /// 📋 عرض تقرير الحضور اليومي للموظفين
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
        /// 📊 عرض إحصائيات حضور موظف
        /// </summary>
        public async Task<IActionResult> Statistics(int employeeId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
                if (employee == null)
                {
                    return NotFound("الموظف غير موجود");
                }

                fromDate ??= DateTime.Now.AddMonths(-1);
                toDate ??= DateTime.Now;

                var response = await _attendanceService.GetStatisticsAsync(employeeId, fromDate.Value, toDate.Value);
                if (!response.Success || response.Data == null)
                {
                    ViewBag.Error = response.Message ?? "حدث خطأ أثناء جلب الإحصائيات";
                    return View(new EmployeeAttendanceStatisticsDto());
                }

                ViewBag.EmployeeName = employee.User?.FullName;
                ViewBag.EmployeeCode = employee.EmployeeCode;
                ViewBag.JobTitle = employee.JobTitle;
                ViewBag.Department = employee.Department;
                ViewBag.FromDate = fromDate.Value;
                ViewBag.ToDate = toDate.Value;

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في Statistics للموظف {EmployeeId}", employeeId);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        /// <summary>
        /// 🔄 تجهيز ViewModel للإنشاء
        /// </summary>
        private async Task<EmployeeAttendanceCreateViewModel> PrepareCreateViewModelAsync(int? employeeId = null)
        {
            var viewModel = new EmployeeAttendanceCreateViewModel();

            // ✅ جلب الموظفين
            var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            viewModel.Employees = employees.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.User?.FullName ?? e.EmployeeCode,
                Selected = e.Id == employeeId
            }).ToList();

            // ✅ حالات الحضور
            viewModel.StatusList = GetAttendanceStatusList();

            if (employeeId.HasValue)
            {
                viewModel.Attendance.EmployeeId = employeeId.Value;
                viewModel.Attendance.AttendanceDate = DateTime.Today;
            }

            return viewModel;
        }

        /// <summary>
        /// 🔄 تجهيز ViewModel للحضور الجماعي للموظفين
        /// </summary>
        private async Task<EmployeeAttendanceCreateRangeViewModel> PrepareCreateRangeViewModelAsync(string? department = null, DateTime? date = null)
        {
            var viewModel = new EmployeeAttendanceCreateRangeViewModel
            {
                Department = department ?? string.Empty,
                Date = date ?? DateTime.Today,
                Attendances = new List<CreateEmployeeAttendanceDto>()
            };

            // ✅ جلب الأقسام
            var allEmployees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            var departments = allEmployees
                .Where(e => !string.IsNullOrEmpty(e.Department))
                .Select(e => e.Department!)
                .Distinct()
                .ToList();

            viewModel.Departments = departments.Select(d => new SelectListItem
            {
                Value = d,
                Text = d,
                Selected = d == department
            }).ToList();

            // ✅ حالات الحضور
            viewModel.StatusList = GetAttendanceStatusList();

            if (!string.IsNullOrEmpty(department))
            {
                // ✅ جلب موظفي القسم
                var employees = await _unitOfWork.EmployeeRepository
                    .FindAsync(e => e.Department == department);

                // ✅ جلب الموظفين المسجلين بالفعل في هذا اليوم
                var existingAttendances = await _unitOfWork.EmployeeAttendances
                    .FindAsync(ea => ea.AttendanceDate.Date == viewModel.Date.Date);

                var existingEmployeeIds = existingAttendances.Select(ea => ea.EmployeeId).ToHashSet();

                // ✅ الموظفين غير المسجلين
                var availableEmployees = employees
                    .Where(e => !existingEmployeeIds.Contains(e.Id))
                    .ToList();

                // ✅ تخزين بيانات الموظفين في ViewBag
                var employeeList = new List<EmployeeInfo>();
                foreach (var employee in availableEmployees)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(employee.UserId);
                    employeeList.Add(new EmployeeInfo
                    {
                        Id = employee.Id,
                        Name = user?.FullName ?? employee.EmployeeCode,
                        Code = employee.EmployeeCode,
                        JobTitle = employee.JobTitle
                    });

                    viewModel.Attendances.Add(new CreateEmployeeAttendanceDto
                    {
                        EmployeeId = employee.Id,
                        AttendanceDate = viewModel.Date,
                        Status = AttendanceStatus.Present,
                        CheckInTime = viewModel.Date.Date.AddHours(8),
                        CheckOutTime = viewModel.Date.Date.AddHours(16)
                    });
                }

                ViewBag.EmployeeList = employeeList;
                ViewBag.DepartmentName = department;
                ViewBag.EmployeeCount = availableEmployees.Count;
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