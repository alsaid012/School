using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// ✅  واجهة مستودع حضور الموظفين (IEmployeeAttendanceRepository)
    /// </summary>
    public interface IEmployeeAttendanceRepository : IGenericRepository<EmployeeAttendance>
    {
        /// <summary>
        /// 📋 الحصول على حضور موظف معين في تاريخ محدد
        /// </summary>
        Task<EmployeeAttendance?> GetByEmployeeAndDateAsync(int employeeId, DateTime date);

        /// <summary>
        /// 📋 الحصول على جميع سجلات حضور موظف معين
        /// </summary>
        Task<IEnumerable<EmployeeAttendance>> GetByEmployeeIdAsync(int employeeId);

        /// <summary>
        /// 📋 الحصول على سجلات الحضور لمدرسة معينة في تاريخ محدد
        /// </summary>
        Task<IEnumerable<EmployeeAttendance>> GetBySchoolAndDateAsync(int schoolId, DateTime date);

        /// <summary>
        /// 📊 الحصول على إحصائيات الحضور لموظف معين
        /// </summary>
        Task<object> GetStatisticsAsync(int employeeId, DateTime fromDate, DateTime toDate);
    }
}