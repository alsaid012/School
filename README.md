# 🏫 SchoolERP - نظام إدارة المدارس

[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-red.svg)](https://www.microsoft.com/en-us/sql-server/)
[![GitHub last commit](https://img.shields.io/github/last-commit/alsaid012/School)](https://github.com/alsaid012/School)
[![GitHub repo size](https://img.shields.io/github/repo-size/alsaid012/School)](https://github.com/alsaid012/School)
[![GitHub stars](https://img.shields.io/github/stars/alsaid012/School)](https://github.com/alsaid012/School)

> نظام متكامل لإدارة المدارس مبني باستخدام **ASP.NET Core MVC** و **Clean Architecture**

---

## 📋 عن المشروع

**SchoolERP** هو نظام إدارة مدارس متكامل يوفر حلولاً شاملة لإدارة المؤسسات التعليمية.

### ✨ المميزات الرئيسية

| القسم | المميزات |
|-------|----------|
| 👤 **إدارة المستخدمين** | المستخدمين، الأدوار، الصلاحيات، المصادقة |
| 🎓 **إدارة الطلاب** | تسجيل الطلاب، الملفات الشخصية، الدرجات، الحضور |
| 👨‍🏫 **إدارة المعلمين** | تسجيل المعلمين، توزيع المواد، الجداول |
| 👨‍💼 **إدارة الموظفين** | تسجيل الموظفين، الحضور، المهام |
| 🏫 **إدارة المدارس** | المدارس، المحافظات، الإدارات التعليمية |
| 📚 **المواد الدراسية** | المواد، الصفوف، الفصول، ربط المعلمين |
| 📅 **جدول الحصص** | إنشاء الجدول، إدارة الحصص، التعارضات |
| 📝 **الامتحانات** | إنشاء الامتحانات، النتائج، الإحصائيات |
| ✅ **الحضور** | حضور الطلاب، حضور الموظفين، التقارير |

---

## 🛠️ التقنيات المستخدمة

| التقنية | الإصدار | الوصف |
|---------|---------|-------|
| **ASP.NET Core MVC** | 10.0 | إطار العمل الرئيسي |
| **Entity Framework Core** | 10.0 | ORM للتعامل مع قاعدة البيانات |
| **SQL Server** | 2022+ | قاعدة البيانات |
| **Clean Architecture** | - | بنية المشروع |
| **Repository Pattern** | - | نمط المستودعات |
| **Unit of Work** | - | نمط وحدة العمل |
| **AutoMapper** | 12.0 | تحويل الكيانات إلى DTOs |
| **Bootstrap 5** | 5.3 | إطار العمل للواجهات |
| **Font Awesome** | 6.0 | أيقونات |
| **DataTables** | 1.13 | جداول تفاعلية |
| **BCrypt** | - | تشفير كلمات المرور |

---

## 🏗️ البنية المعمارية (Clean Architecture)

---

SchoolERP/
├── SchoolERP.Domain/ # طبقة الكيانات (Entities)
│ ├── Entities/ # الكيانات الأساسية
│ ├── Enums/ # التعدادات (Enums)
│ └── Interfaces/ # الواجهات الأساسية
│
├── SchoolERP.Application/ # طبقة التطبيق (Application)
│ ├── DTOs/ # نماذج نقل البيانات
│ ├── Interfaces/ # واجهات الخدمات والمستودعات
│ ├── Services/ # خدمات التطبيق
│ ├── Validators/ # التحقق من البيانات
│ └── Mappings/ # AutoMapper Profiles
│
├── SchoolERP.Infrastructure/ # طبقة البنية التحتية
│ ├── Data/ # DbContext والتهيئة
│ ├── Repositories/ # تنفيذ المستودعات
│ ├── Migrations/ # هجرات قاعدة البيانات
│ └── Extensions/ # إضافات
│
└── SchoolERP.Web/ # طبقة العرض (Presentation)
├── Controllers/ # وحدات التحكم
├── Views/ # صفحات Razor
├── ViewModels/ # نماذج العرض
├── wwwroot/ # الملفات الثابتة
│ ├── css/ # ملفات التنسيق
│ └── js/ # ملفات السكريبت
└── Program.cs # نقطة بدء التطبيق
---




## 🚀 كيفية التشغيل

 1️⃣ استنساخ المشروع

```bash
git clone https://github.com/alsaid012/School.git
cd School



---
## ----------------------------------------------
## 🔑 بيانات الدخول
الدور	اسم المستخدم	كلمة المرور
👑 مدير النظام	admin	Admin@123
🏫 مدير المدرسة	principal	Principal@123
👨‍🏫 معلم	teacher	Teacher@123
👨‍💼 موظف	employee	Employee@123
🎓 طالب	student	Student@123
---






<img width="1899" height="1096" alt="image" src="https://github.com/user-attachments/assets/5997ffc0-1910-41d4-bb9d-1980142eb8f8" />
<img width="1910" height="1028" alt="image" src="https://github.com/user-attachments/assets/74928647-4ec0-4a62-8d05-aab28aca254b" />

