using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Students
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات الطالب (Update Student DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث الطالب من العميل إلى الخادم
    /// 📦  الاستخدام: في StudentsController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateStudentDto
    {
        /// <summary>
        /// معرف الفصل الدراسي
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الفصل")]
        public int? ClassRoomId { get; set; }

        /// <summary>
        /// اسم ولي الأمر
        /// </summary>
        /// <example>محمد أحمد</example>
        [DisplayName("اسم ولي الأمر")]
        [MaxLength(100, ErrorMessage = "اسم ولي الأمر لا يتجاوز 100 حرف")]
        public string? ParentName { get; set; }

        /// <summary>
        /// تليفون ولي الأمر
        /// </summary>
        /// <example>01001234567</example>
        [DisplayName("تليفون ولي الأمر")]
        [Phone(ErrorMessage = "صيغة رقم الهاتف غير صحيحة")]
        [MaxLength(20, ErrorMessage = "رقم الهاتف لا يتجاوز 20 رقم")]
        public string? ParentPhone { get; set; }

        /// <summary>
        /// البريد الإلكتروني لولي الأمر
        /// </summary>
        /// <example>parent@example.com</example>
        [DisplayName("بريد ولي الأمر")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        [MaxLength(100, ErrorMessage = "البريد لا يتجاوز 100 حرف")]
        public string? ParentEmail { get; set; }

        /// <summary>
        /// هل الطالب متخرج؟
        /// </summary>
        /// <example>false</example>
        [DisplayName("متخرج")]
        public bool? IsGraduated { get; set; }

        /// <summary>
        /// هل الطالب مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool? IsActive { get; set; }
    }
}