using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.Exams
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📝  نموذج بيانات الامتحان (Exam DTO)
    /// 📌  الوظيفة: نقل بيانات الامتحان من الخادم إلى العميل
    /// 📦  الاستخدام: في ExamsController (GET endpoints)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ExamDto
    {
        /// <summary>
        /// معرف الامتحان (Primary Key)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الامتحان")]
        public int Id { get; set; }

        /// <summary>
        /// اسم الامتحان
        /// </summary>
        /// <example>امتحان اللغة العربية الشهري</example>
        [DisplayName("اسم الامتحان")]
        public string ExamName { get; set; } = string.Empty;

        /// <summary>
        /// نوع الامتحان (شهري، نصفي، نهائي، اختبار قصير، تقييم)
        /// </summary>
        /// <example>Monthly</example>
        [DisplayName("نوع الامتحان")]
        public ExamType ExamType { get; set; }

        /// <summary>
        /// اسم نوع الامتحان (نص مترجم)
        /// </summary>
        /// <example>شهري</example>
        [DisplayName("نوع الامتحان")]
        public string ExamTypeName { get; set; } = string.Empty;

        /// <summary>
        /// تاريخ الامتحان
        /// </summary>
        /// <example>2024-01-15</example>
        [DisplayName("تاريخ الامتحان")]
        public DateTime ExamDate { get; set; }

        /// <summary>
        /// وقت البداية
        /// </summary>
        /// <example>10:00</example>
        [DisplayName("وقت البداية")]
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// وقت النهاية
        /// </summary>
        /// <example>12:00</example>
        [DisplayName("وقت النهاية")]
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// الدرجة النهائية للامتحان
        /// </summary>
        /// <example>100</example>
        [DisplayName("الدرجة النهائية")]
        public int MaxScore { get; set; }

        /// <summary>
        /// معرف العام الدراسي
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
        /// معرف المادة
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المادة")]
        public int SubjectId { get; set; }

        /// <summary>
        /// اسم المادة
        /// </summary>
        /// <example>اللغة العربية</example>
        [DisplayName("المادة")]
        public string? SubjectName { get; set; }

        /// <summary>
        /// معرف الفصل الدراسي
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الفصل")]
        public int? ClassRoomId { get; set; }

        /// <summary>
        /// اسم الفصل
        /// </summary>
        /// <example>1/أ</example>
        [DisplayName("الفصل")]
        public string? ClassRoomName { get; set; }

        /// <summary>
        /// معرف المعلم المشرف على الامتحان
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المعلم")]
        public int? TeacherId { get; set; }

        /// <summary>
        /// اسم المعلم المشرف
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("المعلم المشرف")]
        public string? TeacherName { get; set; }

        /// <summary>
        /// عدد الطلاب الذين تقدموا للامتحان
        /// </summary>
        /// <example>25</example>
        [DisplayName("عدد الطلاب")]
        public int StudentsCount { get; set; }

        /// <summary>
        /// متوسط درجات الطلاب
        /// </summary>
        /// <example>78.5</example>
        [DisplayName("متوسط الدرجات")]
        public decimal? AverageScore { get; set; }

        /// <summary>
        /// أعلى درجة
        /// </summary>
        /// <example>95</example>
        [DisplayName("أعلى درجة")]
        public int? MaxStudentScore { get; set; }

        /// <summary>
        /// أدنى درجة
        /// </summary>
        /// <example>60</example>
        [DisplayName("أدنى درجة")]
        public int? MinStudentScore { get; set; }

        /// <summary>
        /// نسبة النجاح
        /// </summary>
        /// <example>85.0</example>
        [DisplayName("نسبة النجاح")]
        public decimal? SuccessRate { get; set; }

        /// <summary>
        /// ملاحظات إضافية
        /// </summary>
        /// <example>امتحان شامل</example>
        [DisplayName("ملاحظات")]
        public string? Notes { get; set; }

        /// <summary>
        /// هل الامتحان مفعل؟
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