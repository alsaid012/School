using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.ClassRooms
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات الفصل الدراسي (Update ClassRoom DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث الفصل من العميل إلى الخادم
    /// 📦  الاستخدام: في ClassRoomsController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateClassRoomDto
    {
        /// <summary>
        /// اسم الفصل
        /// </summary>
        /// <example>1/أ</example>
        [DisplayName("اسم الفصل")]
        [MaxLength(50, ErrorMessage = "اسم الفصل لا يتجاوز 50 حرف")]
        public string? ClassName { get; set; }

        /// <summary>
        /// كود الفصل
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
        /// السعة القصوى للفصل
        /// </summary>
        /// <example>30</example>
        [DisplayName("السعة")]
        [Range(1, 100, ErrorMessage = "السعة يجب أن تكون بين 1 و 100")]
        public int? Capacity { get; set; }

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
        public int? GradeLevelId { get; set; }

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

        /// <summary>
        /// هل الفصل مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool? IsActive { get; set; }
    }
}