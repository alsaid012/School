using SchoolERP.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Exams
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة للامتحانات (Exam Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات الامتحانات للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي عمليات الربط مع جداول أخرى
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ExamLookupDto
    {
        /// <summary>
        /// معرف الامتحان
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الامتحان")]
        public int Id { get; set; }


        public int SubjectId { get; set; }

        public int ClassRoomId { get; set; }

        public ExamType ExamType { get; set; }

        /// <summary>
        /// اسم الامتحان (المعروض للمستخدم)
        /// </summary>
        /// <example>امتحان اللغة العربية الشهري</example>
        [DisplayName("اسم الامتحان")]
        public string ExamName { get; set; } = string.Empty;

        /// <summary>
        /// نوع الامتحان
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
        /// عدد الطلاب المتقدمين
        /// </summary>
        /// <example>25</example>
        [DisplayName("عدد الطلاب")]
        public int StudentsCount { get; set; }

        /// <summary>
        /// هل الامتحان مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}