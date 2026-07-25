using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Subjects
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📖  نموذج بيانات المادة الدراسية (Subject DTO)
    /// 📌  الوظيفة: نقل بيانات المادة من الخادم إلى العميل
    /// 📦  الاستخدام: في SubjectsController (GET endpoints)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class SubjectDto
    {
        /// <summary>
        /// معرف المادة (Primary Key)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المادة")]
        public int Id { get; set; }

        /// <summary>
        /// اسم المادة
        /// </summary>
        /// <example>اللغة العربية</example>
        [DisplayName("اسم المادة")]
        public string SubjectName { get; set; } = string.Empty;

        /// <summary>
        /// كود المادة (فريد)
        /// </summary>
        /// <example>SUB-AR-001</example>
        [DisplayName("كود المادة")]
        public string? SubjectCode { get; set; }

        /// <summary>
        /// عدد الحصص الأسبوعية
        /// </summary>
        /// <example>4</example>
        [DisplayName("عدد الحصص الأسبوعية")]
        public int? WeeklyHours { get; set; }

        /// <summary>
        /// هل المادة إجبارية؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مادة إجبارية")]
        public bool IsRequired { get; set; }

        /// <summary>
        /// وصف المادة
        /// </summary>
        /// <example>مادة اللغة العربية - النحو والصرف</example>
        [DisplayName("الوصف")]
        public string? Description { get; set; }

        /// <summary>
        /// معرف الصف الدراسي
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الصف")]
        public int GradeLevelId { get; set; }

        /// <summary>
        /// اسم الصف الدراسي
        /// </summary>
        /// <example>الصف الأول الثانوي</example>
        [DisplayName("الصف الدراسي")]
        public string? GradeLevelName { get; set; }

        /// <summary>
        /// اسم المدرسة
        /// </summary>
        /// <example>مدرسة النصر الثانوية</example>
        [DisplayName("المدرسة")]
        public string? SchoolName { get; set; }

        /// <summary>
        /// عدد المعلمين الذين يدرسون هذه المادة
        /// </summary>
        /// <example>3</example>
        [DisplayName("عدد المعلمين")]
        public int TeachersCount { get; set; }

        /// <summary>
        /// عدد الطلاب الذين يدرسون هذه المادة
        /// </summary>
        /// <example>150</example>
        [DisplayName("عدد الطلاب")]
        public int StudentsCount { get; set; }

        /// <summary>
        /// هل المادة مفعلة؟
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