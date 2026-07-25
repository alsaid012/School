using Microsoft.EntityFrameworkCore;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Interfaces;
using SchoolERP.Infrastructure.Interceptors;

namespace SchoolERP.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly AuditInterceptor _auditInterceptor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            AuditInterceptor auditInterceptor)
            : base(options)
        {
            _auditInterceptor = auditInterceptor;
        }

        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<GradeLevel> GradeLevels { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<TeacherSubject> TeacherSubjects { get; set; }
        public DbSet<ClassSchedule> ClassSchedules { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<StudentAttendance> StudentAttendances { get; set; }
        public DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }
        public DbSet<UserContact> UserContacts { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditInterceptor);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ تطبيق الفلتر على الكيانات الأساسية فقط (Soft Delete)
            ApplySoftDeleteFilter<Governorate>(modelBuilder);
            ApplySoftDeleteFilter<Department>(modelBuilder);
            ApplySoftDeleteFilter<School>(modelBuilder);
            ApplySoftDeleteFilter<User>(modelBuilder);
            ApplySoftDeleteFilter<Student>(modelBuilder);
            ApplySoftDeleteFilter<Teacher>(modelBuilder);
            ApplySoftDeleteFilter<Employee>(modelBuilder);
            ApplySoftDeleteFilter<Subject>(modelBuilder);
            ApplySoftDeleteFilter<GradeLevel>(modelBuilder);
            ApplySoftDeleteFilter<ClassRoom>(modelBuilder);
            ApplySoftDeleteFilter<AcademicYear>(modelBuilder);

            //// ✅ تطبيق Global Query Filter لـ Soft Delete
            //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            //{
            //    if (typeof(ISoftDeleteEntity).IsAssignableFrom(entityType.ClrType))
            //    {
            //        var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
            //        var property = System.Linq.Expressions.Expression.Property(parameter, "IsDeleted");
            //        var condition = System.Linq.Expressions.Expression.Equal(property, System.Linq.Expressions.Expression.Constant(false));
            //        var lambda = System.Linq.Expressions.Expression.Lambda(condition, parameter);

            //        modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            //    }
            //}

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }


        // ✅ طريقة مساعدة لتطبيق الفلتر
        private void ApplySoftDeleteFilter<T>(ModelBuilder modelBuilder) where T : class, ISoftDeleteEntity
        {
            modelBuilder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }


        // ✅ Override SaveChanges لتعيين التواريخ تلقائياً
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                // ✅ تعيين CreatedAt و UpdatedAt تلقائياً
                if (entry.Entity is IBaseEntity entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = DateTime.Now;
                        entity.IsActive = true;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entity.UpdatedAt = DateTime.Now;
                    }
                }

                // ✅ تعيين DeletedAt تلقائياً للـ Soft Delete
                if (entry.Entity is ISoftDeleteEntity softDeleteEntity)
                {
                    if (entry.State == EntityState.Modified && softDeleteEntity.IsDeleted)
                    {
                        softDeleteEntity.DeletedAt = DateTime.Now;
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}