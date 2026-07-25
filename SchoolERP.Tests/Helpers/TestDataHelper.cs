using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;
using BCrypt.Net;

namespace SchoolERP.Tests.Helpers
{
    /// <summary>
    /// 🧪  مساعد بيانات الاختبار
    /// 📌  الوظيفة: إنشاء بيانات افتراضية للاختبارات
    /// </summary>
    public static class TestDataHelper
    {
        #region ════════════════════════════════════ Users ════════════════════════════════════

        /// <summary>
        /// 👤 إنشاء مستخدم اختبار
        /// </summary>
        public static User CreateTestUser(int id = 1, string username = "testuser", string fullName = "Test User")
        {
            return new User
            {
                Id = id,
                Username = username,
                FullName = fullName,
                NationalId = "12345678901234",
                Email = "test@example.com",
                UserType = UserType.Student,
                Status = UserStatus.Active,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        /// <summary>
        /// 👤 إنشاء مستخدم مع كلمة مرور مشفرة
        /// </summary>
        public static User CreateTestUserWithPassword(int id = 1, string username = "testuser", string fullName = "Test User", string password = "Password@123")
        {
            var user = CreateTestUser(id, username, fullName);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return user;
        }

        /// <summary>
        /// 👤 إنشاء مستخدم Admin
        /// </summary>
        public static User CreateTestAdmin(int id = 1, string username = "admin", string fullName = "Admin User")
        {
            var user = CreateTestUser(id, username, fullName);
            user.UserType = UserType.Admin;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");
            user.UserRoles.Add(new UserRole { RoleType = UserType.Admin, IsPrimary = true });
            return user;
        }

        #endregion

        #region ════════════════════════════════════ Students ════════════════════════════════════

        /// <summary>
        /// 🧑‍🎓 إنشاء طالب اختبار
        /// </summary>
        public static Student CreateTestStudent(int id = 1, int userId = 1, string studentCode = "STU-2024-001")
        {
            return new Student
            {
                Id = id,
                UserId = userId,
                StudentCode = studentCode,
                AcademicYearId = 1,
                EnrollmentDate = DateTime.Now,
                IsGraduated = false,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        #endregion

        #region ════════════════════════════════════ Teachers ════════════════════════════════════

        /// <summary>
        /// 👨‍🏫 إنشاء معلم اختبار
        /// </summary>
        public static Teacher CreateTestTeacher(int id = 1, int userId = 1, string teacherCode = "TCH-2024-001")
        {
            return new Teacher
            {
                Id = id,
                UserId = userId,
                TeacherCode = teacherCode,
                Qualification = "ليسانس آداب",
                Specialization = "اللغة العربية",
                HireDate = DateTime.Now.AddYears(-5),
                IsHomeroomTeacher = false,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        #endregion

        #region ════════════════════════════════════ Employees ════════════════════════════════════

        /// <summary>
        /// 👨‍💼 إنشاء موظف اختبار
        /// </summary>
        public static Employee CreateTestEmployee(int id = 1, int userId = 1, string employeeCode = "EMP-2024-001")
        {
            return new Employee
            {
                Id = id,
                UserId = userId,
                EmployeeCode = employeeCode,
                JobTitle = "مدير شؤون الطلاب",
                Department = "شؤون الطلاب",
                HireDate = DateTime.Now.AddYears(-3),
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        #endregion

        #region ════════════════════════════════════ Governorates ════════════════════════════════════

        /// <summary>
        /// 📍 إنشاء محافظة اختبار
        /// </summary>
        public static Governorate CreateTestGovernorate(int id = 1, string name = "القاهرة", string code = "CAI")
        {
            return new Governorate
            {
                Id = id,
                Name = name,
                Code = code,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        #endregion

        #region ════════════════════════════════════ Departments ════════════════════════════════════

        /// <summary>
        /// 🏢 إنشاء إدارة تعليمية اختبار
        /// </summary>
        public static Department CreateTestDepartment(int id = 1, int governorateId = 1, string name = "إدارة شمال القاهرة")
        {
            return new Department
            {
                Id = id,
                GovernorateId = governorateId,
                Name = name,
                Code = "SH-NORTH",
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        #endregion

        #region ════════════════════════════════════ Schools ════════════════════════════════════

        /// <summary>
        /// 🏫 إنشاء مدرسة اختبار
        /// </summary>
        public static School CreateTestSchool(int id = 1, int departmentId = 1, string name = "مدرسة النصر", string code = "SCH-001")
        {
            return new School
            {
                Id = id,
                DepartmentId = departmentId,
                SchoolName = name,
                SchoolCode = code,
                SchoolType = SchoolType.Public,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        #endregion

        #region ════════════════════════════════════ GradeLevels ════════════════════════════════════

        /// <summary>
        /// 📚 إنشاء صف دراسي اختبار
        /// </summary>
        public static GradeLevel CreateTestGradeLevel(int id = 1, int schoolId = 1, string name = "الصف الأول الثانوي", int number = 1)
        {
            return new GradeLevel
            {
                Id = id,
                SchoolId = schoolId,
                GradeName = name,
                GradeNumber = number,
                GradeStage = GradeStage.Secondary,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        #endregion

        #region ════════════════════════════════════ ClassRooms ════════════════════════════════════

        /// <summary>
        /// 🏫 إنشاء فصل دراسي اختبار
        /// </summary>
        public static ClassRoom CreateTestClassRoom(int id = 1, int gradeLevelId = 1, string name = "1/أ", int capacity = 30)
        {
            return new ClassRoom
            {
                Id = id,
                GradeLevelId = gradeLevelId,
                ClassName = name,
                ClassCode = "CLS-001",
                Capacity = capacity,
                HasSmartBoard = true,
                HasProjector = true,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        #endregion

        #region ════════════════════════════════════ Subjects ════════════════════════════════════

        /// <summary>
        /// 📖 إنشاء مادة دراسية اختبار
        /// </summary>
        public static Subject CreateTestSubject(int id = 1, int gradeLevelId = 1, string name = "اللغة العربية")
        {
            return new Subject
            {
                Id = id,
                GradeLevelId = gradeLevelId,
                SubjectName = name,
                SubjectCode = "SUB-AR-001",
                WeeklyHours = 4,
                IsRequired = true,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        #endregion

        #region ════════════════════════════════════ AcademicYears ════════════════════════════════════

        /// <summary>
        /// 📆 إنشاء عام دراسي اختبار
        /// </summary>
        public static AcademicYear CreateTestAcademicYear(int id = 1, int schoolId = 1, string yearName = "2024-2025")
        {
            return new AcademicYear
            {
                Id = id,
                SchoolId = schoolId,
                YearName = yearName,
                StartDate = new DateTime(2024, 9, 1),
                EndDate = new DateTime(2025, 6, 30),
                IsCurrent = true,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        #endregion

        #region ════════════════════════════════════ Exams ════════════════════════════════════

        /// <summary>
        /// 📝 إنشاء امتحان اختبار
        /// </summary>
        public static Exam CreateTestExam(int id = 1, int academicYearId = 1, int subjectId = 1, string name = "امتحان اللغة العربية")
        {
            return new Exam
            {
                Id = id,
                AcademicYearId = academicYearId,
                SubjectId = subjectId,
                ExamName = name,
                ExamType = ExamType.Monthly,
                ExamDate = DateTime.Now.AddDays(7),
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(12, 0, 0),
                MaxScore = 100,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        #endregion

        #region ════════════════════════════════════ ExamResults ════════════════════════════════════

        /// <summary>
        /// 📊 إنشاء نتيجة امتحان اختبار
        /// </summary>
        public static ExamResult CreateTestExamResult(int id = 1, int examId = 1, int studentId = 1, int score = 85)
        {
            return new ExamResult
            {
                Id = id,
                ExamId = examId,
                StudentId = studentId,
                Score = score,
                Percentage = 85,
                Grade = "B",
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        #endregion

        #region ════════════════════════════════════ Attendances ════════════════════════════════════

        /// <summary>
        /// ✅ إنشاء سجل حضور طالب اختبار
        /// </summary>
        public static StudentAttendance CreateTestStudentAttendance(int id = 1, int studentId = 1, DateTime? date = null)
        {
            return new StudentAttendance
            {
                Id = id,
                StudentId = studentId,
                AttendanceDate = date ?? DateTime.Now.Date,
                CheckInTime = DateTime.Now.Date.AddHours(8),
                CheckOutTime = DateTime.Now.Date.AddHours(14),
                Status = AttendanceStatus.Present,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        /// <summary>
        /// ✅ إنشاء سجل حضور موظف اختبار
        /// </summary>
        public static EmployeeAttendance CreateTestEmployeeAttendance(int id = 1, int employeeId = 1, DateTime? date = null)
        {
            return new EmployeeAttendance
            {
                Id = id,
                EmployeeId = employeeId,
                AttendanceDate = date ?? DateTime.Now.Date,
                CheckInTime = DateTime.Now.Date.AddHours(8),
                CheckOutTime = DateTime.Now.Date.AddHours(16),
                Status = AttendanceStatus.Present,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        #endregion

        #region ════════════════════════════════════ UserRoles ════════════════════════════════════

        /// <summary>
        /// 🎭 إنشاء دور مستخدم اختبار
        /// </summary>
        public static UserRole CreateTestUserRole(int id = 1, int userId = 1, UserType roleType = UserType.Student)
        {
            return new UserRole
            {
                Id = id,
                UserId = userId,
                RoleType = roleType,
                IsPrimary = true,
                StartDate = DateTime.Now,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        #endregion

        #region ════════════════════════════════════ UserContacts ════════════════════════════════════

        /// <summary>
        /// 📞 إنشاء جهة اتصال اختبار
        /// </summary>
        public static UserContact CreateTestUserContact(int id = 1, int userId = 1, ContactType type = ContactType.Phone, string value = "01001234567")
        {
            return new UserContact
            {
                Id = id,
                UserId = userId,
                ContactType = type,
                ContactValue = value,
                IsPrimary = true,
                IsVerified = true,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        #endregion
    }
}