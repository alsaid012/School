using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.Users
{
    public class UserStatisticsDto
    {
        // ✅ إحصائيات المستخدم الفردي
        [DisplayName("عدد مرات تسجيل الدخول")]
        public int LoginCount { get; set; }

        [DisplayName("عدد جهات الاتصال")]
        public int ContactsCount { get; set; }

        [DisplayName("عدد الأدوار")]
        public int RolesCount { get; set; }

        [DisplayName("مدة العضوية (بالأيام)")]
        public int MembershipDays { get; set; }

        [DisplayName("تاريخ آخر نشاط")]
        public DateTime? LastActivityDate { get; set; }

        // ✅ إحصائيات عامة (للمستخدم الحالي أو للكل)
        [DisplayName("إجمالي المستخدمين")]
        public int TotalUsers { get; set; }

        [DisplayName("المستخدمين النشطين")]
        public int ActiveUsers { get; set; }

        [DisplayName("المستخدمين المعلقين")]
        public int PendingUsers { get; set; }

        [DisplayName("المستخدمين الموقوفين")]
        public int SuspendedUsers { get; set; }

        [DisplayName("المستخدمين غير النشطين")]
        public int InactiveUsers { get; set; }

        [DisplayName("عدد الطلاب")]
        public int StudentsCount { get; set; }

        [DisplayName("عدد المعلمين")]
        public int TeachersCount { get; set; }

        [DisplayName("عدد الموظفين")]
        public int EmployeesCount { get; set; }

        [DisplayName("عدد الأدمن")]
        public int AdminsCount { get; set; }

        // ✅ توزيع حسب النوع والحالة
        public Dictionary<UserType, int> UsersByType { get; set; } = new();
        public Dictionary<UserStatus, int> UsersByStatus { get; set; } = new();
    }
}