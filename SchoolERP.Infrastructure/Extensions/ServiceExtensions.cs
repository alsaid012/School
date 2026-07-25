using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Application.Services;
using SchoolERP.Infrastructure.Data;
using SchoolERP.Infrastructure.Interceptors;
using SchoolERP.Infrastructure.Repositories;

namespace SchoolERP.Infrastructure.Extensions
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🔧  إضافات تسجيل الخدمات (Service Extensions)
    /// 📌  الوظيفة: تسجيل جميع خدمات الـ Infrastructure في الـ DI Container
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 🔧 تسجيل خدمات الـ Infrastructure
        /// </summary>
        /// <param name="services">مجموعة الخدمات</param>
        /// <param name="configuration">الإعدادات</param>
        /// <returns>مجموعة الخدمات بعد التسجيل</returns>
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ════════════════════════════════════════════════════════════════
            // 📦 تسجيل Audit Interceptor
            // ════════════════════════════════════════════════════════════════
            services.AddScoped<AuditInterceptor>();

            // ════════════════════════════════════════════════════════════════
            // 📦 تسجيل قاعدة البيانات
            // ════════════════════════════════════════════════════════════════
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var interceptor = serviceProvider.GetRequiredService<AuditInterceptor>();
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
                options.AddInterceptors(interceptor);
            });

            // ════════════════════════════════════════════════════════════════
            // 📦 تسجيل المستودعات العامة
            // ════════════════════════════════════════════════════════════════
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // ════════════════════════════════════════════════════════════════
            // 📦 تسجيل المستودعات الخاصة
            // ════════════════════════════════════════════════════════════════
            // ✅ Auth
            services.AddScoped<IAuthService, AuthService>();

            // ✅ الرئيسية
            services.AddScoped<IGovernorateService, GovernorateService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ISchoolService, SchoolService>();

            // ✅ المستخدمين
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            // ✅ الصفوف والفصول
            services.AddScoped<IGradeLevelService, GradeLevelService>();
            services.AddScoped<IClassRoomService, ClassRoomService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ITeacherSubjectService, TeacherSubjectService>();

            // ✅ الجدول والامتحانات
            services.AddScoped<IClassScheduleService, ClassScheduleService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IExamResultService, ExamResultService>();
            services.AddScoped<IAcademicYearService, AcademicYearService>();

            // ✅ الحضور
            services.AddScoped<IStudentAttendanceService, StudentAttendanceService>();
            services.AddScoped<IEmployeeAttendanceService, EmployeeAttendanceService>();

            // ✅ جهات الاتصال والأدوار
            services.AddScoped<IUserContactService, UserContactService>();
            services.AddScoped<IUserRoleService, UserRoleService>();

            // ✅ التقارير
            services.AddScoped<IReportService, ReportService>();



            // ════════════════════════════════════════════════════════════════
            // 📦 تسجيل وحدة العمل (UnitOfWork)
            // ════════════════════════════════════════════════════════════════
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}