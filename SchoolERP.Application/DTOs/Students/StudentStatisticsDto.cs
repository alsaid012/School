using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Students
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج إحصائيات الطالب (Student Statistics DTO)
    /// 📌  الوظيفة: نقل إحصائيات الطالب من الخادم إلى العميل
    /// 📦  الاستخدام: ضمن StudentDetailsDto أو في لوحة تحكم الطالب
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class StudentStatisticsDto
    {
        /// <summary>
        /// إجمالي عدد الامتحانات التي خاضها الطالب
        /// </summary>
        /// <example>10</example>
        [DisplayName("إجمالي الامتحانات")]
        public int TotalExams { get; set; }

        /// <summary>
        /// متوسط درجات الطالب
        /// </summary>
        /// <example>85.5</example>
        [DisplayName("المتوسط")]
        public decimal AverageScore { get; set; }

        /// <summary>
        /// أعلى درجة حصل عليها الطالب
        /// </summary>
        /// <example>95</example>
        [DisplayName("أعلى درجة")]
        public int MaxScore { get; set; }

        /// <summary>
        /// أدنى درجة حصل عليها الطالب
        /// </summary>
        /// <example>70</example>
        [DisplayName("أدنى درجة")]
        public int MinScore { get; set; }

        /// <summary>
        /// عدد أيام الحضور
        /// </summary>
        /// <example>45</example>
        [DisplayName("أيام الحضور")]
        public int PresentDays { get; set; }

        /// <summary>
        /// عدد أيام الغياب
        /// </summary>
        /// <example>3</example>
        [DisplayName("أيام الغياب")]
        public int AbsentDays { get; set; }

        /// <summary>
        /// عدد أيام التأخير
        /// </summary>
        /// <example>2</example>
        [DisplayName("أيام التأخير")]
        public int LateDays { get; set; }

        /// <summary>
        /// عدد الأيام المعذور فيها
        /// </summary>
        /// <example>1</example>
        [DisplayName("أيام العذر")]
        public int ExcusedDays { get; set; }

        /// <summary>
        /// نسبة الحضور (مئوية)
        /// </summary>
        /// <example>90.0</example>
        [DisplayName("نسبة الحضور")]
        public decimal AttendancePercentage { get; set; }

        /// <summary>
        /// عدد المواد التي يدرسها الطالب
        /// </summary>
        /// <example>8</example>
        [DisplayName("عدد المواد")]
        public int SubjectsCount { get; set; }

        /// <summary>
        /// عدد المواد التي نجح فيها الطالب
        /// </summary>
        /// <example>7</example>
        [DisplayName("عدد المواد الناجح فيها")]
        public int PassedSubjects { get; set; }

        /// <summary>
        /// عدد المواد التي رسب فيها الطالب
        /// </summary>
        /// <example>1</example>
        [DisplayName("عدد المواد الراسب فيها")]
        public int FailedSubjects { get; set; }

        /// <summary>
        /// الترتيب في الصف (إذا كان متاحاً)
        /// </summary>
        /// <example>5</example>
        [DisplayName("الترتيب في الصف")]
        public int? ClassRank { get; set; }

        /// <summary>
        /// إجمالي عدد الطلاب في الصف
        /// </summary>
        /// <example>30</example>
        [DisplayName("إجمالي طلاب الصف")]
        public int? TotalStudentsInClass { get; set; }

        /// <summary>
        /// نسبة النجاح (مئوية)
        /// </summary>
        /// <example>87.5</example>
        [DisplayName("نسبة النجاح")]
        public decimal SuccessPercentage { get; set; }
    }
}