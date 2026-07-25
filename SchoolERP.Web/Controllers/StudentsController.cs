using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Students;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Web.ViewModels.Students;

namespace SchoolERP.Web.Controllers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🧑‍🎓  وحدة تحكم الطلاب (StudentsController)
    /// 📌  الوظيفة: إدارة عمليات الطلاب (CRUD)
    /// 📦  الاستخدام: نقاط النهاية (Endpoints) للطلاب
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    [Authorize]
    public class StudentsController : Controller
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly IStudentService _studentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StudentsController> _logger;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        public StudentsController(
            IStudentService studentService,
            IUnitOfWork unitOfWork,
            ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        #region ════════════════════════════════════ Index ════════════════════════════════════

        /// <summary>
        /// 📋 عرض جميع الطلاب
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _studentService.GetAllAsync();
                if (!response.Success || response.Data == null)
                {
                    _logger.LogError("خطأ في جلب الطلاب: {Message}", response.Message);
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
        /// 🔍 عرض تفاصيل طالب
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _studentService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Details للطالب {Id}", id);
                return View("Error");
            }
        }

        #endregion

        #region ════════════════════════════════════ Create ════════════════════════════════════

        /// <summary>
        /// ➕ عرض صفحة إنشاء طالب جديد
        /// </summary>
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create()
        {
            // جلب المستخدمين للقائمة المنسدلة
            var users = await _unitOfWork.Users.GetAllAsync();
            ViewBag.Users = users.ToList();

            // جلب الأعوام الدراسية
            var academicYears = await _unitOfWork.AcademicYears.GetAllAsync();
            ViewBag.AcademicYears = academicYears.ToList();

            // جلب الفصول
            var classRooms = await _unitOfWork.ClassRooms.GetAllAsync();
            ViewBag.ClassRooms = classRooms.ToList();

            return View(new CreateStudentDto());
        }

        /// <summary>
        /// ➕ إنشاء طالب جديد (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> Create(CreateStudentDto createDto)
        {
            if (!ModelState.IsValid)
            {
                var users = await _unitOfWork.Users.GetAllAsync();
                ViewBag.Users = users.ToList();

                var academicYears = await _unitOfWork.AcademicYears.GetAllAsync();
                ViewBag.AcademicYears = academicYears.ToList();

                var classRooms = await _unitOfWork.ClassRooms.GetAllAsync();
                ViewBag.ClassRooms = classRooms.ToList();

                return View(createDto);
            }

            try
            {
                var response = await _studentService.CreateAsync(createDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء إنشاء الطالب");

                    var users = await _unitOfWork.Users.GetAllAsync();
                    ViewBag.Users = users.ToList();

                    var academicYears = await _unitOfWork.AcademicYears.GetAllAsync();
                    ViewBag.AcademicYears = academicYears.ToList();

                    var classRooms = await _unitOfWork.ClassRooms.GetAllAsync();
                    ViewBag.ClassRooms = classRooms.ToList();

                    return View(createDto);
                }

                TempData["Success"] = "تم إنشاء الطالب بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Create");
                ModelState.AddModelError("", "حدث خطأ غير متوقع");

                var users = await _unitOfWork.Users.GetAllAsync();
                ViewBag.Users = users.ToList();

                var academicYears = await _unitOfWork.AcademicYears.GetAllAsync();
                ViewBag.AcademicYears = academicYears.ToList();

                var classRooms = await _unitOfWork.ClassRooms.GetAllAsync();
                ViewBag.ClassRooms = classRooms.ToList();

                return View(createDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Edit ════════════════════════════════════

        /// <summary>
        /// ✏️ عرض صفحة تعديل طالب
        /// </summary>
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _studentService.GetByIdAsync(id);
                if (!response.Success || response.Data == null)
                {
                    return NotFound();
                }

                var updateDto = new UpdateStudentDto
                {
                    ClassRoomId = response.Data.ClassRoomId,
                    ParentName = response.Data.ParentName,
                    ParentPhone = response.Data.ParentPhone,
                    ParentEmail = response.Data.ParentEmail,
                    IsGraduated = response.Data.IsGraduated,
                    IsActive = response.Data.IsActive
                };

                // جلب الفصول للقائمة المنسدلة
                var classRooms = await _unitOfWork.ClassRooms.GetAllAsync();
                ViewBag.ClassRooms = classRooms.ToList();
                ViewBag.Id = id;

                return View(updateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للطالب {Id}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// ✏️ تحديث طالب (POST)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> Edit(int id, UpdateStudentDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                var classRooms = await _unitOfWork.ClassRooms.GetAllAsync();
                ViewBag.ClassRooms = classRooms.ToList();
                ViewBag.Id = id;
                return View(updateDto);
            }

            try
            {
                var response = await _studentService.UpdateAsync(id, updateDto);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message ?? "حدث خطأ أثناء تحديث الطالب");

                    var classRooms = await _unitOfWork.ClassRooms.GetAllAsync();
                    ViewBag.ClassRooms = classRooms.ToList();
                    ViewBag.Id = id;
                    return View(updateDto);
                }

                TempData["Success"] = "تم تحديث الطالب بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Edit للطالب {Id}", id);
                ModelState.AddModelError("", "حدث خطأ غير متوقع");

                var classRooms = await _unitOfWork.ClassRooms.GetAllAsync();
                ViewBag.ClassRooms = classRooms.ToList();
                ViewBag.Id = id;
                return View(updateDto);
            }
        }

        #endregion

        #region ════════════════════════════════════ Delete ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف طالب (Soft Delete)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _studentService.DeleteAsync(id);
                if (!response.Success)
                {
                    TempData["Error"] = response.Message ?? "حدث خطأ أثناء حذف الطالب";
                }
                else
                {
                    TempData["Success"] = "تم حذف الطالب بنجاح";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في Delete للطالب {Id}", id);
                TempData["Error"] = "حدث خطأ غير متوقع";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion



        // ============================================================
        // ✅  الملف الشخصي للطالب
        // ============================================================

        /// <summary>
        /// 📋 عرض الملف الشخصي للطالب الحالي
        /// </summary>
        public async Task<IActionResult> MyProfile()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Auth");
                }

                var userIdInt = int.Parse(userId);

                var students = await _unitOfWork.Students
                    .FindAsync(s => s.UserId == userIdInt);
                var student = students.FirstOrDefault();

                if (student == null)
                {
                    TempData["Error"] = "لا توجد بيانات طالب مرتبطة بهذا المستخدم";
                    return RedirectToAction("Index", "Home");
                }

                // ✅ جلب البيانات الإضافية مع التحقق من null
                var user = await _unitOfWork.Users.GetByIdAsync(student.UserId);
                var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(student.AcademicYearId);

                var classRoom = student.ClassRoomId.HasValue
                    ? await _unitOfWork.ClassRooms.GetByIdAsync(student.ClassRoomId.Value)
                    : null;

                var gradeLevel = classRoom != null
                    ? await _unitOfWork.GradeLevels.GetByIdAsync(classRoom.GradeLevelId)
                    : null;

                var viewModel = new StudentProfileViewModel
                {
                    Id = student.Id,
                    StudentCode = student.StudentCode,
                    FullName = user?.FullName ?? "غير معروف",
                    Email = user?.Email ?? "",
                    Username = user?.Username ?? "",
                    DateOfBirth = user?.DateOfBirth ?? DateTime.Now,
                    Gender = user?.Gender ?? "",
                    Address = user?.Address ?? "",
                    //Phone = user?.Phone ?? "",
                    NationalId = user?.NationalId ?? "",
                    AcademicYear = academicYear?.YearName ?? "غير محدد",
                    ClassRoomName = classRoom?.ClassName ?? "غير محدد",
                    GradeLevelName = gradeLevel?.GradeName ?? "غير محدد",
                    EnrollmentDate = student.EnrollmentDate,
                    IsGraduated = student.IsGraduated,
                    ParentName = student.ParentName,
                    ParentPhone = student.ParentPhone,
                    ParentEmail = student.ParentEmail
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في MyProfile");
                TempData["Error"] = "حدث خطأ أثناء تحميل الملف الشخصي";
                return RedirectToAction("Index", "Home");
            }
        }

        // ============================================================
        // ✅  درجات الطالب
        // ============================================================

        /// <summary>
        /// 📊 عرض درجات الطالب الحالي
        /// </summary>
        public async Task<IActionResult> MyGrades()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Auth");
                }

                var userIdInt = int.Parse(userId);
                var students = await _unitOfWork.Students
                    .FindAsync(s => s.UserId == userIdInt);
                var student = students.FirstOrDefault();

                if (student == null)
                {
                    TempData["Error"] = "لا توجد بيانات طالب مرتبطة بهذا المستخدم";
                    return RedirectToAction("Index", "Home");
                }

                // ✅ جلب نتائج الامتحانات للطالب
                var examResults = await _unitOfWork.ExamResults
                    .FindAsync(er => er.StudentId == student.Id);

                var viewModel = new StudentGradesViewModel
                {
                    StudentName = student.User?.FullName ?? "غير معروف",
                    StudentCode = student.StudentCode,
                    Grades = new List<StudentGradeDto>()
                };

                foreach (var result in examResults)
                {
                    // ✅ جلب الامتحان مع التحقق من null
                    var exam = await _unitOfWork.Exams.GetByIdAsync(result.ExamId);

                    if (exam == null)
                    {
                        continue;
                    }

                    // ✅ جلب المادة مع التحقق من null
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(exam.SubjectId);

                    viewModel.Grades.Add(new StudentGradeDto
                    {
                        ExamName = exam.ExamName ?? "غير معروف",
                        SubjectName = subject?.SubjectName ?? "غير معروف",
                        Score = result.Score,
                        MaxScore = exam.MaxScore > 0 ? exam.MaxScore : 100,
                        Percentage = exam.MaxScore > 0 ? (decimal)result.Score / exam.MaxScore * 100 : 0,
                        Grade = result.Grade ?? GetGradeFromPercentage(result.Score, exam.MaxScore),
                        ExamDate = exam.ExamDate,  // ✅ ExamDate من Exam
                        IsPassed = result.Score >= 50
                    });
                }

                // ✅ ترتيب الدرجات حسب التاريخ (الأحدث أولاً)
                viewModel.Grades = viewModel.Grades
                    .OrderByDescending(g => g.ExamDate)
                    .ToList();

                // ✅ حساب المعدل
                if (viewModel.Grades.Any())
                {
                    viewModel.AverageScore = viewModel.Grades.Average(g => g.Percentage);
                    viewModel.TotalExams = viewModel.Grades.Count;
                    viewModel.PassedExams = viewModel.Grades.Count(g => g.IsPassed);
                    viewModel.FailedExams = viewModel.Grades.Count(g => !g.IsPassed);
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في MyGrades");
                TempData["Error"] = "حدث خطأ أثناء تحميل الدرجات";
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// 📝 حساب التقدير بناءً على النسبة المئوية
        /// </summary>
        private string GetGradeFromPercentage(int score, int maxScore)
        {
            if (maxScore == 0) return "F";

            var percentage = (decimal)score / maxScore * 100;

            if (percentage >= 90) return "A (ممتاز)";
            if (percentage >= 80) return "B (جيد جداً)";
            if (percentage >= 70) return "C (جيد)";
            if (percentage >= 60) return "D (مقبول)";
            if (percentage >= 50) return "E (ضعيف)";
            return "F (راسب)";
        }
 
        // ============================================================
        // ✅  جدول الطالب الدراسي
        // ============================================================

        /// <summary>
        /// 📅 عرض الجدول الدراسي للطالب الحالي
        /// </summary>
        public async Task<IActionResult> MySchedule()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Auth");
                }

                var userIdInt = int.Parse(userId);
                var students = await _unitOfWork.Students
                    .FindAsync(s => s.UserId == userIdInt);
                var student = students.FirstOrDefault();

                if (student == null)
                {
                    TempData["Error"] = "لا توجد بيانات طالب مرتبطة بهذا المستخدم";
                    return RedirectToAction("Index", "Home");
                }

                if (!student.ClassRoomId.HasValue)
                {
                    TempData["Error"] = "لا يوجد فصل دراسي مرتبط بهذا الطالب";
                    return RedirectToAction("Index", "Home");
                }

                // ✅ جلب الجدول الدراسي للفصل
                var schedules = await _unitOfWork.ClassSchedules
                    .FindAsync(cs => cs.ClassRoomId == student.ClassRoomId.Value
                                     && cs.AcademicYearId == student.AcademicYearId);

                // ✅ جلب بيانات الفصل والصف
                var classRoom = await _unitOfWork.ClassRooms.GetByIdAsync(student.ClassRoomId.Value);
                var gradeLevel = classRoom != null
                    ? await _unitOfWork.GradeLevels.GetByIdAsync(classRoom.GradeLevelId)
                    : null;

                var viewModel = new StudentScheduleViewModel
                {
                    StudentName = student.User?.FullName ?? "غير معروف",
                    ClassRoomName = classRoom?.ClassName ?? "غير محدد",
                    GradeLevelName = gradeLevel?.GradeName ?? "غير محدد",
                    AcademicYearName = student.AcademicYear?.YearName ?? "غير محدد",
                    Schedules = new List<StudentScheduleDto>()
                };

                foreach (var schedule in schedules)
                {
                    // ✅ جلب المادة مع التحقق من null
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(schedule.SubjectId);

                    // ✅ جلب المعلم مع التحقق من null
                    var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(schedule.TeacherId);

                    viewModel.Schedules.Add(new StudentScheduleDto
                    {
                        DayOfWeek = schedule.DayOfWeek,
                        DayName = GetDayName(schedule.DayOfWeek),
                        StartTime = schedule.StartTime,
                        EndTime = schedule.EndTime,
                        SubjectName = subject?.SubjectName ?? "غير معروف",
                        TeacherName = teacher?.User?.FullName ?? "غير معروف",
                        PeriodNumber = schedule.PeriodNumber
                    });
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطأ في MySchedule");
                TempData["Error"] = "حدث خطأ أثناء تحميل الجدول الدراسي";
                return RedirectToAction("Index", "Home");
            }
        }
        // ============================================================
        // 🔧  دوال مساعدة
        // ============================================================

        private string GetDayName(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Sunday => "الأحد",
                DayOfWeek.Monday => "الإثنين",
                DayOfWeek.Tuesday => "الثلاثاء",
                DayOfWeek.Wednesday => "الأربعاء",
                DayOfWeek.Thursday => "الخميس",
                DayOfWeek.Friday => "الجمعة",
                DayOfWeek.Saturday => "السبت",
                _ => day.ToString()
            };
        }
    }
}