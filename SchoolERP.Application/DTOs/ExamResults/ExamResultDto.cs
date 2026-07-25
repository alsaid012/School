using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.ExamResults
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج بيانات نتيجة الامتحان (ExamResult DTO)
    /// 📌  الوظيفة: نقل بيانات نتيجة الامتحان من الخادم إلى العميل
    /// 📦  الاستخدام: في ExamResultsController (GET endpoints)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ExamResultDto
    {
        /// <summary>
        /// معرف النتيجة (Primary Key)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف النتيجة")]
        public int Id { get; set; }

        /// <summary>
        /// معرف الامتحان
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الامتحان")]
        public int ExamId { get; set; }

        /// <summary>
        /// اسم الامتحان
        /// </summary>
        /// <example>امتحان اللغة العربية الشهري</example>
        [DisplayName("اسم الامتحان")]
        public string? ExamName { get; set; }

        /// <summary>
        /// معرف الطالب
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الطالب")]
        public int StudentId { get; set; }

        /// <summary>
        /// كود الطالب
        /// </summary>
        /// <example>STU-2024-001</example>
        [DisplayName("كود الطالب")]
        public string? StudentCode { get; set; }

        /// <summary>
        /// اسم الطالب
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [DisplayName("اسم الطالب")]
        public string? StudentName { get; set; }

        /// <summary>
        /// درجة الطالب في الامتحان
        /// </summary>
        /// <example>85</example>
        [DisplayName("الدرجة")]
        public int Score { get; set; }

        /// <summary>
        /// الدرجة النهائية للامتحان
        /// </summary>
        /// <example>100</example>
        [DisplayName("الدرجة النهائية")]
        public int MaxScore { get; set; }

        /// <summary>
        /// النسبة المئوية
        /// </summary>
        /// <example>85.0</example>
        [DisplayName("النسبة المئوية")]
        public decimal? Percentage { get; set; }

        /// <summary>
        /// التقدير (A, B, C, D, F)
        /// </summary>
        /// <example>B</example>
        [DisplayName("التقدير")]
        public string? Grade { get; set; }

        /// <summary>
        /// هل الطالب ناجح؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("ناجح")]
        public bool IsPassed { get; set; }

        /// <summary>
        /// ملاحظات إضافية
        /// </summary>
        /// <example>اجاب جيداً</example>
        [DisplayName("ملاحظات")]
        public string? Remarks { get; set; }

        /// <summary>
        /// تاريخ الامتحان
        /// </summary>
        /// <example>2024-01-15</example>
        [DisplayName("تاريخ الامتحان")]
        public DateTime ExamDate { get; set; }

        /// <summary>
        /// اسم المادة
        /// </summary>
        /// <example>اللغة العربية</example>
        [DisplayName("المادة")]
        public string? SubjectName { get; set; }

        /// <summary>
        /// اسم الفصل
        /// </summary>
        /// <example>1/أ</example>
        [DisplayName("الفصل")]
        public string? ClassRoomName { get; set; }

        /// <summary>
        /// هل النتيجة مفعلة؟
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