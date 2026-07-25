using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.Schools
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات المدرسة (Update School DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث المدرسة من العميل إلى الخادم
    /// 📦  الاستخدام: في SchoolsController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateSchoolDto
    {
        /// <summary>
        /// اسم المدرسة
        /// </summary>
        /// <example>مدرسة النصر الثانوية</example>
        [MaxLength(200, ErrorMessage = "اسم المدرسة لا يتجاوز 200 حرف")]
        public string? SchoolName { get; set; }

        /// <summary>
        /// كود المدرسة
        /// </summary>
        /// <example>SCH-001</example>
        [MaxLength(20, ErrorMessage = "كود المدرسة لا يتجاوز 20 حرف")]
        public string? SchoolCode { get; set; }

        /// <summary>
        /// نوع المدرسة
        /// </summary>
        /// <example>1</example>
        public SchoolType? SchoolType { get; set; }

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
        /// معرف الإدارة التعليمية
        /// </summary>
        /// <example>1</example>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// هل المدرسة مفعلة؟
        /// </summary>
        /// <example>true</example>
        public bool? IsActive { get; set; }
    }
}