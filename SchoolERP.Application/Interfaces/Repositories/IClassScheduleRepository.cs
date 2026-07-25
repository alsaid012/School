using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📅  واجهة مستودع جدول الحصص (IClassScheduleRepository)
    /// 📌  الوظيفة: تعريف العمليات الخاصة بجدول الحصص
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IClassScheduleRepository : IGenericRepository<ClassSchedule>
    {
        /// <summary>
        /// 📋 الحصول على جميع الحصص في عام دراسي معين
        /// </summary>
        Task<IEnumerable<ClassSchedule>> GetByAcademicYearIdAsync(int academicYearId);

        /// <summary>
        /// 📋 الحصول على جميع الحصص لفصل معين
        /// </summary>
        Task<IEnumerable<ClassSchedule>> GetByClassRoomIdAsync(int classRoomId);

        /// <summary>
        /// 📋 الحصول على جميع الحصص لمعلم معين
        /// </summary>
        Task<IEnumerable<ClassSchedule>> GetByTeacherIdAsync(int teacherId);

        /// <summary>
        /// 📋 الحصول على جميع الحصص لمادة معينة
        /// </summary>
        Task<IEnumerable<ClassSchedule>> GetBySubjectIdAsync(int subjectId);

        /// <summary>
        /// 📋 الحصول على جدول فصل معين في يوم معين
        /// </summary>
        Task<IEnumerable<ClassSchedule>> GetByClassRoomAndDayAsync(int classRoomId, DayOfWeek day);

        /// <summary>
        /// 📋 الحصول على جدول معلم معين في يوم معين
        /// </summary>
        Task<IEnumerable<ClassSchedule>> GetByTeacherAndDayAsync(int teacherId, DayOfWeek day);

        ///// <summary>
        ///// 📋 الحصول على جدول الحصص مع جميع البيانات المرتبطة
        ///// </summary>
        //Task<ClassSchedule?> GetWithDetailsAsync(int scheduleId);

        /// <summary>
        /// 📋 الحصول على جميع الحصص في فترة زمنية معينة
        /// </summary>
        Task<IEnumerable<ClassSchedule>> GetByTimeRangeAsync(TimeSpan startTime, TimeSpan endTime);

        /// <summary>
        /// ✅ التحقق من وجود تعارض في الجدول
        /// </summary>
        Task<bool> IsConflictExistsAsync(int classRoomId, int academicYearId, DayOfWeek day, TimeSpan startTime, int? excludeId = null);

        /// <summary>
        /// ✅ التحقق من وجود تعارض مع معلم
        /// </summary>
        Task<bool> IsTeacherConflictExistsAsync(int teacherId, int academicYearId, DayOfWeek day, TimeSpan startTime, int? excludeId = null);

        /// <summary>
        /// 📋 الحصول على جدول مدرسة معينة
        /// </summary>
        Task<IEnumerable<ClassSchedule>> GetBySchoolIdAsync(int schoolId);
    }
}