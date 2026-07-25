🏫 SchoolERP - نظام إدارة المدارس

نظام متكامل لإدارة المدارس مبني باستخدام ASP.NET Core MVC و Clean Architecture

📋 عن المشروع

SchoolERP هو نظام إدارة مدارس متكامل يوفر حلولاً شاملة لإدارة المؤسسات التعليمية.

المميزات الرئيسية:

👤 إدارة المستخدمين: المستخدمين، الأدوار، الصلاحيات، المصادقة
🎓 إدارة الطلاب: تسجيل الطلاب، الملفات الشخصية، الدرجات، الحضور
👨‍🏫 إدارة المعلمين: تسجيل المعلمين، توزيع المواد، الجداول
👨‍💼 إدارة الموظفين: تسجيل الموظفين، الحضور، المهام
🏫 إدارة المدارس: المدارس، المحافظات، الإدارات التعليمية
📚 المواد الدراسية: المواد، الصفوف، الفصول، ربط المعلمين
📅 جدول الحصص: إنشاء الجدول، إدارة الحصص، التعارضات
📝 الامتحانات: إنشاء الامتحانات، النتائج، الإحصائيات
✅ الحضور: حضور الطلاب، حضور الموظفين، التقارير

🛠️ التقنيات المستخدمة:

ASP.NET Core MVC 10.0
Entity Framework Core 10.0
SQL Server 2022+
Clean Architecture
Repository Pattern
Unit of Work
AutoMapper 12.0
Bootstrap 5.3
Font Awesome 6.0
DataTables 1.13
BCrypt

🏗️ البنية المعمارية (Clean Architecture):

SchoolERP.Domain - طبقة الكيانات (Entities)
SchoolERP.Application - طبقة التطبيق (DTOs, Services)
SchoolERP.Infrastructure - طبقة البنية التحتية (DbContext, Repositories)
SchoolERP.Web - طبقة العرض (Controllers, Views)

🚀 كيفية التشغيل:

1. استنساخ المشروع:
   git clone https://github.com/alsaid012/School.git
   cd School

2. تحديث Connection String:
   افتح ملف SchoolERP.Web/appsettings.json وعدل الاتصال بقاعدة البيانات.

3. تشغيل الـ Migrations:
   في Visual Studio (Package Manager Console):
   Select-Project SchoolERP.Infrastructure
   Update-Database

   في Command Line:
   dotnet ef database update --project SchoolERP.Infrastructure --startup-project SchoolERP.Web

4. تشغيل المشروع:
   في Visual Studio: اضغط F5 أو Ctrl + F5
   في Command Line: dotnet run --project SchoolERP.Web

5. فتح المتصفح:
   

🔑 بيانات الدخول:

مدير النظام - admin - Admin@123
مدير المدرسة - principal - Principal@123
معلم - teacher - Teacher@123
موظف - employee - Employee@123
طالب - student - Student@123

📁 هيكل قاعدة البيانات:

الجداول الرئيسية:
Users - المستخدمين
UserRoles - أدوار المستخدمين
Schools - المدارس
Students - الطلاب
Teachers - المعلمين
Employees - الموظفين
GradeLevels - الصفوف الدراسية
ClassRooms - الفصول الدراسية
Subjects - المواد الدراسية
TeacherSubjects - ربط المعلمين بالمواد
ClassSchedules - جدول الحصص
AcademicYears - السنوات الدراسية
Exams - الامتحانات
ExamResults - نتائج الامتحانات
StudentAttendances - حضور الطلاب
EmployeeAttendances - حضور الموظفين
UserContacts - جهات الاتصال

علاقات الجداول:
Governorate (1) -> Department (M) -> School (M)
School (1) -> User (M) -> UserRole (M)
User (1) -> Student (1) / Teacher (1) / Employee (1)
School (1) -> GradeLevel (M) -> ClassRoom (M)
ClassRoom (1) -> Student (M)
GradeLevel (1) -> Subject (M)
Teacher (M) -> TeacherSubject (M) -> Subject (M)
ClassRoom (1) -> ClassSchedule (M) -> Subject (1) -> Teacher (1)
Exam (1) -> ExamResult (M) -> Student (1)
Student (1) -> StudentAttendance (M)
Employee (1) -> EmployeeAttendance (M)
User (1) -> UserContact (M)

📋 المميزات التقنية:

✅ Clean Architecture - فصل واضح بين الطبقات
✅ Repository Pattern - فصل منطق الوصول إلى البيانات
✅ Unit of Work - إدارة المعاملات بشكل موحد
✅ AutoMapper - تحويل تلقائي بين الكيانات و DTOs
✅ Soft Delete - حذف منطقي مع إمكانية الاستعادة
✅ Audit Trail - تتبع من أنشأ وعدل البيانات
✅ Authentication - مصادقة باستخدام Cookies
✅ Authorization - صلاحيات على مستوى الأدوار
✅ Validation - التحقق من البيانات على العميل والخادم
✅ RTL Support - دعم كامل للغة العربية
✅ Responsive Design - تصميم متجاوب مع جميع الأجهزة
✅ DataTables - جداول تفاعلية مع بحث وترتيب

🤝 المساهمة:

نرحب بمساهماتكم! للمساهمة:
1. عمل Fork للمشروع
2. إنشاء فرع جديد
3. عمل Commit للتغييرات
4. Push إلى الفرع
5. فتح Pull Request

📜 الترخيص:
هذا المشروع مرخص تحت MIT License.

👤 المطور:
    - السيد عبدالرحمن  - المطور الرئيسي
GitHub: @alsaid012
البريد الإلكتروني: alsaid012@gmail.com

⭐ إذا أعجبك المشروع، لا تنسى وضع نجمة (Star) على GitHub!

Made with ❤️ by Ahmed Al-Said
