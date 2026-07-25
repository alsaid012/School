using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.GradeLevels
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📚  نموذج بيانات الصف الدراسي (GradeLevel DTO)
    /// 📌  الوظيفة: نقل بيانات الصف الدراسي من الخادم إلى العميل
    /// 📦  الاستخدام: في GradeLevelsController (GET endpoints)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class GradeLevelDto
    {
        /// <summary>
        /// معرف الصف (Primary Key)
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف الصف")]
        public int Id { get; set; }

        /// <summary>
        /// اسم الصف الدراسي
        /// </summary>
        /// <example>الصف الأول الثانوي</example>
        [DisplayName("اسم الصف")]
        public string GradeName { get; set; } = string.Empty;

        /// <summary>
        /// رقم الصف (1، 2، 3، ...)
        /// </summary>
        /// <example>1</example>
        [DisplayName("رقم الصف")]
        public int GradeNumber { get; set; }

        /// <summary>
        /// المرحلة الدراسية (ابتدائي، إعدادي، ثانوي)
        /// </summary>
        /// <example>Secondary</example>
        [DisplayName("المرحلة الدراسية")]
        public GradeStage GradeStage { get; set; }

        /// <summary>
        /// اسم المرحلة الدراسية (نص مترجم)
        /// </summary>
        /// <example>ثانوي</example>
        [DisplayName("المرحلة")]
        public string GradeStageName { get; set; } = string.Empty;

        /// <summary>
        /// وصف الصف
        /// </summary>
        /// <example>المرحلة الثانوية - السنة الأولى</example>
        [DisplayName("الوصف")]
        public string? Description { get; set; }

        /// <summary>
        /// معرف المدرسة
        /// </summary>
        /// <example>1</example>
        [DisplayName("معرف المدرسة")]
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
        /// عدد المواد في هذا الصف
        /// </summary>
        /// <example>8</example>
        [DisplayName("عدد المواد")]
        public int SubjectsCount { get; set; }

        /// <summary>
        /// عدد الطلاب في هذا الصف
        /// </summary>
        /// <example>150</example>
        [DisplayName("عدد الطلاب")]
        public int StudentsCount { get; set; }

        /// <summary>
        /// هل الصف مفعل؟
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