using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.Employees
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 👨‍💼  نموذج بيانات الموظف (Employee DTO)
    /// 📌  الوظيفة: نقل بيانات الموظف من الخادم إلى العميل
    /// 📦  الاستخدام: في EmployeesController (GET endpoints)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class EmployeeDto
    {
        /// <summary>
        /// معرف الموظف (Primary Key)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الموظف")]
        public int Id { get; set; }

        /// <summary>
        /// معرف المستخدم المرتبط بالموظف
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المستخدم")]
        public int UserId { get; set; }

        /// <summary>
        /// كود الموظف (فريد)
        /// </summary>
        /// <example>EMP-2024-001</example>
        [DisplayName("كود الموظف")]
        public string EmployeeCode { get; set; } = string.Empty;

        /// <summary>
        /// اسم الموظف (من جدول المستخدمين)
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("اسم الموظف")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// البريد الإلكتروني
        /// </summary>
        /// <example>ahmed@example.com</example>
        [DisplayName("البريد الإلكتروني")]
        public string? Email { get; set; }

        /// <summary>
        /// رقم الهاتف المحمول
        /// </summary>
        /// <example>01001234567</example>
        [DisplayName("رقم الهاتف")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// المسمى الوظيفي
        /// </summary>
        /// <example>مدير شؤون الطلاب</example>
        [DisplayName("المسمى الوظيفي")]
        public string JobTitle { get; set; } = string.Empty;

        /// <summary>
        /// القسم التابع له الموظف
        /// </summary>
        /// <example>شؤون الطلاب</example>
        [DisplayName("القسم")]
        public string? Department { get; set; }

        /// <summary>
        /// تاريخ التعيين
        /// </summary>
        /// <example>2020-09-01</example>
        [DisplayName("تاريخ التعيين")]
        public DateTime HireDate { get; set; }

        /// <summary>
        /// الراتب
        /// </summary>
        /// <example>5000.00</example>
        [DisplayName("الراتب")]
        public decimal? Salary { get; set; }

        /// <summary>
        /// اسم المدرسة
        /// </summary>
        /// <example>مدرسة النصر الثانوية</example>
        [DisplayName("المدرسة")]
        public string? SchoolName { get; set; }

        /// <summary>
        /// هل الموظف مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }

        /// <summary>
        /// تاريخ الإنشاء
        /// </summary>
        /// <example>2024-01-01T12:00:00</example>
        [DisplayName("تاريخ الإنشاء")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// تاريخ آخر تحديث
        /// </summary>
        /// <example>2024-01-15T10:30:00</example>
        [DisplayName("تاريخ التحديث")]
        public DateTime? UpdatedAt { get; set; }
    }
}