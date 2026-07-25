using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.ClassRooms
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏫  نموذج بيانات الفصل الدراسي (ClassRoom DTO)
    /// 📌  الوظيفة: نقل بيانات الفصل من الخادم إلى العميل
    /// 📦  الاستخدام: في ClassRoomsController (GET endpoints)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ClassRoomDto
    {
        /// <summary>
        /// معرف الفصل (Primary Key)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الفصل")]
        public int Id { get; set; }

        /// <summary>
        /// اسم الفصل
        /// </summary>
        /// <example>1/أ</example>
        [DisplayName("اسم الفصل")]
        public string ClassName { get; set; } = string.Empty;

        /// <summary>
        /// كود الفصل (فريد)
        /// </summary>
        /// <example>CLS-001</example>
        [DisplayName("كود الفصل")]
        public string? ClassCode { get; set; }

        /// <summary>
        /// رقم الغرفة
        /// </summary>
        /// <example>101</example>
        [DisplayName("رقم الغرفة")]
        public string? RoomNumber { get; set; }

        /// <summary>
        /// السعة القصوى للفصل
        /// </summary>
        /// <example>30</example>
        [DisplayName("السعة")]
        public int Capacity { get; set; }

        /// <summary>
        /// يوجد سبورة ذكية؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("سبورة ذكية")]
        public bool HasSmartBoard { get; set; }

        /// <summary>
        /// يوجد بروجيكتور؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("بروجيكتور")]
        public bool HasProjector { get; set; }

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
        /// معرف معلم الفصل (Homeroom Teacher)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف معلم الفصل")]
        public int? TeacherId { get; set; }

        /// <summary>
        /// اسم معلم الفصل
        /// </summary>
        /// <example>أحمد حسن</example>
        [DisplayName("معلم الفصل")]
        public string? TeacherName { get; set; }

        /// <summary>
        /// عدد الطلاب في الفصل
        /// </summary>
        /// <example>25</example>
        [DisplayName("عدد الطلاب")]
        public int StudentsCount { get; set; }

        /// <summary>
        /// ملاحظات إضافية
        /// </summary>
        /// <example>فصل مجهز بالكامل</example>
        [DisplayName("ملاحظات")]
        public string? Notes { get; set; }

        /// <summary>
        /// هل الفصل مفعل؟
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