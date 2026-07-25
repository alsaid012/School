using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.ClassRooms
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج إحصائيات الفصل الدراسي (ClassRoom Statistics DTO)
    /// 📌  الوظيفة: نقل إحصائيات الفصل من الخادم إلى العميل
    /// 📦  الاستخدام: ضمن ClassRoomDetailsDto أو في لوحة تحكم الفصل
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class ClassRoomStatisticsDto
    {
        /// <summary>
        /// عدد الطلاب في الفصل
        /// </summary>
        /// <example>25</example>
        [DisplayName("عدد الطلاب")]
        public int TotalStudents { get; set; }

        /// <summary>
        /// عدد الحصص الأسبوعية في هذا الفصل
        /// </summary>
        /// <example>30</example>
        [DisplayName("عدد الحصص الأسبوعية")]
        public int WeeklyHours { get; set; }

        /// <summary>
        /// عدد المواد التي تدرس في هذا الفصل
        /// </summary>
        /// <example>8</example>
        [DisplayName("عدد المواد")]
        public int TotalSubjects { get; set; }

        /// <summary>
        /// عدد المعلمين الذين يدرسون في هذا الفصل
        /// </summary>
        /// <example>10</example>
        [DisplayName("عدد المعلمين")]
        public int TotalTeachers { get; set; }

        /// <summary>
        /// عدد الامتحانات في هذا الفصل
        /// </summary>
        /// <example>12</example>
        [DisplayName("عدد الامتحانات")]
        public int TotalExams { get; set; }

        /// <summary>
        /// نسبة الحضور في هذا الفصل
        /// </summary>
        /// <example>90.0</example>
        [DisplayName("نسبة الحضور")]
        public decimal AttendanceRate { get; set; }

        /// <summary>
        /// نسبة النجاح في هذا الفصل
        /// </summary>
        /// <example>85.0</example>
        [DisplayName("نسبة النجاح")]
        public decimal SuccessRate { get; set; }

        /// <summary>
        /// نسبة إشغال الفصل (عدد الطلاب / السعة)
        /// </summary>
        /// <example>83.3</example>
        [DisplayName("نسبة الإشغال")]
        public decimal OccupancyRate { get; set; }

        /// <summary>
        /// عدد الطلاب الذكور
        /// </summary>
        /// <example>15</example>
        [DisplayName("الطلاب الذكور")]
        public int MaleStudents { get; set; }

        /// <summary>
        /// عدد الطلاب الإناث
        /// </summary>
        /// <example>10</example>
        [DisplayName("الطلاب الإناث")]
        public int FemaleStudents { get; set; }
    }
}