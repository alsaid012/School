using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.ClassRooms
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة للفصول الدراسية (ClassRoom Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات الفصول للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي عمليات الربط مع جداول أخرى
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ClassRoomLookupDto
    {
        /// <summary>
        /// معرف الفصل
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الفصل")]
        public int Id { get; set; }

        /// <summary>
        /// اسم الفصل (المعروض للمستخدم)
        /// </summary>
        /// <example>1/أ</example>
        [DisplayName("اسم الفصل")]
        public string ClassName { get; set; } = string.Empty;

        /// <summary>
        /// كود الفصل
        /// </summary>
        /// <example>CLS-001</example>
        [DisplayName("كود الفصل")]
        public string? ClassCode { get; set; }

        /// <summary>
        /// اسم الصف الدراسي
        /// </summary>
        /// <example>الصف الأول الثانوي</example>
        [DisplayName("الصف الدراسي")]
        public string? GradeLevelName { get; set; }

        /// <summary>
        /// اسم معلم الفصل
        /// </summary>
        /// <example>أحمد حسن</example>
        [DisplayName("معلم الفصل")]
        public string? TeacherName { get; set; }

        /// <summary>
        /// السعة القصوى
        /// </summary>
        /// <example>30</example>
        [DisplayName("السعة")]
        public int Capacity { get; set; }

        /// <summary>
        /// عدد الطلاب في الفصل
        /// </summary>
        /// <example>25</example>
        [DisplayName("عدد الطلاب")]
        public int StudentsCount { get; set; }

        /// <summary>
        /// هل الفصل مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}