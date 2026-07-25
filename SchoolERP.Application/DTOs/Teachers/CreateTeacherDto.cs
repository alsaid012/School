using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.Teachers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء معلم جديد (Create Teacher DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء المعلم من العميل إلى الخادم
    /// 📦  الاستخدام: في TeachersController (POST endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateTeacherDto
    {
        /// <summary>
        /// معرف المستخدم المرتبط بالمعلم (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المستخدم")]
        [Required(ErrorMessage = "معرف المستخدم مطلوب")]
        public int UserId { get; set; }

        /// <summary>
        /// كود المعلم (مطلوب وفريد)
        /// </summary>
        /// <example>TCH-2024-001</example>
        [DisplayName("كود المعلم")]
        [Required(ErrorMessage = "كود المعلم مطلوب")]
        [MaxLength(20, ErrorMessage = "كود المعلم لا يتجاوز 20 حرف")]
        public string TeacherCode { get; set; } = string.Empty;

        /// <summary>
        /// المؤهل الدراسي
        /// </summary>
        /// <example>ليسانس آداب</example>
        [DisplayName("المؤهل الدراسي")]
        [MaxLength(200, ErrorMessage = "المؤهل لا يتجاوز 200 حرف")]
        public string? Qualification { get; set; }

        /// <summary>
        /// التخصص
        /// </summary>
        /// <example>اللغة العربية</example>
        [DisplayName("التخصص")]
        [MaxLength(200, ErrorMessage = "التخصص لا يتجاوز 200 حرف")]
        public string? Specialization { get; set; }

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

        /// <summary>
        /// هل هو معلم فصل (Homeroom Teacher)؟
        /// </summary>
        /// <example>false</example>
        [DisplayName("معلم فصل")]
        public bool IsHomeroomTeacher { get; set; }

        /// <summary>
        /// قائمة معرفات المواد التي يدرسها المعلم
        /// </summary>
        /// <example>[1, 2, 3]</example>
        [DisplayName("المواد")]
        public List<int> SubjectIds { get; set; } = new();
    }
}