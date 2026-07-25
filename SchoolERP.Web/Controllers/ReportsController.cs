using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolERP.Application.Interfaces.Services;

namespace SchoolERP.Web.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        // ============================================================
        // عرض صفحة التقارير الرئيسية
        // ============================================================
        public IActionResult Index()
        {
            return View();
        }

        // ============================================================
        // تصدير التقرير بصيغة PDF
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> ExportPdf(string reportName, object data)
        {
            var response = await _reportService.ExportToPdfAsync(reportName, data);
            if (!response.Success || response.Data == null)
            {
                TempData["Error"] = response.Message ?? "حدث خطأ أثناء تصدير التقرير";
                return RedirectToAction(nameof(Index));
            }

            // ✅ تصدير PDF في MVC
            return File(response.Data, "application/pdf", $"{reportName}.pdf");
        }

        // ============================================================
        // تصدير التقرير بصيغة Excel
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> ExportExcel(string reportName, object data)
        {
            var response = await _reportService.ExportToExcelAsync(reportName, data);
            if (!response.Success || response.Data == null)
            {
                TempData["Error"] = response.Message ?? "حدث خطأ أثناء تصدير التقرير";
                return RedirectToAction(nameof(Index));
            }

            // ✅ تصدير Excel في MVC
            return File(response.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{reportName}.xlsx");
        }

        // ============================================================
        // تقرير حضور الطلاب اليومي
        // ============================================================
        public async Task<IActionResult> DailyStudentAttendance(int schoolId, DateTime date)
        {
            var response = await _reportService.GetDailyStudentAttendanceReportAsync(schoolId, date);
            if (!response.Success)
            {
                TempData["Error"] = response.Message ?? "حدث خطأ";
                return RedirectToAction(nameof(Index));
            }
            return View(response.Data);
        }

        // ============================================================
        // تقرير حضور الطلاب الشهري
        // ============================================================
        public async Task<IActionResult> MonthlyStudentAttendance(int schoolId, int month, int year)
        {
            var response = await _reportService.GetMonthlyStudentAttendanceReportAsync(schoolId, month, year);
            if (!response.Success)
            {
                TempData["Error"] = response.Message ?? "حدث خطأ";
                return RedirectToAction(nameof(Index));
            }
            return View(response.Data);
        }

        // ============================================================
        // تقرير حضور الموظفين اليومي
        // ============================================================
        public async Task<IActionResult> DailyEmployeeAttendance(int schoolId, DateTime date)
        {
            var response = await _reportService.GetDailyEmployeeAttendanceReportAsync(schoolId, date);
            if (!response.Success)
            {
                TempData["Error"] = response.Message ?? "حدث خطأ";
                return RedirectToAction(nameof(Index));
            }
            return View(response.Data);
        }

        // ============================================================
        // تقرير نتائج الامتحانات
        // ============================================================
        public async Task<IActionResult> ExamResults(int examId)
        {
            var response = await _reportService.GetExamResultsReportAsync(examId);
            if (!response.Success)
            {
                TempData["Error"] = response.Message ?? "حدث خطأ";
                return RedirectToAction(nameof(Index));
            }
            return View(response.Data);
        }

        // ============================================================
        // تقرير نتائج الطالب
        // ============================================================
        public async Task<IActionResult> StudentReport(int studentId, int academicYearId)
        {
            var response = await _reportService.GetStudentReportAsync(studentId, academicYearId);
            if (!response.Success)
            {
                TempData["Error"] = response.Message ?? "حدث خطأ";
                return RedirectToAction(nameof(Index));
            }
            return View(response.Data);
        }

        // ============================================================
        // تقرير أداء المعلم
        // ============================================================
        public async Task<IActionResult> TeacherPerformance(int teacherId, int academicYearId)
        {
            var response = await _reportService.GetTeacherPerformanceReportAsync(teacherId, academicYearId);
            if (!response.Success)
            {
                TempData["Error"] = response.Message ?? "حدث خطأ";
                return RedirectToAction(nameof(Index));
            }
            return View(response.Data);
        }

        // ============================================================
        // تقرير الجدول الأسبوعي للفصل
        // ============================================================
        public async Task<IActionResult> WeeklySchedule(int classRoomId)
        {
            var response = await _reportService.GetWeeklyScheduleReportAsync(classRoomId);
            if (!response.Success)
            {
                TempData["Error"] = response.Message ?? "حدث خطأ";
                return RedirectToAction(nameof(Index));
            }
            return View(response.Data);
        }

        // ============================================================
        // تقرير إحصائيات المدرسة
        // ============================================================
        public async Task<IActionResult> SchoolStatistics(int schoolId)
        {
            var response = await _reportService.GetSchoolStatisticsReportAsync(schoolId);
            if (!response.Success)
            {
                TempData["Error"] = response.Message ?? "حدث خطأ";
                return RedirectToAction(nameof(Index));
            }
            return View(response.Data);
        }

        // ============================================================
        // تقرير توزيع الطلاب
        // ============================================================
        public async Task<IActionResult> StudentDistribution(int schoolId)
        {
            var response = await _reportService.GetStudentDistributionReportAsync(schoolId);
            if (!response.Success)
            {
                TempData["Error"] = response.Message ?? "حدث خطأ";
                return RedirectToAction(nameof(Index));
            }
            return View(response.Data);
        }

        // ============================================================
        // تقرير توزيع المعلمين
        // ============================================================
        public async Task<IActionResult> TeacherDistribution(int schoolId)
        {
            var response = await _reportService.GetTeacherDistributionReportAsync(schoolId);
            if (!response.Success)
            {
                TempData["Error"] = response.Message ?? "حدث خطأ";
                return RedirectToAction(nameof(Index));
            }
            return View(response.Data);
        }

        // ============================================================
        // تقرير الغرف المدرسية الأسبوعي
        // ============================================================
        public async Task<IActionResult> WeeklyClassRooms(int schoolId, int academicYearId)
        {
            var response = await _reportService.GetWeeklyClassRoomScheduleReportAsync(schoolId, academicYearId);
            if (!response.Success)
            {
                TempData["Error"] = response.Message ?? "حدث خطأ";
                return RedirectToAction(nameof(Index));
            }
            return View(response.Data);
        }
    }
}