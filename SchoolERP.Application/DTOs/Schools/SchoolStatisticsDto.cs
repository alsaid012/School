namespace SchoolERP.Application.DTOs.Schools
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج إحصائيات المدرسة (School Statistics DTO)
    /// 📌  الوظيفة: نقل إحصائيات المدرسة من الخادم إلى العميل
    /// 📦  الاستخدام: ضمن SchoolDto
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class SchoolStatisticsDto
    {
        /// <summary>
        /// إجمالي عدد الطلاب في المدرسة
        /// </summary>
        /// <example>500</example>
        public int TotalStudents { get; set; }

        /// <summary>
        /// إجمالي عدد المعلمين في المدرسة
        /// </summary>
        /// <example>50</example>
        public int TotalTeachers { get; set; }

        /// <summary>
        /// إجمالي عدد الموظفين في المدرسة
        /// </summary>
        /// <example>20</example>
        public int TotalEmployees { get; set; }

        /// <summary>
        /// إجمالي عدد الفصول الدراسية
        /// </summary>
        /// <example>25</example>
        public int TotalClassRooms { get; set; }

        /// <summary>
        /// إجمالي عدد الصفوف الدراسية
        /// </summary>
        /// <example>9</example>
        public int TotalGradeLevels { get; set; }

        /// <summary>
        /// إجمالي عدد الأعوام الدراسية
        /// </summary>
        /// <example>3</example>
        public int TotalAcademicYears { get; set; }
    }
}