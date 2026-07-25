using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolERP.Application.Interfaces;

namespace SchoolERP.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IUnitOfWork unitOfWork, ILogger<DashboardController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // ✅ استخدام TeacherRepository و EmployeeRepository
                var totalStudents = (await _unitOfWork.Students.GetAllAsync()).Count();
                var totalTeachers = (await _unitOfWork.TeacherRepository.GetAllAsync()).Count();
                var totalEmployees = (await _unitOfWork.EmployeeRepository.GetAllAsync()).Count();
                var totalSchools = (await _unitOfWork.SchoolRepository.GetAllAsync()).Count();

                ViewBag.TotalStudents = totalStudents;
                ViewBag.TotalTeachers = totalTeachers;
                ViewBag.TotalEmployees = totalEmployees;
                ViewBag.TotalSchools = totalSchools;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في لوحة التحكم");
                return View("Error");
            }
        }
    }
}