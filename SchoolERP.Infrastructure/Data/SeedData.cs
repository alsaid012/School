using Microsoft.EntityFrameworkCore;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Infrastructure.Data
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🌱  بيانات افتراضية للتشغيل (Seed Data)
    /// 📌  الوظيفة: إضافة بيانات أولية للتشغيل (Admin, محافظات, إدارات, مدارس)
    /// 🔄  يتم استدعاؤها عند إنشاء قاعدة البيانات أو تحديثها
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// 🌱 تهيئة البيانات الافتراضية
        /// </summary>
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            //// ════════════════════════════════════════════════════════════════
            //// 1. المحافظات (Governorates)
            //// ════════════════════════════════════════════════════════════════
            //if (!context.Governorates.Any())
            //{
            //    var governorates = new List<Governorate>
            //    {
            //         new Governorate { Name = "القاهرة", Code = "CAI" },
            //        new Governorate { Name = "الجيزة", Code = "GIZ" },
            //        new Governorate { Name = "الإسكندرية", Code = "ALX" },
            //        new Governorate { Name = "الدقهلية", Code = "DK" },
            //        new Governorate { Name = "الشرقية", Code = "SH" },
            //        new Governorate { Name = "الغربية", Code = "GH" },
            //        new Governorate { Name = "المنوفية", Code = "MN" },
            //        new Governorate { Name = "القليوبية", Code = "QL" },
            //        new Governorate { Name = "سوهاج", Code = "SHG" },
            //        new Governorate { Name = "أسيوط", Code = "AST" },
            //        new Governorate { Name = "الأقصر", Code = "LXR" },
            //        new Governorate { Name = "أسوان", Code = "ASW" },
            //        new Governorate { Name = "البحيرة", Code = "BHR" },
            //        new Governorate { Name = "بورسعيد", Code = "PSC" },
            //        new Governorate { Name = "دمياط", Code = "DMT" },
            //        new Governorate { Name = "الإسماعيلية", Code = "ISM" },
            //        new Governorate { Name = "السويس", Code = "SUZ" },
            //        new Governorate { Name = "كفر الشيخ", Code = "KFS" },
            //        new Governorate { Name = "المنيا", Code = "MNY" },
            //        new Governorate { Name = "بني سويف", Code = "BNS" },
            //        new Governorate { Name = "الفيوم", Code = "FYM" },
            //        new Governorate { Name = "قنا", Code = "QNA" },
            //        new Governorate { Name = "الأقصر", Code = "LXR2" },
            //        new Governorate { Name = "مرسى مطروح", Code = "MTH" },
            //        new Governorate { Name = "شمال سيناء", Code = "NSI" },
            //        new Governorate { Name = "جنوب سيناء", Code = "SSI" },
            //        new Governorate { Name = "الوادي الجديد", Code = "WJD" }
            //    };

            //    await context.Governorates.AddRangeAsync(governorates);
            //    await context.SaveChangesAsync();
            //}

            //// ════════════════════════════════════════════════════════════════
            //// 2. الإدارات التعليمية (Departments)
            //// ════════════════════════════════════════════════════════════════
            //if (!context.Departments.Any())
            //{
            //    var cairoId = context.Governorates.First(g => g.Code == "CAI").Id;
            //    var gizaId = context.Governorates.First(g => g.Code == "GIZ").Id;
            //    var alexId = context.Governorates.First(g => g.Code == "ALX").Id;
            //    var dkId = context.Governorates.First(g => g.Code == "DK").Id;
            //    var shId = context.Governorates.First(g => g.Code == "SH").Id;

            //    var departments = new List<Department>
            //    {
            //        new Department { GovernorateId = cairoId, Name = "إدارة شمال القاهرة التعليمية", Code = "SH-NORTH-CAIRO", DirectorName = "أ. محمد أحمد", Phone = "0223456789", Email = "north.cairo@moedu.gov.eg", Address = "شمال القاهرة - مصر الجديدة" },
            //        new Department { GovernorateId = cairoId, Name = "إدارة جنوب القاهرة التعليمية", Code = "SH-SOUTH-CAIRO", DirectorName = "أ. خالد حسن", Phone = "0229876543", Email = "south.cairo@moedu.gov.eg", Address = "جنوب القاهرة - المعادي" },
            //        new Department { GovernorateId = cairoId, Name = "إدارة شرق القاهرة التعليمية", Code = "SH-EAST-CAIRO", DirectorName = "أ. سعيد محمود", Phone = "0223456790", Email = "east.cairo@moedu.gov.eg", Address = "شرق القاهرة - مدينة نصر" },
            //        new Department { GovernorateId = cairoId, Name = "إدارة غرب القاهرة التعليمية", Code = "SH-WEST-CAIRO", DirectorName = "أ. نادية سعيد", Phone = "0223456791", Email = "west.cairo@moedu.gov.eg", Address = "غرب القاهرة - الدقي" },
            //        new Department { GovernorateId = gizaId, Name = "إدارة الجيزة التعليمية", Code = "SH-GIZA", DirectorName = "أ. ياسر محمد", Phone = "0234567890", Email = "giza@moedu.gov.eg", Address = "الجيزة - الدقي" },
            //        new Department { GovernorateId = gizaId, Name = "إدارة جنوب الجيزة التعليمية", Code = "SH-SOUTH-GIZA", DirectorName = "أ. هاني سعيد", Phone = "0234567891", Email = "south.giza@moedu.gov.eg", Address = "جنوب الجيزة - البدرشين" },
            //        new Department { GovernorateId = alexId, Name = "إدارة الإسكندرية التعليمية", Code = "SH-ALEX", DirectorName = "أ. فاطمة علي", Phone = "0334567890", Email = "alex@moedu.gov.eg", Address = "الإسكندرية - سيدي جابر" },
            //        new Department { GovernorateId = dkId, Name = "إدارة الدقهلية التعليمية", Code = "SH-DK", DirectorName = "أ. محمود حسن", Phone = "0553456789", Email = "dk@moedu.gov.eg", Address = "الدقهلية - المنصورة" },
            //        new Department { GovernorateId = shId, Name = "إدارة الشرقية التعليمية", Code = "SH-SH", DirectorName = "أ. سمير أحمد", Phone = "0554456789", Email = "sh@moedu.gov.eg", Address = "الشرقية - الزقازيق" },
            //        new Department { GovernorateId = cairoId, Name = "إدارة الوايلي التعليمية", Code = "SH-WAILY", DirectorName = "أ. كريم محمد", Phone = "0223456792", Email = "waily@moedu.gov.eg", Address = "الوايلي - القاهرة" },
            //        new Department { GovernorateId = cairoId, Name = "إدارة الزيتون التعليمية", Code = "SH-ZEITOON", DirectorName = "أ. شيرين عادل", Phone = "0223456793", Email = "zeetoon@moedu.gov.eg", Address = "الزيتون - القاهرة" },
            //        new Department { GovernorateId = gizaId, Name = "إدارة أكتوبر التعليمية", Code = "SH-OCTOBER", DirectorName = "أ. عمرو خالد", Phone = "0234567892", Email = "october@moedu.gov.eg", Address = "مدينة 6 أكتوبر - الجيزة" },
            //        new Department { GovernorateId = gizaId, Name = "إدارة الشيخ زايد التعليمية", Code = "SH-SHEIKH-ZAYED", DirectorName = "أ. منى سعيد", Phone = "0234567893", Email = "sheikh.zayed@moedu.gov.eg", Address = "الشيخ زايد - الجيزة" },
            //        new Department { GovernorateId = cairoId, Name = "إدارة مصر الجديدة التعليمية", Code = "SH-HELIOPOLIS", DirectorName = "أ. طارق محمود", Phone = "0223456794", Email = "heliopolis@moedu.gov.eg", Address = "مصر الجديدة - القاهرة" },
            //        new Department { GovernorateId = cairoId, Name = "إدارة المعادي التعليمية", Code = "SH-MAADI", DirectorName = "أ. دينا محمد", Phone = "0223456795", Email = "maadi@moedu.gov.eg", Address = "المعادي - القاهرة" }
            //    };

            //    await context.Departments.AddRangeAsync(departments);
            //    await context.SaveChangesAsync();
            //}

            //// ════════════════════════════════════════════════════════════════
            //// 3. المدارس (Schools)
            //// ════════════════════════════════════════════════════════════════
            //if (!context.Schools.Any())
            //{
            //    var depts = context.Departments.ToList();
            //    var schools = new List<School>();

            //    var schoolNames = new[]
            //    {
            //        "النصر", "السلام", "الأمل", "النهضة", "التوفيق", "النجاح", "التميز", "الإخلاص",
            //        "الوفاء", "العمل", "الحرية", "المستقبل", "القمة", "الريادة", "العبور", "الواحة",
            //        "الزهور", "الأزهار", "النخبة", "الأوائل", "الرواد", "النجوم", "البناة", "الأمانة",
            //        "الصدق", "الكمال", "القدوة", "الطموح", "الارتقاء", "الإبداع", "الابتكار", "الاجتهاد"
            //    };

            //    var schoolTypes = new[] { SchoolType.Public, SchoolType.Private, SchoolType.Language, SchoolType.International };

            //    for (int i = 0; i < 40; i++)
            //    {
            //        var dept = depts[i % depts.Count];
            //        var nameIndex = i % schoolNames.Length;
            //        var typeIndex = i % schoolTypes.Length;

            //        schools.Add(new School
            //        {
            //            DepartmentId = dept.Id,
            //            SchoolName = $"مدرسة {schoolNames[nameIndex]} {GetSchoolStage(i)}",
            //            SchoolCode = $"SCH-{1000 + i}",
            //            SchoolType = schoolTypes[typeIndex],
            //            Address = $"{dept.Address} - المنطقة {i + 1}",
            //            Phone = $"0{new Random().Next(10, 99)}{new Random().Next(10000000, 99999999)}",
            //            Email = $"school{i + 1}@edu.eg",
            //            PrincipalName = $"أ. {GetRandomPrincipalName(i)}",
            //            EstablishedYear = 1990 + (i % 30),
            //            IsActive = true,
            //            CreatedAt = DateTime.Now
            //        });
            //    }

            //    await context.Schools.AddRangeAsync(schools);
            //    await context.SaveChangesAsync();
            //}

            // ════════════════════════════════════════════════════════════════
            // 4. المستخدمين (Users) - Admin
            // ════════════════════════════════════════════════════════════════
            if (!context.Users.Any(u => u.Username == "admin"))
            {
                var school = context.Schools.First();

                var adminUser = new User
                {
                    SchoolId = school.Id,
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    FullName = "مدير النظام",
                    NationalId = "00000000000000",
                    DateOfBirth = new DateTime(1980, 1, 1),
                    UserType = UserType.Admin,
                    Status = UserStatus.Active,
                    IsActive = true
                };

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();

                // ════════════════════════════════════════════════════════════════
                // 5. أدوار المستخدم (UserRoles) - Admin
                // ════════════════════════════════════════════════════════════════
                var userRole = new UserRole
                {
                    UserId = adminUser.Id,
                    RoleType = UserType.Admin,
                    IsPrimary = true,
                    StartDate = DateTime.Now
                };

                await context.UserRoles.AddAsync(userRole);
                await context.SaveChangesAsync();
            }

            //// ════════════════════════════════════════════════════════════════
            //// 6. الصفوف الدراسية (GradeLevels)
            //// ════════════════════════════════════════════════════════════════
            //if (!context.GradeLevels.Any())
            //{
            //    var school = context.Schools.First();

            //    var gradeLevels = new List<GradeLevel>
            //    {
            //        new GradeLevel
            //        {
            //            SchoolId = school.Id,
            //            GradeName = "الصف الأول الابتدائي",
            //            GradeNumber = 1,
            //            GradeStage = GradeStage.Primary
            //        },
            //        new GradeLevel
            //        {
            //            SchoolId = school.Id,
            //            GradeName = "الصف الثاني الابتدائي",
            //            GradeNumber = 2,
            //            GradeStage = GradeStage.Primary
            //        },
            //        new GradeLevel
            //        {
            //            SchoolId = school.Id,
            //            GradeName = "الصف الثالث الابتدائي",
            //            GradeNumber = 3,
            //            GradeStage = GradeStage.Primary
            //        },
            //        new GradeLevel
            //        {
            //            SchoolId = school.Id,
            //            GradeName = "الصف الأول الإعدادي",
            //            GradeNumber = 1,
            //            GradeStage = GradeStage.Preparatory
            //        },
            //        new GradeLevel
            //        {
            //            SchoolId = school.Id,
            //            GradeName = "الصف الثاني الإعدادي",
            //            GradeNumber = 2,
            //            GradeStage = GradeStage.Preparatory
            //        },
            //        new GradeLevel
            //        {
            //            SchoolId = school.Id,
            //            GradeName = "الصف الثالث الإعدادي",
            //            GradeNumber = 3,
            //            GradeStage = GradeStage.Preparatory
            //        },
            //        new GradeLevel
            //        {
            //            SchoolId = school.Id,
            //            GradeName = "الصف الأول الثانوي",
            //            GradeNumber = 1,
            //            GradeStage = GradeStage.Secondary
            //        },
            //        new GradeLevel
            //        {
            //            SchoolId = school.Id,
            //            GradeName = "الصف الثاني الثانوي",
            //            GradeNumber = 2,
            //            GradeStage = GradeStage.Secondary
            //        },
            //        new GradeLevel
            //        {
            //            SchoolId = school.Id,
            //            GradeName = "الصف الثالث الثانوي",
            //            GradeNumber = 3,
            //            GradeStage = GradeStage.Secondary
            //        }
            //    };

            //    await context.GradeLevels.AddRangeAsync(gradeLevels);
            //    await context.SaveChangesAsync();
            //}

        //    // ════════════════════════════════════════════════════════════════
        //    // 7. العام الدراسي (AcademicYear)
        //    // ════════════════════════════════════════════════════════════════
        //    if (!context.AcademicYears.Any())
        //    {
        //        var school = context.Schools.First();

        //        var currentYear = new AcademicYear
        //        {
        //            SchoolId = school.Id,
        //            YearName = "2024-2025",
        //            StartDate = new DateTime(2024, 9, 1),
        //            EndDate = new DateTime(2025, 6, 30),
        //            IsCurrent = true
        //        };

        //        await context.AcademicYears.AddAsync(currentYear);
        //        await context.SaveChangesAsync();
        //    }
        //}

        //#region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════
        //private static string GetSchoolStage(int index)
        //{
        //    var stages = new[] { "الابتدائية", "الإعدادية", "الثانوية", "الابتدائية", "الإعدادية", "الثانوية" };
        //    return stages[index % stages.Length];
        //}

        //private static string GetRandomPrincipalName(int index)
        //{
        //    var names = new[] { "أحمد حسن", "محمد علي", "خالد سعيد", "ياسر محمود", "طارق عبدالله", "سامي إبراهيم", "حسام محمد", "عمرو خالد" };
        //    return names[index % names.Length];
        //}

        //private static string GetRandomFirstName(int index)
        //{
        //    var names = new[] { "أحمد", "محمد", "خالد", "ياسر", "طارق", "سامي", "حسام", "عمرو", "محمود", "علي", "حسن", "سعيد", "عادل", "نادر" };
        //    return names[index % names.Length];
        //}

        //private static string GetRandomLastName(int index)
        //{
        //    var names = new[] { "حسن", "محمد", "علي", "سعيد", "محمود", "إبراهيم", "عبدالله", "خالد", "عامر", "ناصر", "راشد", "سالم" };
        //    return names[index % names.Length];
        //}

        //private static string GetRandomNationalId(int index)
        //{
        //    return $"2{new Random().Next(0, 9)}{new Random().Next(0, 9)}{new Random().Next(0, 9)}{new Random().Next(0, 9)}{new Random().Next(0, 9)}{new Random().Next(0, 9)}{new Random().Next(0, 9)}{new Random().Next(0, 9)}{new Random().Next(0, 9)}{new Random().Next(0, 9)}{new Random().Next(0, 9)}{new Random().Next(0, 9)}";
        //}

        //private static DateTime GetRandomDateOfBirth(int index)
        //{
        //    var year = 1990 + (index % 15);
        //    var month = 1 + (index % 12);
        //    var day = 1 + (index % 28);
        //    return new DateTime(year, month, day);
        //}

        //private static string GetRandomQualification(int index)
        //{
        //    var qualifications = new[]
        //    {
        //        "ليسانس آداب", "ليسانس تربية", "بكالوريوس علوم", "بكالوريوس تجارة", "بكالوريوس هندسة",
        //        "ليسانس حقوق", "بكالوريوس طب", "بكالوريوس صيدلة", "بكالوريوس زراعة", "بكالوريوس فنون"
        //    };
        //    return qualifications[index % qualifications.Length];
        //}

        //private static string GetRandomParentName(int index)
        //{
        //    var names = new[] { "أحمد", "محمد", "خالد", "ياسر", "طارق", "سامي", "حسام", "عمرو", "محمود", "علي" };
        //    return $"{names[index % names.Length]} {names[(index + 1) % names.Length]}";
        //}

        //private static string GetGrade(int score)
        //{
        //    return score switch
        //    {
        //        >= 90 => "A",
        //        >= 80 => "B",
        //        >= 70 => "C",
        //        >= 60 => "D",
        //        >= 50 => "E",
        //        _ => "F"
        //    };
        //}

        //#endregion
    
    
    }
}