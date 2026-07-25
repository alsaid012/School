using Microsoft.EntityFrameworkCore;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Infrastructure.Data
{
    /// <summary>
    /// 🌱  بيانات افتراضية للتشغيل (Seed Data)
    /// </summary>
    public static class SeedData
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // ════════════════════════════════════════════════════════════════
            // 1. إنشاء Governorate (محافظة) إذا لم توجد
            // ════════════════════════════════════════════════════════════════
            if (!context.Governorates.Any())
            {
                var governorate = new Governorate
                {
                    Name = "القاهرة",
                    Code = "CAI",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };
                await context.Governorates.AddAsync(governorate);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════
            // 2. إنشاء Department (إدارة تعليمية) إذا لم توجد
            // ════════════════════════════════════════════════════════════════
            if (!context.Departments.Any())
            {
                var governorateId = context.Governorates.First().Id;

                var department = new Department
                {
                    GovernorateId = governorateId,
                    Name = "إدارة القاهرة التعليمية",
                    Code = "DEP-CAI-001",
                    DirectorName = "أ. د/ محمد حسن",
                    Phone = "0223456789",
                    Email = "cairo@moedu.gov.eg",
                    Address = "القاهرة - وسط البلد",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                await context.Departments.AddAsync(department);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════
            // 3. إنشاء مدرسة (إذا لم توجد)
            // ════════════════════════════════════════════════════════════════
            if (!context.Schools.Any())
            {
                var departmentId = context.Departments.First().Id;

                var school = new School
                {
                    DepartmentId = departmentId,
                    SchoolName = "مدرسة النموذجية التجريبية",
                    SchoolCode = "SCH-001",
                    SchoolType = SchoolType.Public,
                    Address = "القاهرة - مصر الجديدة",
                    Phone = "0223456789",
                    Email = "info@modelschool.edu.eg",
                    PrincipalName = "أ. د/ محمد أحمد",
                    EstablishedYear = 2000,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                await context.Schools.AddAsync(school);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════
            // 4. الحصول على المدرسة الموجودة
            // ════════════════════════════════════════════════════════════════
            var firstSchool = context.Schools.FirstOrDefault();
            if (firstSchool == null)
            {
                throw new Exception("لا توجد مدرسة في قاعدة البيانات");
            }

            var firstSchoolId = firstSchool.Id;

            // ════════════════════════════════════════════════════════════════
            // 5. إنشاء AcademicYear (سنة دراسية) إذا لم توجد
            // ════════════════════════════════════════════════════════════════
            if (!context.AcademicYears.Any())
            {
                var academicYear = new AcademicYear
                {
                    SchoolId = firstSchoolId,
                    YearName = "2024-2025",
                    StartDate = new DateTime(2024, 9, 1),
                    EndDate = new DateTime(2025, 6, 30),
                    IsCurrent = true,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                await context.AcademicYears.AddAsync(academicYear);
                await context.SaveChangesAsync();
            }

            // ════════════════════════════════════════════════════════════════
            // 6. إنشاء المستخدمين الخمسة
            // ════════════════════════════════════════════════════════════════
            var users = new List<User>
            {
                new User
                {
                    SchoolId = firstSchoolId,
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    FullName = "أحمد محمد حسن - مدير النظام",
                    NationalId = "29901010123456",
                    DateOfBirth = new DateTime(1985, 5, 15),
                    Email = "admin@school.com",
                    UserType = UserType.Admin,
                    Status = UserStatus.Active,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new User
                {
                    SchoolId = firstSchoolId,
                    Username = "principal",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Principal@123"),
                    FullName = "سعيد محمود إبراهيم - مدير المدرسة",
                    NationalId = "28802020234567",
                    DateOfBirth = new DateTime(1978, 8, 20),
                    Email = "principal@school.com",
                    UserType = UserType.Principal,
                    Status = UserStatus.Active,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new User
                {
                    SchoolId = firstSchoolId,
                    Username = "teacher",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher@123"),
                    FullName = "خالد حسن علي - معلم رياضيات",
                    NationalId = "27703030345678",
                    DateOfBirth = new DateTime(1990, 3, 10),
                    Email = "teacher@school.com",
                    UserType = UserType.Teacher,
                    Status = UserStatus.Active,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new User
                {
                    SchoolId = firstSchoolId,
                    Username = "employee",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Employee@123"),
                    FullName = "نادية سعيد محمود - موظفة إدارية",
                    NationalId = "26604040456789",
                    DateOfBirth = new DateTime(1992, 7, 25),
                    Email = "employee@school.com",
                    UserType = UserType.Employee,
                    Status = UserStatus.Active,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new User
                {
                    SchoolId = firstSchoolId,
                    Username = "student",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
                    FullName = "محمود أحمد سعيد - طالب",
                    NationalId = "25505050567890",
                    DateOfBirth = new DateTime(2008, 9, 1),
                    Email = "student@school.com",
                    UserType = UserType.Student,
                    Status = UserStatus.Active,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                }
            };

            foreach (var user in users)
            {
                if (!context.Users.Any(u => u.Username == user.Username))
                {
                    await context.Users.AddAsync(user);
                }
            }
            await context.SaveChangesAsync();

            // ════════════════════════════════════════════════════════════════
            // 7. إنشاء أدوار المستخدمين
            // ════════════════════════════════════════════════════════════════
            var existingUsers = context.Users.ToList();

            foreach (var user in existingUsers)
            {
                var existingRole = context.UserRoles.FirstOrDefault(ur => ur.UserId == user.Id);
                if (existingRole == null)
                {
                    var userRole = new UserRole
                    {
                        UserId = user.Id,
                        RoleType = user.UserType,
                        IsPrimary = true,
                        StartDate = DateTime.Now,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };

                    await context.UserRoles.AddAsync(userRole);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}