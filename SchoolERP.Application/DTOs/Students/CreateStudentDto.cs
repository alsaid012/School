using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Students
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء طالب جديد (Create Student DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء الطالب من العميل إلى الخادم
    /// 📦  الاستخدام: في StudentsController (POST endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateStudentDto
    {
        /// <summary>
        /// معرف المستخدم المرتبط بالطالب (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المستخدم")]
        [Required(ErrorMessage = "معرف المستخدم مطلوب")]
        public int UserId { get; set; }

        /// <summary>
        /// كود الطالب (مطلوب وفريد)
        /// </summary>
        /// <example>STU-2024-001</example>
        [DisplayName("كود الطالب")]
        [Required(ErrorMessage = "كود الطالب مطلوب")]
        [MaxLength(20, ErrorMessage = "كود الطالب لا يتجاوز 20 حرف")]
        public string StudentCode { get; set; } = string.Empty;

        /// <summary>
        /// معرف العام الدراسي (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف العام الدراسي")]
        [Required(ErrorMessage = "معرف العام الدراسي مطلوب")]
        public int AcademicYearId { get; set; }

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
        public bool IsGraduated { get; set; } = false;
    }
}