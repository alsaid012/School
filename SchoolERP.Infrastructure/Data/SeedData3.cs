using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Infrastructure.Data
{
    public static class SeedData3
    {
        private static readonly Random _random = new Random();
        private static ILogger? _logger;

        public static async Task SeedAsync(
            ApplicationDbContext context,
            ILogger? logger = null,
            CancellationToken cancellationToken = default )
        {
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            _logger = logger;


            try
            {
                // ════════════════════════════════════════════════════════════════
                // 1. المحافظات (Governorates) - 26 محافظة (تم إزالة التكرار)
                // ════════════════════════════════════════════════════════════════
                if (!await context.Governorates.AnyAsync(cancellationToken))
                {
                    var governorates = new List<Governorate>
                    {
                        new Governorate { Name = "القاهرة", Code = "CAI" },
                        new Governorate { Name = "الجيزة", Code = "GIZ" },
                        new Governorate { Name = "الإسكندرية", Code = "ALX" },
                        new Governorate { Name = "الدقهلية", Code = "DK" },
                        new Governorate { Name = "الشرقية", Code = "SH" },
                        new Governorate { Name = "الغربية", Code = "GH" },
                        new Governorate { Name = "المنوفية", Code = "MN" },
                        new Governorate { Name = "القليوبية", Code = "QL" },
                        new Governorate { Name = "سوهاج", Code = "SHG" },
                        new Governorate { Name = "أسيوط", Code = "AST" },
                        new Governorate { Name = "الأقصر", Code = "LXR" },
                        new Governorate { Name = "أسوان", Code = "ASW" },
                        new Governorate { Name = "البحيرة", Code = "BHR" },
                        new Governorate { Name = "بورسعيد", Code = "PSC" },
                        new Governorate { Name = "دمياط", Code = "DMT" },
                        new Governorate { Name = "الإسماعيلية", Code = "ISM" },
                        new Governorate { Name = "السويس", Code = "SUZ" },
                        new Governorate { Name = "كفر الشيخ", Code = "KFS" },
                        new Governorate { Name = "المنيا", Code = "MNY" },
                        new Governorate { Name = "بني سويف", Code = "BNS" },
                        new Governorate { Name = "الفيوم", Code = "FYM" },
                        new Governorate { Name = "قنا", Code = "QNA" },
                        new Governorate { Name = "مرسى مطروح", Code = "MTH" },
                        new Governorate { Name = "شمال سيناء", Code = "NSI" },
                        new Governorate { Name = "جنوب سيناء", Code = "SSI" },
                        new Governorate { Name = "الوادي الجديد", Code = "WJD" }
                    };

                    await context.Governorates.AddRangeAsync(governorates, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                // ════════════════════════════════════════════════════════════════
                // 2. الإدارات التعليمية (Departments) - 15 إدارة
                // ════════════════════════════════════════════════════════════════
                if (!await context.Departments.AnyAsync(cancellationToken))
                {
                    var governorates = await context.Governorates.ToListAsync(cancellationToken);
                    var cairoId = governorates.First(g => g.Code == "CAI").Id;
                    var gizaId = governorates.First(g => g.Code == "GIZ").Id;
                    var alexId = governorates.First(g => g.Code == "ALX").Id;
                    var dkId = governorates.First(g => g.Code == "DK").Id;
                    var shId = governorates.First(g => g.Code == "SH").Id;

                    var departments = new List<Department>
                    {
                        new Department { GovernorateId = cairoId, Name = "إدارة شمال القاهرة التعليمية", Code = "SH-NORTH-CAIRO", DirectorName = "أ. محمد أحمد", Phone = "0223456789", Email = "north.cairo@moedu.gov.eg", Address = "شمال القاهرة - مصر الجديدة" },
                        new Department { GovernorateId = cairoId, Name = "إدارة جنوب القاهرة التعليمية", Code = "SH-SOUTH-CAIRO", DirectorName = "أ. خالد حسن", Phone = "0229876543", Email = "south.cairo@moedu.gov.eg", Address = "جنوب القاهرة - المعادي" },
                        new Department { GovernorateId = cairoId, Name = "إدارة شرق القاهرة التعليمية", Code = "SH-EAST-CAIRO", DirectorName = "أ. سعيد محمود", Phone = "0223456790", Email = "east.cairo@moedu.gov.eg", Address = "شرق القاهرة - مدينة نصر" },
                        new Department { GovernorateId = cairoId, Name = "إدارة غرب القاهرة التعليمية", Code = "SH-WEST-CAIRO", DirectorName = "أ. نادية سعيد", Phone = "0223456791", Email = "west.cairo@moedu.gov.eg", Address = "غرب القاهرة - الدقي" },
                        new Department { GovernorateId = gizaId, Name = "إدارة الجيزة التعليمية", Code = "SH-GIZA", DirectorName = "أ. ياسر محمد", Phone = "0234567890", Email = "giza@moedu.gov.eg", Address = "الجيزة - الدقي" },
                        new Department { GovernorateId = gizaId, Name = "إدارة جنوب الجيزة التعليمية", Code = "SH-SOUTH-GIZA", DirectorName = "أ. هاني سعيد", Phone = "0234567891", Email = "south.giza@moedu.gov.eg", Address = "جنوب الجيزة - البدرشين" },
                        new Department { GovernorateId = alexId, Name = "إدارة الإسكندرية التعليمية", Code = "SH-ALEX", DirectorName = "أ. فاطمة علي", Phone = "0334567890", Email = "alex@moedu.gov.eg", Address = "الإسكندرية - سيدي جابر" },
                        new Department { GovernorateId = dkId, Name = "إدارة الدقهلية التعليمية", Code = "SH-DK", DirectorName = "أ. محمود حسن", Phone = "0553456789", Email = "dk@moedu.gov.eg", Address = "الدقهلية - المنصورة" },
                        new Department { GovernorateId = shId, Name = "إدارة الشرقية التعليمية", Code = "SH-SH", DirectorName = "أ. سمير أحمد", Phone = "0554456789", Email = "sh@moedu.gov.eg", Address = "الشرقية - الزقازيق" },
                        new Department { GovernorateId = cairoId, Name = "إدارة الوايلي التعليمية", Code = "SH-WAILY", DirectorName = "أ. كريم محمد", Phone = "0223456792", Email = "waily@moedu.gov.eg", Address = "الوايلي - القاهرة" },
                        new Department { GovernorateId = cairoId, Name = "إدارة الزيتون التعليمية", Code = "SH-ZEITOON", DirectorName = "أ. شيرين عادل", Phone = "0223456793", Email = "zeetoon@moedu.gov.eg", Address = "الزيتون - القاهرة" },
                        new Department { GovernorateId = gizaId, Name = "إدارة أكتوبر التعليمية", Code = "SH-OCTOBER", DirectorName = "أ. عمرو خالد", Phone = "0234567892", Email = "october@moedu.gov.eg", Address = "مدينة 6 أكتوبر - الجيزة" },
                        new Department { GovernorateId = gizaId, Name = "إدارة الشيخ زايد التعليمية", Code = "SH-SHEIKH-ZAYED", DirectorName = "أ. منى سعيد", Phone = "0234567893", Email = "sheikh.zayed@moedu.gov.eg", Address = "الشيخ زايد - الجيزة" },
                        new Department { GovernorateId = cairoId, Name = "إدارة مصر الجديدة التعليمية", Code = "SH-HELIOPOLIS", DirectorName = "أ. طارق محمود", Phone = "0223456794", Email = "heliopolis@moedu.gov.eg", Address = "مصر الجديدة - القاهرة" },
                        new Department { GovernorateId = cairoId, Name = "إدارة المعادي التعليمية", Code = "SH-MAADI", DirectorName = "أ. دينا محمد", Phone = "0223456795", Email = "maadi@moedu.gov.eg", Address = "المعادي - القاهرة" }
                    };

                    await context.Departments.AddRangeAsync(departments, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                // ════════════════════════════════════════════════════════════════
                // 3. المدارس (Schools) - 40 مدرسة
                // ════════════════════════════════════════════════════════════════
                if (!await context.Schools.AnyAsync(cancellationToken))
                {
                    var depts = await context.Departments.ToListAsync(cancellationToken);
                    var schools = new List<School>();

                    var schoolNames = new[]
                    {
                        "النصر", "السلام", "الأمل", "النهضة", "التوفيق", "النجاح", "التميز", "الإخلاص",
                        "الوفاء", "العمل", "الحرية", "المستقبل", "القمة", "الريادة", "العبور", "الواحة",
                        "الزهور", "الأزهار", "النخبة", "الأوائل", "الرواد", "النجوم", "البناة", "الأمانة",
                        "الصدق", "الكمال", "القدوة", "الطموح", "الارتقاء", "الإبداع", "الابتكار", "الاجتهاد"
                    };

                    var schoolTypes = new[] { SchoolType.Public, SchoolType.Private, SchoolType.Language, SchoolType.International };

                    for (int i = 0; i < 40; i++)
                    {
                        var dept = depts[i % depts.Count];
                        var nameIndex = i % schoolNames.Length;
                        var typeIndex = i % schoolTypes.Length;

                        schools.Add(new School
                        {
                            DepartmentId = dept.Id,
                            SchoolName = $"مدرسة {schoolNames[nameIndex]} {GetSchoolStage(i)}",
                            SchoolCode = $"SCH-{1000 + i}",
                            SchoolType = schoolTypes[typeIndex],
                            Address = $"{dept.Address} - المنطقة {i + 1}",
                            Phone = $"0{_random.Next(10, 99)}{_random.Next(10000000, 99999999)}",
                            Email = $"school{i + 1}@edu.eg",
                            PrincipalName = $"أ. {GetRandomPrincipalName(i)}",
                            EstablishedYear = 1990 + (i % 30),
                            IsActive = true,
                            CreatedAt = DateTime.Now
                        });
                    }

                    await context.Schools.AddRangeAsync(schools, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                // ════════════════════════════════════════════════════════════════
                // 4. المستخدمين (Users) - 500+ مستخدم
                // ════════════════════════════════════════════════════════════════
                if (!await context.Users.AnyAsync(cancellationToken))
                {
                    var schools = await context.Schools.ToListAsync(cancellationToken);
                    var users = new List<User>();
                    var userTypes = new[] { UserType.Student, UserType.Student, UserType.Student, UserType.Teacher, UserType.Employee };
                    var statuses = new[] { UserStatus.Active, UserStatus.Active, UserStatus.Active, UserStatus.Active, UserStatus.Pending };

                    // ✅ تحسين الأداء: تخزين الـ hash مرة واحدة
                    var adminPasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");
                    var userPasswordHash = BCrypt.Net.BCrypt.HashPassword("Password@123");

                    // Admin User
                    var adminUser = new User
                    {
                        SchoolId = schools.First().Id,
                        Username = "admin",
                        PasswordHash = adminPasswordHash,
                        FullName = "مدير النظام",
                        NationalId = "00000000000000",
                        Email = "admin@system.com",
                        DateOfBirth = new DateTime(1980, 1, 1),
                        Address = "مصر الجديدة - القاهرة",
                        UserType = UserType.Admin,
                        Status = UserStatus.Active,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };
                    users.Add(adminUser);

                    // Create 500+ users
                    for (int i = 1; i <= 500; i++)
                    {
                        var school = schools[i % schools.Count];
                        var userType = userTypes[i % userTypes.Length];
                        var status = statuses[i % statuses.Length];
                        var firstName = GetRandomFirstName(i);
                        var lastName = GetRandomLastName(i);

                        var user = new User
                        {
                            SchoolId = school.Id,
                            Username = $"{firstName.ToLower()}.{lastName.ToLower()}{i}",
                            PasswordHash = userPasswordHash, // ✅ استخدام الـ hash المخزن
                            FullName = $"{firstName} {lastName}",
                            NationalId = $"{GetRandomNationalId(i)}",
                            Email = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@example.com",
                            DateOfBirth = GetRandomDateOfBirth(i),
                            Address = $"{school.Address} - شقة {i}",
                            UserType = userType,
                            Status = status,
                            IsActive = true,
                            CreatedAt = DateTime.Now.AddDays(-i)
                        };
                        users.Add(user);
                    }

                    await context.Users.AddRangeAsync(users, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                // ════════════════════════════════════════════════════════════════
                // 5. الصفوف الدراسية (GradeLevels) - 9 صفوف لكل مدرسة
                // ════════════════════════════════════════════════════════════════
                if (!await context.GradeLevels.AnyAsync(cancellationToken))
                {
                    var schools = await context.Schools.ToListAsync(cancellationToken);
                    var gradeLevels = new List<GradeLevel>();

                    var stages = new[]
                    {
                        new { Stage = GradeStage.Primary, Name = "ابتدائي", Grades = new[] { "الأول", "الثاني", "الثالث", "الرابع", "الخامس", "السادس" } },
                        new { Stage = GradeStage.Preparatory, Name = "إعدادي", Grades = new[] { "الأول", "الثاني", "الثالث" } },
                        new { Stage = GradeStage.Secondary, Name = "ثانوي", Grades = new[] { "الأول", "الثاني", "الثالث" } }
                    };

                    foreach (var school in schools)
                    {
                        int gradeNumber = 1;
                        foreach (var stage in stages)
                        {
                            foreach (var gradeName in stage.Grades)
                            {
                                gradeLevels.Add(new GradeLevel
                                {
                                    SchoolId = school.Id,
                                    GradeName = $"الصف {gradeName} {stage.Name}",
                                    GradeNumber = gradeNumber++,
                                    GradeStage = stage.Stage,
                                    Description = $"الصف {gradeName} {stage.Name} - {school.SchoolName}",
                                    IsActive = true,
                                    CreatedAt = DateTime.Now
                                });
                            }
                        }
                    }

                    await context.GradeLevels.AddRangeAsync(gradeLevels, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                // ════════════════════════════════════════════════════════════════
                // 6. الفصول الدراسية (ClassRooms) - 4 فصول لكل صف
                // ════════════════════════════════════════════════════════════════
                if (!await context.ClassRooms.AnyAsync(cancellationToken))
                {
                    var gradeLevels = await context.GradeLevels.ToListAsync(cancellationToken);
                    var classRooms = new List<ClassRoom>();
                    var classNames = new[] { "أ", "ب", "ج", "د" };

                    foreach (var grade in gradeLevels)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            classRooms.Add(new ClassRoom
                            {
                                GradeLevelId = grade.Id,
                                ClassName = $"{grade.GradeNumber}/{classNames[i]}",
                                ClassCode = $"CLS-{grade.Id}-{i + 1}",
                                RoomNumber = $"{grade.Id}{i + 1}",
                                Capacity = 25 + (i * 5),
                                HasSmartBoard = i % 2 == 0,
                                HasProjector = i % 2 == 1,
                                IsActive = true,
                                CreatedAt = DateTime.Now
                            });
                        }
                    }

                    await context.ClassRooms.AddRangeAsync(classRooms, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                // ════════════════════════════════════════════════════════════════
                // 7. المواد الدراسية (Subjects) - 10 مواد لكل صف
                // ════════════════════════════════════════════════════════════════
                if (!await context.Subjects.AnyAsync(cancellationToken))
                {
                    var gradeLevels = await context.GradeLevels.ToListAsync(cancellationToken);
                    var subjectNames = new[]
                    {
                        "اللغة العربية", "اللغة الإنجليزية", "الرياضيات", "العلوم", "التاريخ",
                        "الجغرافيا", "التربية الدينية", "الحاسوب", "التربية الفنية", "الموسيقى"
                    };

                    var subjects = new List<Subject>();

                    foreach (var grade in gradeLevels)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            subjects.Add(new Subject
                            {
                                GradeLevelId = grade.Id,
                                SubjectName = subjectNames[i % subjectNames.Length],
                                SubjectCode = $"SUB-{grade.Id}-{i + 1}",
                                WeeklyHours = 2 + (i % 4),
                                IsRequired = i < 7,
                                Description = $"مادة {subjectNames[i % subjectNames.Length]} - {grade.GradeName}",
                                IsActive = true,
                                CreatedAt = DateTime.Now
                            });
                        }
                    }

                    await context.Subjects.AddRangeAsync(subjects, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                // ════════════════════════════════════════════════════════════════
                // 8. العام الدراسي (AcademicYear)
                // ════════════════════════════════════════════════════════════════
                if (!await context.AcademicYears.AnyAsync(cancellationToken))
                {
                    var schools = await context.Schools.ToListAsync(cancellationToken);
                    var academicYears = new List<AcademicYear>();

                    foreach (var school in schools)
                    {
                        // ✅ السنة الحالية فقط
                        academicYears.Add(new AcademicYear
                        {
                            SchoolId = school.Id,
                            YearName = "2024-2025",
                            StartDate = new DateTime(2024, 9, 1),
                            EndDate = new DateTime(2025, 6, 30),
                            IsCurrent = true,
                            IsActive = true,
                            CreatedAt = DateTime.Now
                        });

                        // السنة القادمة (غير حالية)
                        academicYears.Add(new AcademicYear
                        {
                            SchoolId = school.Id,
                            YearName = "2025-2026",
                            StartDate = new DateTime(2025, 9, 1),
                            EndDate = new DateTime(2026, 6, 30),
                            IsCurrent = false,
                            IsActive = true,
                            CreatedAt = DateTime.Now
                        });
                    }

                    await context.AcademicYears.AddRangeAsync(academicYears, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                // ════════════════════════════════════════════════════════════════
                // 9. المعلمين (Teachers) - 3 معلمين لكل مادة
                // ════════════════════════════════════════════════════════════════
                if (!await context.Teachers.AnyAsync(cancellationToken))
                {
                    var users = await context.Users
                        .Where(u => u.UserType == UserType.Teacher)
                        .ToListAsync(cancellationToken);

                    var subjects = await context.Subjects.ToListAsync(cancellationToken);
                    var teachers = new List<Teacher>();

                    // ✅ التحقق من وجود معلمين كافيين
                    if (users.Count >= 3)
                    {
                        foreach (var subject in subjects.Take(500))
                        {
                            var teacherUsers = users
                                .Skip(subject.Id % Math.Max(1, users.Count - 3))
                                .Take(3)
                                .ToList();

                            foreach (var user in teacherUsers)
                            {
                                teachers.Add(new Teacher
                                {
                                    UserId = user.Id,
                                    TeacherCode = $"TCH-{subject.Id}-{user.Id}",
                                    Qualification = GetRandomQualification(subject.Id),
                                    Specialization = subject.SubjectName,
                                    HireDate = DateTime.Now.AddYears(-_random.Next(1, 20)),
                                    Salary = 3000 + (subject.Id % 5000),
                                    IsHomeroomTeacher = subject.Id % 5 == 0,
                                    IsActive = true,
                                    CreatedAt = DateTime.Now
                                });
                            }
                        }
                    }

                    if (teachers.Any())
                    {
                        await context.Teachers.AddRangeAsync(teachers, cancellationToken);
                        await context.SaveChangesAsync(cancellationToken);
                    }
                }

                // ════════════════════════════════════════════════════════════════
                // 10. ربط المعلم بالمادة (TeacherSubject)
                // ════════════════════════════════════════════════════════════════
                if (!await context.TeacherSubjects.AnyAsync(cancellationToken))
                {
                    var teachers = await context.Teachers
                        .Include(t => t.User)
                        .ToListAsync(cancellationToken);

                    var teacherSubjects = new List<TeacherSubject>();

                    foreach (var teacher in teachers)
                    {
                        // ✅ إضافة Include لحل مشكلة GradeLevel.SchoolId
                        var subjectIds = await context.Subjects
                            .Include(s => s.GradeLevel)
                            .Where(s => s.GradeLevel.SchoolId == teacher.User.SchoolId)
                            .Select(s => s.Id)
                            .Take(_random.Next(1, 4))
                            .ToListAsync(cancellationToken);

                        foreach (var subjectId in subjectIds)
                        {
                            teacherSubjects.Add(new TeacherSubject
                            {
                                TeacherId = teacher.Id,
                                SubjectId = subjectId,
                                IsPrimary = subjectIds.First() == subjectId,
                                IsActive = true,
                                CreatedAt = DateTime.Now
                            });
                        }
                    }

                    if (teacherSubjects.Any())
                    {
                        await context.TeacherSubjects.AddRangeAsync(teacherSubjects, cancellationToken);
                        await context.SaveChangesAsync(cancellationToken);
                    }
                }

                // ════════════════════════════════════════════════════════════════
                // 11. الطلاب (Students)
                // ════════════════════════════════════════════════════════════════
                if (!await context.Students.AnyAsync(cancellationToken))
                {
                    var users = await context.Users
                        .Where(u => u.UserType == UserType.Student)
                        .ToListAsync(cancellationToken);

                    var classRooms = await context.ClassRooms.ToListAsync(cancellationToken);
                    var academicYears = await context.AcademicYears
                        .Where(ay => ay.IsCurrent)
                        .ToListAsync(cancellationToken);

                    var students = new List<Student>();

                    foreach (var user in users)
                    {
                        var classRoom = classRooms[user.Id % classRooms.Count];
                        var academicYear = academicYears[user.Id % academicYears.Count];

                        students.Add(new Student
                        {
                            UserId = user.Id,
                            StudentCode = $"STU-{2024}-{user.Id.ToString().PadLeft(4, '0')}",
                            AcademicYearId = academicYear.Id,
                            ClassRoomId = classRoom.Id,
                            ParentName = GetRandomParentName(user.Id),
                            ParentPhone = $"01{_random.Next(0, 5)}{_random.Next(10000000, 99999999)}",
                            ParentEmail = $"parent{user.Id}@example.com",
                            EnrollmentDate = DateTime.Now.AddDays(-user.Id),
                            IsGraduated = user.Id % 10 == 0,
                            IsActive = true,
                            CreatedAt = DateTime.Now
                        });
                    }

                    await context.Students.AddRangeAsync(students, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                // ════════════════════════════════════════════════════════════════
                // 12. الموظفين (Employees)
                // ════════════════════════════════════════════════════════════════
                if (!await context.Employees.AnyAsync(cancellationToken))
                {
                    var users = await context.Users
                        .Where(u => u.UserType == UserType.Employee)
                        .ToListAsync(cancellationToken);

                    var employees = new List<Employee>();
                    var jobTitles = new[]
                    {
                        "مدير شؤون الطلاب", "أمين مكتبة", "محاسب", "سكرتير", "مشرف نشاط",
                        "أخصائي نفسي", "أخصائي اجتماعي", "مسؤول تكنولوجيا المعلومات", "مدير موارد بشرية",
                        "منسق أنشطة", "مدير مالي", "مساعد إداري", "حارس أمن", "سائق"
                    };

                    foreach (var user in users)
                    {
                        employees.Add(new Employee
                        {
                            UserId = user.Id,
                            EmployeeCode = $"EMP-{user.Id.ToString().PadLeft(4, '0')}",
                            JobTitle = jobTitles[user.Id % jobTitles.Length],
                            Department = jobTitles[user.Id % jobTitles.Length],
                            HireDate = DateTime.Now.AddYears(-_random.Next(1, 15)),
                            Salary = 2500 + (user.Id % 4000),
                            IsActive = true,
                            CreatedAt = DateTime.Now
                        });
                    }

                    await context.Employees.AddRangeAsync(employees, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                // ════════════════════════════════════════════════════════════════
                // 13. جدول الحصص (ClassSchedules)
                // ════════════════════════════════════════════════════════════════
                if (!await context.ClassSchedules.AnyAsync(cancellationToken))
                {
                    var classRooms = await context.ClassRooms
                        .Take(200)
                        .ToListAsync(cancellationToken);

                    var academicYears = await context.AcademicYears
                        .Where(ay => ay.IsCurrent)
                        .ToListAsync(cancellationToken);

                    var classSchedules = new List<ClassSchedule>();
                    var days = new[] { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday };

                    foreach (var classRoom in classRooms)
                    {
                        var subjects = await context.Subjects
                            .Where(s => s.GradeLevelId == classRoom.GradeLevelId)
                            .Take(6)
                            .ToListAsync(cancellationToken);

                        foreach (var day in days)
                        {
                            for (int period = 1; period <= 6; period++)
                            {
                                var subject = subjects[period % subjects.Count];

                                var teacherSubjects = await context.TeacherSubjects
                                    .Where(ts => ts.SubjectId == subject.Id)
                                    .ToListAsync(cancellationToken);

                                if (!teacherSubjects.Any()) continue;

                                var teacherSubject = teacherSubjects[period % teacherSubjects.Count];
                                var startTime = new TimeSpan(8 + period - 1, 0, 0);
                                var endTime = startTime.Add(new TimeSpan(0, 45, 0));

                                classSchedules.Add(new ClassSchedule
                                {
                                    AcademicYearId = academicYears.First().Id,
                                    ClassRoomId = classRoom.Id,
                                    SubjectId = subject.Id,
                                    TeacherId = teacherSubject.TeacherId,
                                    DayOfWeek = day,
                                    StartTime = startTime,
                                    EndTime = endTime,
                                    PeriodNumber = period,
                                    IsActive = true,
                                    CreatedAt = DateTime.Now
                                });
                            }
                        }
                    }

                    await context.ClassSchedules.AddRangeAsync(classSchedules, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                // ════════════════════════════════════════════════════════════════
                // 14. الامتحانات (Exams)
                // ════════════════════════════════════════════════════════════════
                if (!await context.Exams.AnyAsync(cancellationToken))
                {
                    var subjects = await context.Subjects
                        .Take(500)
                        .ToListAsync(cancellationToken);

                    var academicYears = await context.AcademicYears
                        .Where(ay => ay.IsCurrent)
                        .ToListAsync(cancellationToken);

                    var exams = new List<Exam>();
                    var examTypes = new[] { ExamType.Monthly, ExamType.MidTerm, ExamType.Final, ExamType.Quiz, ExamType.Assessment };

                    foreach (var subject in subjects)
                    {
                        var classRooms = await context.ClassRooms
                            .Where(cr => cr.GradeLevelId == subject.GradeLevelId)
                            .Take(2)
                            .ToListAsync(cancellationToken);

                        var teacherSubject = await context.TeacherSubjects
                            .FirstOrDefaultAsync(ts => ts.SubjectId == subject.Id, cancellationToken);

                        if (teacherSubject == null) continue;

                        foreach (var classRoom in classRooms)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var examDate = DateTime.Now.AddDays(i * 14 + subject.Id % 30);
                                exams.Add(new Exam
                                {
                                    AcademicYearId = academicYears.First().Id,
                                    SubjectId = subject.Id,
                                    ClassRoomId = classRoom.Id,
                                    ExamName = $"امتحان {subject.SubjectName} - {examTypes[i % examTypes.Length]}",
                                    ExamType = examTypes[i % examTypes.Length],
                                    ExamDate = examDate,
                                    StartTime = new TimeSpan(10, 0, 0),
                                    EndTime = new TimeSpan(12, 0, 0),
                                    MaxScore = 100,
                                    TeacherId = teacherSubject.TeacherId,
                                    IsActive = true,
                                    CreatedAt = DateTime.Now
                                });
                            }
                        }
                    }

                    foreach (var chunk in exams.Chunk(100))
                    {
                        await context.Exams.AddRangeAsync(chunk, cancellationToken);
                        await context.SaveChangesAsync(cancellationToken);
                    }
                }

                // ════════════════════════════════════════════════════════════════════
                // 15. نتائج الامتحانات (ExamResults)
                // ════════════════════════════════════════════════════════════════════
                if (!await context.ExamResults.AnyAsync(cancellationToken))
                {
                    // 🔥 تأكد من وجود امتحانات وطلاب
                    var exams = await context.Exams
                        .Include(e => e.ClassRoom)
                        .ThenInclude(cr => cr.GradeLevel)
                        .Take(300)
                        .ToListAsync(cancellationToken);

                    if (!exams.Any())
                    {
                        _logger?.LogWarning("لا توجد امتحانات لإنشاء نتائج لها");
                        return;
                    }

                    var examResults = new List<ExamResult>();

                    foreach (var exam in exams)
                    {
                        // ✅ التحقق من وجود ClassRoomId
                        if (exam.ClassRoomId == null)
                        {
                            _logger?.LogWarning($"الامتحان {exam.Id} ليس له ClassRoomId");
                            continue;
                        }

                        // ✅ جلب الطلاب من نفس الفصل
                        var students = await context.Students
                            .Where(s => s.ClassRoomId == exam.ClassRoomId.Value
                                        && s.IsActive
                                        && !s.IsGraduated)
                            .Take(20)
                            .ToListAsync(cancellationToken);

                        // ✅ لو مفيش طلاب في الفصل، نجيب أي طلاب في المدرسة
                        if (!students.Any())
                        {
                            var schoolId = await context.ClassRooms
                                .Where(cr => cr.Id == exam.ClassRoomId.Value)
                                .Select(cr => cr.GradeLevel.SchoolId)
                                .FirstOrDefaultAsync(cancellationToken);

                            if (schoolId > 0)
                            {
                                students = await context.Students
                                    .Include(s => s.User)
                                    .Where(s => s.User.SchoolId == schoolId
                                                && s.IsActive
                                                && !s.IsGraduated)
                                    .Take(20)
                                    .ToListAsync(cancellationToken);
                            }
                        }

                        // ✅ لو مفيش طلاب، نتخطى الامتحان
                        if (!students.Any())
                        {
                            _logger?.LogWarning($"لا يوجد طلاب للامتحان {exam.Id}");
                            continue;
                        }

                        foreach (var student in students)
                        {
                            var score = _random.Next(30, 101);
                            var percentage = exam.MaxScore > 0 ? score / exam.MaxScore * 100 : 0;

                            examResults.Add(new ExamResult
                            {
                                ExamId = exam.Id,
                                StudentId = student.Id,
                                Score = score,
                                Grade = GetGrade(percentage),
                                Percentage = percentage,
                                Remarks = score >= 50 ? "ناجح" : "راسب",
                                CreatedAt = DateTime.Now,
                                IsActive = true
                            });
                        }
                    }

                    // ✅ إضافة النتائج على دفعات
                    if (examResults.Any())
                    {
                        foreach (var chunk in examResults.Chunk(100))
                        {
                            await context.ExamResults.AddRangeAsync(chunk, cancellationToken);
                            await context.SaveChangesAsync(cancellationToken);
                        }
                        _logger?.LogInformation($"تم إضافة {examResults.Count} نتيجة امتحان");
                    }
                    else
                    {
                        _logger?.LogWarning("لم يتم إضافة أي نتائج امتحانات");
                    }
                }
                // ════════════════════════════════════════════════════════════════════
                // 15.5. التأكد من وجود نتائج - بيانات اختبار إضافية
                // ════════════════════════════════════════════════════════════════════
                if (!await context.ExamResults.AnyAsync(cancellationToken))
                {
                    // جلب أول امتحان وطلابه
                    var firstExam = await context.Exams
                        .Include(e => e.ClassRoom)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (firstExam != null)
                    {
                        var students = await context.Students
                            .Where(s => s.ClassRoomId == firstExam.ClassRoomId)
                            .Take(10)
                            .ToListAsync(cancellationToken);

                        if (students.Any())
                        {
                            var testResults = new List<ExamResult>();
                            foreach (var student in students)
                            {
                                var score = _random.Next(40, 100);
                                testResults.Add(new ExamResult
                                {
                                    ExamId = firstExam.Id,
                                    StudentId = student.Id,
                                    Score = score,
                                    Grade = GetGrade(score),
                                    Percentage = score,
                                    Remarks = "بيانات اختبار",
                                    IsActive = true,
                                    CreatedAt = DateTime.Now
                                });
                            }

                            await context.ExamResults.AddRangeAsync(testResults, cancellationToken);
                            await context.SaveChangesAsync(cancellationToken);
                            _logger?.LogInformation($"تم إضافة {testResults.Count} نتيجة اختبارية");
                        }
                    }
                }

                // ════════════════════════════════════════════════════════════════
                // 16. حضور الطلاب (StudentAttendances)
                // ════════════════════════════════════════════════════════════════
                if (!await context.StudentAttendances.AnyAsync(cancellationToken))
                {
                    var students = await context.Students
                        .Take(500)
                        .ToListAsync(cancellationToken);

                    var studentAttendances = new List<StudentAttendance>();
                    var statuses = new[] { AttendanceStatus.Present, AttendanceStatus.Present, AttendanceStatus.Present, AttendanceStatus.Absent, AttendanceStatus.Late };

                    foreach (var student in students)
                    {
                        for (int day = 1; day <= 20; day++)
                        {
                            var date = DateTime.Now.AddDays(-day);
                            var status = statuses[_random.Next(statuses.Length)];

                            studentAttendances.Add(new StudentAttendance
                            {
                                StudentId = student.Id,
                                AttendanceDate = date,
                                CheckInTime = status == AttendanceStatus.Present ? date.Date.AddHours(8 + _random.Next(0, 15)) : null,
                                CheckOutTime = status == AttendanceStatus.Present ? date.Date.AddHours(14 + _random.Next(0, 30)) : null,
                                Status = status,
                                DelayMinutes = status == AttendanceStatus.Late ? _random.Next(5, 30) : null,
                                Notes = status == AttendanceStatus.Absent ? "غياب بدون عذر" : null,
                                IsActive = true,
                                CreatedAt = DateTime.Now
                            });
                        }
                    }

                    await context.StudentAttendances.AddRangeAsync(studentAttendances, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                // ════════════════════════════════════════════════════════════════
                // 17. حضور الموظفين (EmployeeAttendances)
                // ════════════════════════════════════════════════════════════════
                if (!await context.EmployeeAttendances.AnyAsync(cancellationToken))
                {
                    var employees = await context.Employees
                        .Take(200)
                        .ToListAsync(cancellationToken);

                    var employeeAttendances = new List<EmployeeAttendance>();
                    var statuses = new[] { AttendanceStatus.Present, AttendanceStatus.Present, AttendanceStatus.Present, AttendanceStatus.Absent, AttendanceStatus.Late };

                    foreach (var employee in employees)
                    {
                        for (int day = 1; day <= 20; day++)
                        {
                            var date = DateTime.Now.AddDays(-day);
                            var status = statuses[_random.Next(statuses.Length)];

                            employeeAttendances.Add(new EmployeeAttendance
                            {
                                EmployeeId = employee.Id,
                                AttendanceDate = date,
                                CheckInTime = status == AttendanceStatus.Present ? date.Date.AddHours(8 + _random.Next(0, 15)) : null,
                                CheckOutTime = status == AttendanceStatus.Present ? date.Date.AddHours(16 + _random.Next(0, 30)) : null,
                                Status = status,
                                DelayMinutes = status == AttendanceStatus.Late ? _random.Next(5, 30) : null,
                                Notes = status == AttendanceStatus.Absent ? "غياب بدون عذر" : null,
                                IsActive = true,
                                CreatedAt = DateTime.Now
                            });
                        }
                    }

                    await context.EmployeeAttendances.AddRangeAsync(employeeAttendances, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                // ════════════════════════════════════════════════════════════════
                // 18. جهات الاتصال (UserContacts)
                // ════════════════════════════════════════════════════════════════
                if (!await context.UserContacts.AnyAsync(cancellationToken))
                {
                    var users = await context.Users
                        .Take(500)
                        .ToListAsync(cancellationToken);

                    var userContacts = new List<UserContact>();
                    var contactTypes = new[] { ContactType.Phone, ContactType.Mobile, ContactType.Email, ContactType.WhatsApp };

                    foreach (var user in users)
                    {
                        for (int i = 0; i < _random.Next(1, 4); i++)
                        {
                            var type = contactTypes[_random.Next(contactTypes.Length)];
                            var value = type switch
                            {
                                ContactType.Phone => $"02{_random.Next(10000000, 99999999)}",
                                ContactType.Mobile => $"01{_random.Next(0, 5)}{_random.Next(10000000, 99999999)}",
                                ContactType.Email => $"{user.Username}{i}@example.com",
                                ContactType.WhatsApp => $"01{_random.Next(0, 5)}{_random.Next(10000000, 99999999)}",
                                _ => string.Empty
                            };

                            userContacts.Add(new UserContact
                            {
                                UserId = user.Id,
                                ContactType = type,
                                ContactValue = value,
                                IsPrimary = i == 0,
                                IsVerified = i % 2 == 0,
                                IsActive = true,
                                CreatedAt = DateTime.Now
                            });
                        }
                    }

                    await context.UserContacts.AddRangeAsync(userContacts, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                // ════════════════════════════════════════════════════════════════
                // 19. أدوار المستخدمين (UserRoles)
                // ════════════════════════════════════════════════════════════════
                if (!await context.UserRoles.AnyAsync(cancellationToken))
                {
                    var users = await context.Users
                        .Where(u => u.UserType != UserType.Admin)
                        .ToListAsync(cancellationToken);

                    var userRoles = new List<UserRole>();

                    foreach (var user in users)
                    {
                        userRoles.Add(new UserRole
                        {
                            UserId = user.Id,
                            RoleType = user.UserType,
                            IsPrimary = true,
                            StartDate = DateTime.Now.AddMonths(-user.Id % 12),
                            IsActive = true,
                            CreatedAt = DateTime.Now
                        });
                    }

                    await context.UserRoles.AddRangeAsync(userRoles, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                // ✅ تأكيد العملية
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        private static string GetSchoolStage(int index)
        {
            var stages = new[] { "الابتدائية", "الإعدادية", "الثانوية", "الابتدائية", "الإعدادية", "الثانوية" };
            return stages[index % stages.Length];
        }

        private static string GetRandomPrincipalName(int index)
        {
            var names = new[] { "أحمد حسن", "محمد علي", "خالد سعيد", "ياسر محمود", "طارق عبدالله", "سامي إبراهيم", "حسام محمد", "عمرو خالد" };
            return names[index % names.Length];
        }

        private static string GetRandomFirstName(int index)
        {
            var names = new[] { "أحمد", "محمد", "خالد", "ياسر", "طارق", "سامي", "حسام", "عمرو", "محمود", "علي", "حسن", "سعيد", "عادل", "نادر" };
            return names[index % names.Length];
        }

        private static string GetRandomLastName(int index)
        {
            var names = new[] { "حسن", "محمد", "علي", "سعيد", "محمود", "إبراهيم", "عبدالله", "خالد", "عامر", "ناصر", "راشد", "سالم" };
            return names[index % names.Length];
        }

        private static string GetRandomNationalId(int index)
        {
            return $"2{_random.Next(0, 9)}{_random.Next(0, 9)}{_random.Next(0, 9)}{_random.Next(0, 9)}{_random.Next(0, 9)}{_random.Next(0, 9)}{_random.Next(0, 9)}{_random.Next(0, 9)}{_random.Next(0, 9)}{_random.Next(0, 9)}{_random.Next(0, 9)}{_random.Next(0, 9)}";
        }

        private static DateTime GetRandomDateOfBirth(int index)
        {
            var year = 1990 + (index % 15);
            var month = 1 + (index % 12);
            var day = 1 + (index % 28);
            return new DateTime(year, month, day);
        }

        private static string GetRandomQualification(int index)
        {
            var qualifications = new[]
            {
                "ليسانس آداب", "ليسانس تربية", "بكالوريوس علوم", "بكالوريوس تجارة", "بكالوريوس هندسة",
                "ليسانس حقوق", "بكالوريوس طب", "بكالوريوس صيدلة", "بكالوريوس زراعة", "بكالوريوس فنون"
            };
            return qualifications[index % qualifications.Length];
        }

        private static string GetRandomParentName(int index)
        {
            var names = new[] { "أحمد", "محمد", "خالد", "ياسر", "طارق", "سامي", "حسام", "عمرو", "محمود", "علي" };
            return $"{names[index % names.Length]} {names[(index + 1) % names.Length]}";
        }

        private static string GetGrade(int score)
        {
            return score switch
            {
                >= 90 => "A",
                >= 80 => "B",
                >= 70 => "C",
                >= 60 => "D",
                >= 50 => "E",
                _ => "F"
            };
        }

        #endregion
    }
}