using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.Employees
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء موظف جديد (Create Employee DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء الموظف من العميل إلى الخادم
    /// 📦  الاستخدام: في EmployeesController (POST endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateEmployeeDto
    {
        /// <summary>
        /// معرف المستخدم المرتبط بالموظف (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المستخدم")]
        [Required(ErrorMessage = "معرف المستخدم مطلوب")]
        public int UserId { get; set; }

        /// <summary>
        /// كود الموظف (مطلوب وفريد)
        /// </summary>
        /// <example>EMP-2024-001</example>
        [DisplayName("كود الموظف")]
        [Required(ErrorMessage = "كود الموظف مطلوب")]
        [MaxLength(20, ErrorMessage = "كود الموظف لا يتجاوز 20 حرف")]
        public string EmployeeCode { get; set; } = string.Empty;

        /// <summary>
        /// المسمى الوظيفي (مطلوب)
        /// </summary>
        /// <example>مدير شؤون الطلاب</example>
        [DisplayName("المسمى الوظيفي")]
        [Required(ErrorMessage = "المسمى الوظيفي مطلوب")]
        [MaxLength(100, ErrorMessage = "المسمى الوظيفي لا يتجاوز 100 حرف")]
        public string JobTitle { get; set; } = string.Empty;

        /// <summary>
        /// القسم التابع له الموظف
        /// </summary>
        /// <example>شؤون الطلاب</example>
        [DisplayName("القسم")]
        [MaxLength(100, ErrorMessage = "القسم لا يتجاوز 100 حرف")]
        public string? Department { get; set; }

        /// <summary>
        /// تاريخ التعيين (مطلوب)
        /// </summary>
        /// <example>2020-09-01</example>
        [DisplayName("تاريخ التعيين")]
        [Required(ErrorMessage = "تاريخ التعيين مطلوب")]
        public DateTime HireDate { get; set; }

        /// <summary>
        /// الراتب
        /// </summary>
        /// <example>5000.00</example>
        [DisplayName("الراتب")]
        [Range(0, double.MaxValue, ErrorMessage = "الراتب يجب أن يكون قيمة موجبة")]
        public decimal? Salary { get; set; }
    }
}