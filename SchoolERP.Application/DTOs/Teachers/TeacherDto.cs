using SchoolERP.Application.DTOs.ClassRooms;
using System.ComponentModel;

namespace SchoolERP.Application.DTOs.Teachers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 👨‍🏫  نموذج بيانات المعلم (Teacher DTO)
    /// 📌  الوظيفة: نقل بيانات المعلم من الخادم إلى العميل
    /// 📦  الاستخدام: في TeachersController (GET endpoints)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class TeacherDto
    {
        /// <summary>
        /// معرف المعلم (Primary Key)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المعلم")]
        public int Id { get; set; }

        /// <summary>
        /// معرف المستخدم المرتبط بالمعلم
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المستخدم")]
        public int UserId { get; set; }

        /// <summary>
        /// كود المعلم (فريد)
        /// </summary>
        /// <example>TCH-2024-001</example>
        [DisplayName("كود المعلم")]
        public string TeacherCode { get; set; } = string.Empty;

        /// <summary>
        /// اسم المعلم (من جدول المستخدمين)
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("اسم المعلم")]
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
        /// المؤهل الدراسي
        /// </summary>
        /// <example>ليسانس آداب</example>
        [DisplayName("المؤهل الدراسي")]
        public string? Qualification { get; set; }

        /// <summary>
        /// التخصص
        /// </summary>
        /// <example>اللغة العربية</example>
        [DisplayName("التخصص")]
        public string? Specialization { get; set; }

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
        /// هل هو معلم فصل (Homeroom Teacher)؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("معلم فصل")]
        public bool IsHomeroomTeacher { get; set; }

        /// <summary>
        /// اسم المدرسة
        /// </summary>
        /// <example>مدرسة النصر الثانوية</example>
        [DisplayName("المدرسة")]
        public string? SchoolName { get; set; }

        /// <summary>
        /// قائمة المواد التي يدرسها المعلم
        /// </summary>
        [DisplayName("المواد")]
        public List<SubjectTeacherDto> Subjects { get; set; } = new();

        /// <summary>
        /// قائمة الفصول التي يشرف عليها (إذا كان معلم فصل)
        /// </summary>
        [DisplayName("الفصول")]
        public List<ClassRoomDto> ClassRooms { get; set; } = new();

        /// <summary>
        /// هل المعلم مفعل؟
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