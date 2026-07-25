using Microsoft.Extensions.DependencyInjection;
using SchoolERP.Application.Interfaces.Services;
using SchoolERP.Application.Services;

namespace SchoolERP.Application.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IGovernorateService, GovernorateService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ISchoolService, SchoolService>();
            services.AddScoped<IGradeLevelService, GradeLevelService>();
            services.AddScoped<IClassRoomService, ClassRoomService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IExamResultService, ExamResultService>();
            services.AddScoped<IReportService, ReportService>();

            return services;
        }
    }
}