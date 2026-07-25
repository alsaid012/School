using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Employees
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة للموظفين (Employee Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات الموظفين للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي عمليات الربط مع جداول أخرى
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class EmployeeLookupDto
    {
        /// <summary>
        /// معرف الموظف
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الموظف")]
        public int Id { get; set; }

        /// <summary>
        /// كود الموظف
        /// </summary>
        /// <example>EMP-2024-001</example>
        [DisplayName("كود الموظف")]
        public string EmployeeCode { get; set; } = string.Empty;

        /// <summary>
        /// اسم الموظف (المعروض للمستخدم)
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("اسم الموظف")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// المسمى الوظيفي
        /// </summary>
        /// <example>مدير شؤون الطلاب</example>
        [DisplayName("المسمى الوظيفي")]
        public string JobTitle { get; set; } = string.Empty;

        /// <summary>
        /// القسم
        /// </summary>
        /// <example>شؤون الطلاب</example>
        [DisplayName("القسم")]
        public string? Department { get; set; }

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
    }
}