using Microsoft.EntityFrameworkCore.Storage;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace SchoolERP.Infrastructure.Data
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🔄  وحدة العمل (UnitOfWork)
    /// 📌  الوظيفة: تنفيذ واجهة IUnitOfWork وإدارة جميع المستودعات
    /// 🔧  الميزة: ضمان تناسق البيانات عبر معاملات متكاملة
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;
        private bool _disposed;

        #endregion

        #region ════════════════════════════════════ المستودعات ════════════════════════════════════


        private IGenericRepository<Governorate>? _governorateRepository;
        private IGenericRepository<Department>? _departmentRepository;

        //private IGenericRepository<School>? _schoolRepository;
        private ISchoolRepository? _schoolRepository;

        private IUserRepository? _userRepository;
        private IStudentRepository? _studentRepository;
        //private IGenericRepository<Teacher>? _teacherRepository;
        private ITeacherRepository? _teacherRepository;
        //private IGenericRepository<Employee>? _employeeRepository;
        private IEmployeeRepository? _employeeRepository;

        private IGenericRepository<GradeLevel>? _gradeLevelRepository;
        private IGenericRepository<ClassRoom>? _classRoomRepository;
        private IGenericRepository<Subject>? _subjectRepository;
        private IGenericRepository<TeacherSubject>? _teacherSubjectRepository;
        private IGenericRepository<ClassSchedule>? _classScheduleRepository;
        private IGenericRepository<Exam>? _examRepository;


        private IGenericRepository<ExamResult>? _examResultRepository;
        //private IExamResultRepository? _examResultRepository;

        private IGenericRepository<AcademicYear>? _academicYearRepository;
        private IGenericRepository<StudentAttendance>? _studentAttendanceRepository;
        private IGenericRepository<EmployeeAttendance>? _employeeAttendanceRepository;
        private IGenericRepository<UserContact>? _userContactRepository;
        private IGenericRepository<UserRole>? _userRoleRepository;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        /// <summary>
        /// المُنشئ - يستقبل قاعدة البيانات
        /// </summary>
        /// <param name="context">قاعدة البيانات</param>
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region ════════════════════════════════════ تنفيذ المستودعات ════════════════════════════════════

        /// <summary>👤 مستودع المستخدمين</summary>
        public IUserRepository Users =>
            _userRepository ??= new UserRepository(_context);

        /// <summary>🧑‍🎓 مستودع الطلاب</summary>
        public IStudentRepository Students =>
            _studentRepository ??= new StudentRepository(_context);

        /// <summary>👨‍🏫 مستودع المعلمين</summary>
        //public IGenericRepository<Teacher> Teachers =>
        //    _teacherRepository ??= new GenericRepository<Teacher>(_context);

        public ITeacherRepository TeacherRepository =>
            _teacherRepository ??= new TeacherRepository(_context);


        /// <summary>👨‍💼 مستودع الموظفين</summary>
        //public IGenericRepository<Employee> Employees =>
        //    _employeeRepository ??= new GenericRepository<Employee>(_context);
        public IEmployeeRepository EmployeeRepository =>
       _employeeRepository ??= new EmployeeRepository(_context);

        /// <summary>🏫 مستودع المدارس</summary>
        //public IGenericRepository<School> Schools =>
        //    _schoolRepository ??= new GenericRepository<School>(_context);
        public ISchoolRepository SchoolRepository =>
       _schoolRepository ??= new SchoolRepository(_context);

        /// <summary>🏢 مستودع الإدارات</summary>
        public IGenericRepository<Department> Departments =>
            _departmentRepository ??= new GenericRepository<Department>(_context);

        /// <summary>📍 مستودع المحافظات</summary>
        public IGenericRepository<Governorate> Governorates =>
            _governorateRepository ??= new GenericRepository<Governorate>(_context);

        /// <summary>📚 مستودع الصفوف الدراسية</summary>
        public IGenericRepository<GradeLevel> GradeLevels =>
            _gradeLevelRepository ??= new GenericRepository<GradeLevel>(_context);

        /// <summary>🏫 مستودع الفصول الدراسية</summary>
        public IGenericRepository<ClassRoom> ClassRooms =>
            _classRoomRepository ??= new GenericRepository<ClassRoom>(_context);

        /// <summary>📖 مستودع المواد الدراسية</summary>
        public IGenericRepository<Subject> Subjects =>
            _subjectRepository ??= new GenericRepository<Subject>(_context);

        /// <summary>🔗 مستودع ربط المعلم بالمواد</summary>
        public IGenericRepository<TeacherSubject> TeacherSubjects =>
            _teacherSubjectRepository ??= new GenericRepository<TeacherSubject>(_context);

        /// <summary>📅 مستودع جدول الحصص</summary>
        public IGenericRepository<ClassSchedule> ClassSchedules =>
            _classScheduleRepository ??= new GenericRepository<ClassSchedule>(_context);

        /// <summary>📝 مستودع الامتحانات</summary>
        public IGenericRepository<Exam> Exams =>
            _examRepository ??= new GenericRepository<Exam>(_context);

        /// <summary>📊 مستودع نتائج الامتحانات</summary>
        
        //public IExamResultRepository ExamResults =>
        //_examResultRepository ??= new ExamResultRepository(_context);

        public IGenericRepository<ExamResult> ExamResults =>
            _examResultRepository ??= new GenericRepository<ExamResult>(_context);

        /// <summary>📆 مستودع العام الدراسي</summary>
        public IGenericRepository<AcademicYear> AcademicYears =>
            _academicYearRepository ??= new GenericRepository<AcademicYear>(_context);

        /// <summary>✅ مستودع حضور الطلاب</summary>
        public IGenericRepository<StudentAttendance> StudentAttendances =>
            _studentAttendanceRepository ??= new GenericRepository<StudentAttendance>(_context);

        /// <summary>✅ مستودع حضور الموظفين</summary>
        public IGenericRepository<EmployeeAttendance> EmployeeAttendances =>
            _employeeAttendanceRepository ??= new GenericRepository<EmployeeAttendance>(_context);

        /// <summary>📞 مستودع جهات الاتصال</summary>
        public IGenericRepository<UserContact> UserContacts =>
            _userContactRepository ??= new GenericRepository<UserContact>(_context);

        /// <summary>🎭 مستودع أدوار المستخدمين</summary>
        public IGenericRepository<UserRole> UserRoles =>
            _userRoleRepository ??= new GenericRepository<UserRole>(_context);

        #endregion

        #region ════════════════════════════════════ عمليات المعاملات ════════════════════════════════════

        /// <summary>
        /// 💾 حفظ جميع التغييرات في قاعدة البيانات
        /// </summary>
        public async Task<int> CompleteAsync()
        {
            try
            {
                var result = await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                // ✅ سجل الخطأ
                Console.WriteLine($"Error in SaveChanges: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 🔄 بدء معاملة جديدة
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// ✅ تأكيد المعاملة الحالية
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// ❌ التراجع عن المعاملة الحالية
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        #endregion

        #region ════════════════════════════════════ التخلص من الموارد ════════════════════════════════════

        /// <summary>
        /// 🗑️ التخلص من الموارد المستخدمة
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        #endregion
    }
}