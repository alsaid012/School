using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.UserRoles
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  نموذج إحصائيات أدوار المستخدمين (UserRole Statistics DTO)
    /// 📌  الوظيفة: نقل إحصائيات أدوار المستخدمين من الخادم إلى العميل
    /// 📦  الاستخدام: ضمن UserRoleDetailsDto أو في لوحة تحكم الأدوار
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UserRoleStatisticsDto
    {
        /// <summary>
        /// إجمالي عدد الأدوار المسجلة
        /// </summary>
        /// <example>100</example>
        [DisplayName("إجمالي الأدوار")]
        public int TotalRoles { get; set; }

        /// <summary>
        /// عدد المستخدمين الذين لديهم دور (مستخدمين نشطين)
        /// </summary>
        /// <example>50</example>
        [DisplayName("المستخدمين النشطين")]
        public int ActiveUsersWithRoles { get; set; }

        /// <summary>
        /// عدد الأدوار من نوع طالب
        /// </summary>
        /// <example>30</example>
        [DisplayName("أدوار الطلاب")]
        public int StudentRoles { get; set; }

        /// <summary>
        /// عدد الأدوار من نوع معلم
        /// </summary>
        /// <example>40</example>
        [DisplayName("أدوار المعلمين")]
        public int TeacherRoles { get; set; }

        /// <summary>
        /// عدد الأدوار من نوع موظف
        /// </summary>
        /// <example>20</example>
        [DisplayName("أدوار الموظفين")]
        public int EmployeeRoles { get; set; }

        /// <summary>
        /// عدد الأدوار من نوع مدير
        /// </summary>
        /// <example>5</example>
        [DisplayName("أدوار المديرين")]
        public int PrincipalRoles { get; set; }

        /// <summary>
        /// عدد الأدوار من نوع أدمن
        /// </summary>
        /// <example>5</example>
        [DisplayName("أدوار الأدمن")]
        public int AdminRoles { get; set; }

        /// <summary>
        /// عدد المستخدمين الذين لديهم أكثر من دور
        /// </summary>
        /// <example>15</example>
        [DisplayName("مستخدمين متعددي الأدوار")]
        public int MultiRoleUsers { get; set; }

        /// <summary>
        /// عدد الأدوار الأساسية
        /// </summary>
        /// <example>50</example>
        [DisplayName("الأدوار الأساسية")]
        public int PrimaryRoles { get; set; }

        /// <summary>
        /// عدد الأدوار المؤقتة (لها تاريخ انتهاء)
        /// </summary>
        /// <example>20</example>
        [DisplayName("الأدوار المؤقتة")]
        public int TemporaryRoles { get; set; }

        /// <summary>
        /// توزيع الأدوار حسب النوع
        /// </summary>
        [DisplayName("توزيع الأدوار حسب النوع")]
        public Dictionary<string, int> RolesByType { get; set; } = new();

        /// <summary>
        /// أكثر المستخدمين أدواراً
        /// </summary>
        [DisplayName("أكثر المستخدمين أدواراً")]
        public List<TopUserRolesDto> TopUsersWithRoles { get; set; } = new();
    }
}