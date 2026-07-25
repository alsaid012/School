using System.ComponentModel;

namespace SchoolERP.Application.DTOs.Teachers
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج إحصائيات المعلم (Teacher Statistics DTO)
    /// 📌  الوظيفة: نقل إحصائيات المعلم من الخادم إلى العميل
    /// 📦  الاستخدام: ضمن TeacherDetailsDto أو في لوحة تحكم المعلم
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class TeacherStatisticsDto
    {
        /// <summary>
        /// عدد المواد التي يدرسها المعلم
        /// </summary>
        /// <example>5</example>
        [DisplayName("عدد المواد")]
        public int TotalSubjects { get; set; }

        /// <summary>
        /// عدد الفصول التي يدرس فيها المعلم
        /// </summary>
        /// <example>3</example>
        [DisplayName("عدد الفصول")]
        public int TotalClassRooms { get; set; }

        /// <summary>
        /// عدد الطلاب الذين يدرسهم المعلم
        /// </summary>
        /// <example>120</example>
        [DisplayName("عدد الطلاب")]
        public int TotalStudents { get; set; }

        /// <summary>
        /// عدد الحصص الأسبوعية
        /// </summary>
        /// <example>16</example>
        [DisplayName("عدد الحصص الأسبوعية")]
        public int WeeklyHours { get; set; }

        /// <summary>
        /// عدد الامتحانات التي وضعها المعلم
        /// </summary>
        /// <example>8</example>
        [DisplayName("عدد الامتحانات")]
        public int TotalExams { get; set; }

        /// <summary>
        /// متوسط درجات طلابه
        /// </summary>
        /// <example>78.5</example>
        [DisplayName("متوسط درجات الطلاب")]
        public decimal AverageStudentScore { get; set; }

        /// <summary>
        /// نسبة نجاح طلابه
        /// </summary>
        /// <example>85.0</example>
        [DisplayName("نسبة نجاح الطلاب")]
        public decimal StudentSuccessRate { get; set; }

        /// <summary>
        /// عدد سنوات الخبرة
        /// </summary>
        /// <example>10</example>
        [DisplayName("سنوات الخبرة")]
        public int YearsOfExperience { get; set; }

        /// <summary>
        /// عدد الفصول التي يشرف عليها (إذا كان معلم فصل)
        /// </summary>
        /// <example>2</example>
        [DisplayName("الفصول المشرف عليها")]
        public int HomeroomClassRoomsCount { get; set; }

        /// <summary>
        /// عدد الطلاب في فصوله (إذا كان معلم فصل)
        /// </summary>
        /// <example>60</example>
        [DisplayName("طلاب الفصول المشرف عليها")]
        public int HomeroomStudentsCount { get; set; }

        /// <summary>
        /// عدد الأيام التي تغيب فيها المعلم
        /// </summary>
        /// <example>2</example>
        [DisplayName("أيام الغياب")]
        public int AbsentDays { get; set; }

        /// <summary>
        /// عدد أيام التأخير
        /// </summary>
        /// <example>1</example>
        [DisplayName("أيام التأخير")]
        public int LateDays { get; set; }

        /// <summary>
        /// نسبة الحضور
        /// </summary>
        /// <example>95.0</example>
        [DisplayName("نسبة الحضور")]
        public decimal AttendancePercentage { get; set; }
    }
}