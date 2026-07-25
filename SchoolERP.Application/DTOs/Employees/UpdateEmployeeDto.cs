using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.Employees
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات الموظف (Update Employee DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث الموظف من العميل إلى الخادم
    /// 📦  الاستخدام: في EmployeesController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateEmployeeDto
    {
        /// <summary>
        /// المسمى الوظيفي
        /// </summary>
        /// <example>مدير شؤون الطلاب</example>
        [DisplayName("المسمى الوظيفي")]
        [MaxLength(100, ErrorMessage = "المسمى الوظيفي لا يتجاوز 100 حرف")]
        public string? JobTitle { get; set; }

        /// <summary>
        /// القسم التابع له الموظف
        /// </summary>
        /// <example>شؤون الطلاب</example>
        [DisplayName("القسم")]
        [MaxLength(100, ErrorMessage = "القسم لا يتجاوز 100 حرف")]
        public string? Department { get; set; }

        /// <summary>
        /// الراتب
        /// </summary>
        /// <example>5000.00</example>
        [DisplayName("الراتب")]
        [Range(0, double.MaxValue, ErrorMessage = "الراتب يجب أن يكون قيمة موجبة")]
        public decimal? Salary { get; set; }

        /// <summary>
        /// هل الموظف مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool? IsActive { get; set; }
    }
}