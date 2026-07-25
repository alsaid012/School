using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Students
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🧑‍🎓  نموذج بيانات الطالب الأساسية (Student DTO)
    /// 📌  الوظيفة: نقل بيانات الطالب من الخادم إلى العميل
    /// 📦  الاستخدام: في StudentsController (GET endpoints)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class StudentDto
    {
        /// <summary>
        /// معرف الطالب (Primary Key)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الطالب")]
        public int Id { get; set; }

        /// <summary>
        /// كود الطالب (فريد)
        /// </summary>
        /// <example>STU-2024-001</example>
        [DisplayName("كود الطالب")]
        public string StudentCode { get; set; } = string.Empty;

        /// <summary>
        /// معرف المستخدم المرتبط بالطالب
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المستخدم")]
        public int UserId { get; set; }

        /// <summary>
        /// اسم الطالب (من جدول المستخدمين)
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("اسم الطالب")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// اسم ولي الأمر
        /// </summary>
        /// <example>محمد أحمد</example>
        [DisplayName("اسم ولي الأمر")]
        public string? ParentName { get; set; }

        /// <summary>
        /// تليفون ولي الأمر
        /// </summary>
        /// <example>01001234567</example>
        [DisplayName("تليفون ولي الأمر")]
        public string? ParentPhone { get; set; }

        /// <summary>
        /// البريد الإلكتروني لولي الأمر
        /// </summary>
        /// <example>parent@example.com</example>
        [DisplayName("بريد ولي الأمر")]
        public string? ParentEmail { get; set; }

        /// <summary>
        /// تاريخ التقييد (الالتحاق بالمدرسة)
        /// </summary>
        /// <example>2024-09-01</example>
        [DisplayName("تاريخ التقييد")]
        public DateTime EnrollmentDate { get; set; }

        /// <summary>
        /// هل الطالب متخرج؟
        /// </summary>
        /// <example>false</example>
        [DisplayName("متخرج")]
        public bool IsGraduated { get; set; }

        /// <summary>
        /// معرف العام الدراسي الحالي
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف العام الدراسي")]
        public int AcademicYearId { get; set; }

        /// <summary>
        /// اسم العام الدراسي
        /// </summary>
        /// <example>2024-2025</example>
        [DisplayName("العام الدراسي")]
        public string? AcademicYearName { get; set; }

        /// <summary>
        /// معرف الفصل الدراسي
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الفصل")]
        public int? ClassRoomId { get; set; }

        /// <summary>
        /// اسم الفصل الدراسي
        /// </summary>
        /// <example>1/أ</example>
        [DisplayName("الفصل")]
        public string? ClassRoomName { get; set; }

        /// <summary>
        /// معرف الصف الدراسي
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الصف")]
        public int? GradeLevelId { get; set; }

        /// <summary>
        /// اسم الصف الدراسي
        /// </summary>
        /// <example>الصف الأول الثانوي</example>
        [DisplayName("الصف الدراسي")]
        public string? GradeLevelName { get; set; }

        /// <summary>
        /// هل الطالب مفعل؟
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