using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.ClassRooms
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء فصل دراسي جديد (Create ClassRoom DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء الفصل من العميل إلى الخادم
    /// 📦  الاستخدام: في ClassRoomsController (POST endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateClassRoomDto
    {
        /// <summary>
        /// اسم الفصل (مطلوب)
        /// </summary>
        /// <example>1/أ</example>
        [DisplayName("اسم الفصل")]
        [Required(ErrorMessage = "اسم الفصل مطلوب")]
        [MaxLength(50, ErrorMessage = "اسم الفصل لا يتجاوز 50 حرف")]
        public string ClassName { get; set; } = string.Empty;

        /// <summary>
        /// كود الفصل (فريد)
        /// </summary>
        /// <example>CLS-001</example>
        [DisplayName("كود الفصل")]
        [MaxLength(20, ErrorMessage = "كود الفصل لا يتجاوز 20 حرف")]
        public string? ClassCode { get; set; }

        /// <summary>
        /// رقم الغرفة
        /// </summary>
        /// <example>101</example>
        [DisplayName("رقم الغرفة")]
        [MaxLength(20, ErrorMessage = "رقم الغرفة لا يتجاوز 20 حرف")]
        public string? RoomNumber { get; set; }

        /// <summary>
        /// السعة القصوى للفصل (مطلوب)
        /// </summary>
        /// <example>30</example>
        [DisplayName("السعة")]
        [Required(ErrorMessage = "السعة مطلوبة")]
        [Range(1, 100, ErrorMessage = "السعة يجب أن تكون بين 1 و 100")]
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
        /// معرف الصف الدراسي (مطلوب)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الصف")]
        [Required(ErrorMessage = "معرف الصف مطلوب")]
        public int GradeLevelId { get; set; }

        /// <summary>
        /// معرف معلم الفصل (Homeroom Teacher)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف معلم الفصل")]
        public int? TeacherId { get; set; }

        /// <summary>
        /// ملاحظات إضافية
        /// </summary>
        /// <example>فصل مجهز بالكامل</example>
        [DisplayName("ملاحظات")]
        [MaxLength(500, ErrorMessage = "الملاحظات لا تتجاوز 500 حرف")]
        public string? Notes { get; set; }
    }
}