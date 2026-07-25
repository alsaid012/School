using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.Schools
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء مدرسة جديدة (Create School DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء المدرسة من العميل إلى الخادم
    /// 📦  الاستخدام: في SchoolsController (POST endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateSchoolDto
    {
        /// <summary>
        /// اسم المدرسة (مطلوب)
        /// </summary>
        /// <example>مدرسة النصر الثانوية</example>
        [Required(ErrorMessage = "اسم المدرسة مطلوب")]
        [MaxLength(200, ErrorMessage = "اسم المدرسة لا يتجاوز 200 حرف")]
        public string SchoolName { get; set; } = string.Empty;

        /// <summary>
        /// كود المدرسة (مطلوب وفريد)
        /// </summary>
        /// <example>SCH-001</example>
        [Required(ErrorMessage = "كود المدرسة مطلوب")]
        [MaxLength(20, ErrorMessage = "كود المدرسة لا يتجاوز 20 حرف")]
        public string SchoolCode { get; set; } = string.Empty;

        /// <summary>
        /// نوع المدرسة (مطلوب)
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "نوع المدرسة مطلوب")]
        public SchoolType SchoolType { get; set; }

        /// <summary>
        /// عنوان المدرسة
        /// </summary>
        /// <example>مصر الجديدة - القاهرة</example>
        [MaxLength(500, ErrorMessage = "العنوان لا يتجاوز 500 حرف")]
        public string? Address { get; set; }

        /// <summary>
        /// رقم هاتف المدرسة
        /// </summary>
        /// <example>0223456789</example>
        [Phone(ErrorMessage = "صيغة رقم الهاتف غير صحيحة")]
        public string? Phone { get; set; }

        /// <summary>
        /// البريد الإلكتروني للمدرسة
        /// </summary>
        /// <example>school@example.com</example>
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string? Email { get; set; }

        /// <summary>
        /// اسم مدير المدرسة
        /// </summary>
        /// <example>أ. حسين علي</example>
        [MaxLength(100, ErrorMessage = "اسم المدير لا يتجاوز 100 حرف")]
        public string? PrincipalName { get; set; }

        /// <summary>
        /// سنة تأسيس المدرسة
        /// </summary>
        /// <example>1990</example>
        [Range(1900, 2100, ErrorMessage = "سنة التأسيس غير صحيحة")]
        public int? EstablishedYear { get; set; }

        /// <summary>
        /// معرف الإدارة التعليمية (مطلوب)
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "معرف الإدارة مطلوب")]
        public int DepartmentId { get; set; }
    }
}