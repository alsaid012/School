using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.Schools
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏫  نموذج بيانات المدرسة (School DTO)
    /// 📌  الوظيفة: نقل بيانات المدرسة من الخادم إلى العميل
    /// 📦  الاستخدام: في SchoolsController (GET endpoints)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class SchoolDto
    {
        /// <summary>
        /// معرف المدرسة (Primary Key)
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// اسم المدرسة
        /// </summary>
        /// <example>مدرسة النصر الثانوية</example>
        public string SchoolName { get; set; } = string.Empty;

        /// <summary>
        /// كود المدرسة (فريد)
        /// </summary>
        /// <example>SCH-001</example>
        public string SchoolCode { get; set; } = string.Empty;

        /// <summary>
        /// نوع المدرسة
        /// </summary>
        /// <example>Public</example>
        public SchoolType SchoolType { get; set; }

        /// <summary>
        /// عنوان المدرسة
        /// </summary>
        /// <example>مصر الجديدة - القاهرة</example>
        public string? Address { get; set; }

        /// <summary>
        /// رقم هاتف المدرسة
        /// </summary>
        /// <example>0223456789</example>
        public string? Phone { get; set; }

        /// <summary>
        /// البريد الإلكتروني للمدرسة
        /// </summary>
        /// <example>school@example.com</example>
        public string? Email { get; set; }

        /// <summary>
        /// اسم مدير المدرسة
        /// </summary>
        /// <example>أ. حسين علي</example>
        public string? PrincipalName { get; set; }

        /// <summary>
        /// سنة تأسيس المدرسة
        /// </summary>
        /// <example>1990</example>
        public int? EstablishedYear { get; set; }

        /// <summary>
        /// معرف الإدارة التعليمية التابعة لها
        /// </summary>
        /// <example>1</example>
        public int DepartmentId { get; set; }

        /// <summary>
        /// اسم الإدارة التعليمية
        /// </summary>
        /// <example>إدارة شمال القاهرة التعليمية</example>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// اسم المحافظة
        /// </summary>
        /// <example>القاهرة</example>
        public string? GovernorateName { get; set; }

        /// <summary>
        /// عدد الطلاب
        /// </summary>
        public int StudentsCount { get; set; }

        /// <summary>
        /// عدد المعلمين
        /// </summary>
        public int TeachersCount { get; set; }


        /// <summary>
        /// هل المدرسة مفعلة؟
        /// </summary>
        /// <example>true</example>
        public bool IsActive { get; set; }
        /// <summary>
        /// تاريخ الإنشاء
        /// </summary>
        /// <example>2024-01-01T12:00:00</example>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// تاريخ آخر تحديث
        /// </summary>
        /// <example>2024-01-15T10:30:00</example>
        public DateTime? UpdatedAt { get; set; }
    }
}