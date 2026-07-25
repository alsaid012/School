using SchoolERP.Application.DTOs.Users;
using System.ComponentModel;

namespace SchoolERP.Application.DTOs.Teachers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 👨‍🏫  نموذج تفاصيل المعلم (Teacher Details DTO)
    /// 📌  الوظيفة: نقل بيانات المعلم مع التفاصيل الكاملة
    /// 📦  الاستخدام: في TeachersController (GET /{id} endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class TeacherDetailsDto : TeacherDto
    {
        /// <summary>
        /// بيانات المستخدم الكاملة
        /// </summary>
        [DisplayName("بيانات المستخدم")]
        public UserDetailsDto? User { get; set; }

        /// <summary>
        /// إحصائيات المعلم
        /// </summary>
        [DisplayName("إحصائيات المعلم")]
        public TeacherStatisticsDto? Statistics { get; set; }

        // ❌ تم إزالة Subjects و ClassRooms لأنهم موجودين بالفعل في TeacherDto
    }
}