using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.GradeLevels
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📋  نموذج القائمة المنسدلة للصفوف الدراسية (GradeLevel Lookup DTO)
    /// 📌  الوظيفة: نقل بيانات الصفوف للقوائم المنسدلة (Dropdown/ComboBox)
    /// 📦  الاستخدام: في الـ UI (Select Lists) وفي عمليات الربط مع جداول أخرى
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class GradeLevelLookupDto
    {
        /// <summary>
        /// معرف الصف
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الصف")]
        public int Id { get; set; }

        /// <summary>
        /// اسم الصف (المعروض للمستخدم)
        /// </summary>
        /// <example>الصف الأول الثانوي</example>
        [DisplayName("اسم الصف")]
        public string GradeName { get; set; } = string.Empty;

        /// <summary>
        /// رقم الصف
        /// </summary>
        /// <example>1</example>
        [DisplayName("رقم الصف")]
        public int GradeNumber { get; set; }


        public GradeStage GradeStage { get; set; }


        /// <summary>
        /// المرحلة الدراسية
        /// </summary>
        /// <example>ثانوي</example>
        [DisplayName("المرحلة")]
        public string GradeStageName  { get; set; } = string.Empty;


        public int SchoolId { get; set; }


        /// <summary>
        /// اسم المدرسة
        /// </summary>
        /// <example>مدرسة النصر الثانوية</example>
        [DisplayName("المدرسة")]
        public string? SchoolName { get; set; }

        /// <summary>
        /// عدد الفصول في هذا الصف
        /// </summary>
        /// <example>5</example>
        [DisplayName("عدد الفصول")]
        public int ClassRoomsCount { get; set; }

        /// <summary>
        /// هل الصف مفعل؟
        /// </summary>
        /// <example>true</example>
        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}