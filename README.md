# 🏫 SchoolERP - نظام إدارة المدارس

[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-red.svg)](https://www.microsoft.com/en-us/sql-server/)

> نظام متكامل لإدارة المدارس مبني باستخدام **ASP.NET Core MVC** و **Clean Architecture**

---

## 📋 عن المشروع

**School** هو نظام إدارة مدارس متكامل يوفر حلولاً شاملة لإدارة:
- 👤 المستخدمين والأدوار
- 🎓 الطلاب والمعلمين والموظفين
- 🏫 المدارس والصفوف والفصول
- 📚 المواد الدراسية وجدول الحصص
- 📝 الامتحانات والنتائج
- ✅ حضور الطلاب والموظفين
- 📞 جهات الاتصال

---

## 🛠️ التقنيات المستخدمة

| التقنية | الإصدار |
|---------|---------|
| ASP.NET Core MVC | 10.0 |
| Entity Framework Core | 10.0 |
| SQL Server | 2022+ |
| Bootstrap | 5.3 |
| Font Awesome | 6.0 |
| DataTables | 1.13 |
| AutoMapper | 12.0 |

---

## 🏗️ البنية المعمارية (Clean Architecture)
SchoolERP/
├── SchoolERP.Domain/ # طبقة الكيانات (Entities)
├── SchoolERP.Application/ # طبقة التطبيق (DTOs, Services)
├── SchoolERP.Infrastructure/ # طبقة البنية التحتية (DbContext, Repositories)
└── SchoolERP.Web/ # طبقة العرض (Controllers, Views)



---



## 🚀  كيفية التشغيل ##
---

 






🔑 بيانات الدخول
الدور	اسم       |    المستخدم	 |      كلمة المرور
مدير النظام |         admin     | 	Admin@123
مدير المدرسة    	principal     	Principal@123
معلم	             teacher       	        Teacher@123
موظف             	 employee     	Employee@123
طالب               student       	Student@123

---

git clone https://github.com/alsaid012/School.git

