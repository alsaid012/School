namespace SchoolERP.Application.DTOs.Departments
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج إحصائيات الإدارة التعليمية (Department Statistics DTO)
    /// 📌  الوظيفة: نقل إحصائيات الإدارة من الخادم إلى العميل
    /// 📦  الاستخدام: ضمن DepartmentDetailsDto
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class DepartmentStatisticsDto
    {
        /// <summary>
        /// إجمالي عدد المدارس التابعة للإدارة
        /// </summary>
        /// <example>10</example>
        public int TotalSchools { get; set; }

        /// <summary>
        /// إجمالي عدد الطلاب في جميع المدارس
        /// </summary>
        /// <example>5000</example>
        public int TotalStudents { get; set; }

        /// <summary>
        /// إجمالي عدد المعلمين في جميع المدارس
        /// </summary>
        /// <example>500</example>
        public int TotalTeachers { get; set; }

        /// <summary>
        /// إجمالي عدد الموظفين في جميع المدارس
        /// </summary>
        /// <example>200</example>
        public int TotalEmployees { get; set; }
    }
}