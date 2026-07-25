using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Employees
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج إحصائيات الموظف (Employee Statistics DTO)
    /// 📌  الوظيفة: نقل إحصائيات الموظف من الخادم إلى العميل
    /// 📦  الاستخدام: ضمن EmployeeDetailsDto أو في لوحة تحكم الموظف
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class EmployeeStatisticsDto
    {
        /// <summary>
        /// عدد سنوات الخبرة
        /// </summary>
        /// <example>5</example>
        [DisplayName("سنوات الخبرة")]
        public int YearsOfExperience { get; set; }

        /// <summary>
        /// عدد الأيام التي تغيب فيها الموظف
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
        /// عدد أيام الحضور
        /// </summary>
        /// <example>45</example>
        [DisplayName("أيام الحضور")]
        public int PresentDays { get; set; }

        /// <summary>
        /// نسبة الحضور
        /// </summary>
        /// <example>95.0</example>
        [DisplayName("نسبة الحضور")]
        public decimal AttendancePercentage { get; set; }

        /// <summary>
        /// عدد المهام المنجزة
        /// </summary>
        /// <example>12</example>
        [DisplayName("المهام المنجزة")]
        public int CompletedTasks { get; set; }

        /// <summary>
        /// عدد المهام المعلقة
        /// </summary>
        /// <example>3</example>
        [DisplayName("المهام المعلقة")]
        public int PendingTasks { get; set; }

        /// <summary>
        /// تقييم الأداء (من 5)
        /// </summary>
        /// <example>4.5</example>
        [DisplayName("تقييم الأداء")]
        public decimal PerformanceRating { get; set; }
    }
}