using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Web.ViewModels.Home;

namespace SchoolERP.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IUnitOfWork unitOfWork,
            ILogger<HomeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeDashboardViewModel();

            // ✅ جلب الإحصائيات الأساسية باستخدام الـ Repositories الصحيحة
            var users = await _unitOfWork.Users.GetAllAsync();
            var students = await _unitOfWork.Students.GetAllAsync();

            // ✅ إصلاح: استخدام TeacherRepository بدلاً من Teachers
            var teachers = await _unitOfWork.TeacherRepository.GetAllAsync();

            // ✅ إصلاح: استخدام EmployeeRepository بدلاً من Employees
            var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();

            // ✅ إصلاح: استخدام SchoolRepository بدلاً من Schools
            var schools = await _unitOfWork.SchoolRepository.GetAllAsync();

            var academicYears = await _unitOfWork.AcademicYears.GetAllAsync();

            viewModel.TotalUsers = users.Count();
            viewModel.TotalStudents = students.Count();
            viewModel.TotalTeachers = teachers.Count();
            viewModel.TotalEmployees = employees.Count();
            viewModel.TotalSchools = schools.Count();
            viewModel.TotalAcademicYears = academicYears.Count();

            // ✅ السنة الدراسية الحالية
            var currentYear = academicYears.FirstOrDefault(y => y.IsCurrent);
            viewModel.CurrentAcademicYear = currentYear?.YearName ?? "غير محدد";

            // ✅ عدد الطلاب المسجلين في السنة الحالية
            if (currentYear != null)
            {
                viewModel.CurrentYearStudents = students.Count(s => s.AcademicYearId == currentYear.Id);
            }

            // ✅ عدد المعلمين الجدد (آخر 30 يوم)
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);
            viewModel.NewTeachers = teachers.Count(t => t.CreatedAt >= thirtyDaysAgo);
            viewModel.NewStudents = students.Count(s => s.CreatedAt >= thirtyDaysAgo);

            // ✅ عدد المستخدمين النشطين
            viewModel.ActiveUsers = users.Count(u => u.IsActive && u.Status == Domain.Enums.UserStatus.Active);

            // ✅ جلب آخر 5 مستخدمين مسجلين
            var recentUsers = users
                .OrderByDescending(u => u.CreatedAt)
                .Take(5)
                .Select(u => new RecentUserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    UserType = u.UserType.ToString(),
                    CreatedAt = u.CreatedAt
                })
                .ToList();
            viewModel.RecentUsers = recentUsers;

            // ✅ جلب آخر 5 طلاب مسجلين مع بيانات المستخدم والفصل
            var recentStudents = new List<RecentStudentDto>();
            var studentList = students
                .OrderByDescending(s => s.CreatedAt)
                .Take(5)
                .ToList();

            foreach (var student in studentList)
            {
                var user = await _unitOfWork.Users.GetByIdAsync(student.UserId);
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(student.AcademicYearId);

                recentStudents.Add(new RecentStudentDto
                {
                    Id = student.Id,
                    StudentCode = student.StudentCode,
                    FullName = user?.FullName ?? "غير معروف",
                    AcademicYear = academicYear?.YearName ?? "غير محدد"
                });
            }
            viewModel.RecentStudents = recentStudents;

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
















//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace SchoolERP.Web.Controllers
//{
//    public class HomeController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }

//        [Authorize]
//        public IActionResult Dashboard()
//        {
//            return View();
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View();
//        }
//    }
//}