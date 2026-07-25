using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// ✅  واجهة مستودع حضور الطلاب (IStudentAttendanceRepository)
    /// </summary>
    public interface IStudentAttendanceRepository : IGenericRepository<StudentAttendance>
    {
        /// <summary>
        /// 📋 الحصول على حضور طالب معين في تاريخ محدد
        /// </summary>
        Task<StudentAttendance?> GetByStudentAndDateAsync(int studentId, DateTime date);

        /// <summary>
        /// 📋 الحصول على جميع سجلات حضور طالب معين
        /// </summary>
        Task<IEnumerable<StudentAttendance>> GetByStudentIdAsync(int studentId);

        /// <summary>
        /// 📋 الحصول على سجلات الحضور لفصل معين في تاريخ محدد
        /// </summary>
        Task<IEnumerable<StudentAttendance>> GetByClassRoomAndDateAsync(int classRoomId, DateTime date);

        /// <summary>
        /// 📋 الحصول على سجلات الحضور لصف معين في تاريخ محدد
        /// </summary>
        Task<IEnumerable<StudentAttendance>> GetByGradeLevelAndDateAsync(int gradeLevelId, DateTime date);

        /// <summary>
        /// 📊 الحصول على إحصائيات الحضور لطالب معين
        /// </summary>
        Task<object> GetStatisticsAsync(int studentId, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// 📊 الحصول على تقرير الحضور اليومي لمدرسة معينة
        /// </summary>
        Task<object> GetDailyReportAsync(int schoolId, DateTime date);
    }
}