using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Interfaces
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🔄  واجهة وحدة العمل (IUnitOfWork)
    /// 📌  الوظيفة: تجميع جميع المستودعات في وحدة واحدة وتنسيق المعاملات
    /// 📦  الميزة: ضمان تناسق البيانات وتسهيل التعامل مع المعاملات
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        #region ════════════════════════════════════ المستودعات ════════════════════════════════════

        /// <summary>📍 مستودع المحافظات</summary>
        IGenericRepository<Governorate> Governorates { get; }

        //IGovernorateRepository Governorates { get; }
        //IDepartmentRepository Departments { get; }


        /// <summary>🏢 مستودع الإدارات</summary>
        IGenericRepository<Department> Departments { get; }

        /// <summary>🏫 مستودع المدارس</summary>
        //IGenericRepository<School> Schools { get; }
        ISchoolRepository SchoolRepository { get; }  // ✅ إضافة

        /// <summary>👤 مستودع المستخدمين</summary>
        IUserRepository Users { get; }

        /// <summary>🧑‍🎓 مستودع الطلاب</summary>
        IStudentRepository Students { get; }

        /// <summary>👨‍🏫 مستودع المعلمين</summary>
        //IGenericRepository<Teacher> Teachers { get; }
        ITeacherRepository TeacherRepository { get; }

        /// <summary>👨‍💼 مستودع الموظفين</summary>
        //IGenericRepository<Employee> Employees { get; }
        IEmployeeRepository EmployeeRepository { get; }

 


        /// <summary>📚 مستودع الصفوف الدراسية</summary>
        IGenericRepository<GradeLevel> GradeLevels { get; }

        /// <summary>🏫 مستودع الفصول الدراسية</summary>
        IGenericRepository<ClassRoom> ClassRooms { get; }

        /// <summary>📖 مستودع المواد الدراسية</summary>
        IGenericRepository<Subject> Subjects { get; }

        /// <summary>🔗 مستودع ربط المعلم بالمواد</summary>
        IGenericRepository<TeacherSubject> TeacherSubjects { get; }

        /// <summary>📅 مستودع جدول الحصص</summary>
        IGenericRepository<ClassSchedule> ClassSchedules { get; }

        /// <summary>📝 مستودع الامتحانات</summary>
        IGenericRepository<Exam> Exams { get; }

        /// <summary>📊 مستودع نتائج الامتحانات</summary>
        IGenericRepository<ExamResult> ExamResults { get; }

        /// <summary>📆 مستودع العام الدراسي</summary>
        IGenericRepository<AcademicYear> AcademicYears { get; }

        /// <summary>✅ مستودع حضور الطلاب</summary>
        IGenericRepository<StudentAttendance> StudentAttendances { get; }

        /// <summary>✅ مستودع حضور الموظفين</summary>
        IGenericRepository<EmployeeAttendance> EmployeeAttendances { get; }

        /// <summary>📞 مستودع جهات الاتصال</summary>
        IGenericRepository<UserContact> UserContacts { get; }

        /// <summary>🎭 مستودع أدوار المستخدمين</summary>
        IGenericRepository<UserRole> UserRoles { get; }

        #endregion

        #region ════════════════════════════════════ عمليات المعاملات ════════════════════════════════════

        /// <summary>
        /// 💾 حفظ جميع التغييرات في قاعدة البيانات
        /// </summary>
        /// <returns>عدد السجلات المتأثرة</returns>
        Task<int> CompleteAsync();

        /// <summary>
        /// 🔄 بدء معاملة جديدة
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// ✅ تأكيد المعاملة الحالية
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// ❌ التراجع عن المعاملة الحالية
        /// </summary>
        Task RollbackTransactionAsync();

        #endregion
    }
}