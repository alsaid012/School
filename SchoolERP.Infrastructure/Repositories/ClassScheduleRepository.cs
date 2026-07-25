using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    /// <summary>
    /// 📅  مستودع جدول الحصص (ClassScheduleRepository)
    /// 📌  الوظيفة: تنفيذ عمليات قاعدة البيانات الخاصة بجدول الحصص
    /// </summary>
    public class ClassScheduleRepository : GenericRepository<ClassSchedule>, IClassScheduleRepository
    {
        public ClassScheduleRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 📋 الحصول على جميع الحصص في عام دراسي معين
        /// </summary>
        public async Task<IEnumerable<ClassSchedule>> GetByAcademicYearIdAsync(int academicYearId)
        {
            return await _dbSet
                .Where(cs => cs.AcademicYearId == academicYearId)
                .Include(cs => cs.ClassRoom)
                    .ThenInclude(c => c.GradeLevel)
                .Include(cs => cs.Subject)
                .Include(cs => cs.Teacher)
                    .ThenInclude(t => t.User)
                .OrderBy(cs => cs.DayOfWeek)
                .ThenBy(cs => cs.StartTime)
                .ToListAsync();
        }

        /// <summary>
        /// 📋 الحصول على جميع الحصص لفصل معين
        /// </summary>
        public async Task<IEnumerable<ClassSchedule>> GetByClassRoomIdAsync(int classRoomId)
        {
            return await _dbSet
                .Where(cs => cs.ClassRoomId == classRoomId)
                .Include(cs => cs.Subject)
                .Include(cs => cs.Teacher)
                    .ThenInclude(t => t.User)
                .OrderBy(cs => cs.DayOfWeek)
                .ThenBy(cs => cs.StartTime)
                .ToListAsync();
        }

        /// <summary>
        /// 📋 الحصول على جميع الحصص لمعلم معين
        /// </summary>
        public async Task<IEnumerable<ClassSchedule>> GetByTeacherIdAsync(int teacherId)
        {
            return await _dbSet
                .Where(cs => cs.TeacherId == teacherId)
                .Include(cs => cs.ClassRoom)
                    .ThenInclude(c => c.GradeLevel)
                .Include(cs => cs.Subject)
                .OrderBy(cs => cs.DayOfWeek)
                .ThenBy(cs => cs.StartTime)
                .ToListAsync();
        }

        /// <summary>
        /// 📋 الحصول على جميع الحصص لمادة معينة
        /// </summary>
        public async Task<IEnumerable<ClassSchedule>> GetBySubjectIdAsync(int subjectId)
        {
            return await _dbSet
                .Where(cs => cs.SubjectId == subjectId)
                .Include(cs => cs.ClassRoom)
                    .ThenInclude(c => c.GradeLevel)
                .Include(cs => cs.Teacher)
                    .ThenInclude(t => t.User)
                .OrderBy(cs => cs.DayOfWeek)
                .ThenBy(cs => cs.StartTime)
                .ToListAsync();
        }

        /// <summary>
        /// 📋 الحصول على جدول فصل معين في يوم معين
        /// </summary>
        public async Task<IEnumerable<ClassSchedule>> GetByClassRoomAndDayAsync(int classRoomId, DayOfWeek day)
        {
            return await _dbSet
                .Where(cs => cs.ClassRoomId == classRoomId && cs.DayOfWeek == day)
                .Include(cs => cs.Subject)
                .Include(cs => cs.Teacher)
                    .ThenInclude(t => t.User)
                .OrderBy(cs => cs.StartTime)
                .ToListAsync();
        }

        /// <summary>
        /// 📋 الحصول على جدول معلم معين في يوم معين
        /// </summary>
        public async Task<IEnumerable<ClassSchedule>> GetByTeacherAndDayAsync(int teacherId, DayOfWeek day)
        {
            return await _dbSet
                .Where(cs => cs.TeacherId == teacherId && cs.DayOfWeek == day)
                .Include(cs => cs.ClassRoom)
                    .ThenInclude(c => c.GradeLevel)
                .Include(cs => cs.Subject)
                .OrderBy(cs => cs.StartTime)
                .ToListAsync();
        }

        /// <summary>
        /// 📋 الحصول على جدول الحصص مع جميع البيانات المرتبطة
        /// </summary>
        public override async Task<ClassSchedule?> GetWithDetailsAsync(int scheduleId)
        {
            return await _dbSet
                .Include(cs => cs.AcademicYear)
                .Include(cs => cs.ClassRoom)
                    .ThenInclude(c => c.GradeLevel)
                        .ThenInclude(g => g.School)
                .Include(cs => cs.Subject)
                .Include(cs => cs.Teacher)
                    .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(cs => cs.Id == scheduleId);
        }

        /// <summary>
        /// 📋 الحصول على جميع الحصص في فترة زمنية معينة
        /// </summary>
        public async Task<IEnumerable<ClassSchedule>> GetByTimeRangeAsync(TimeSpan startTime, TimeSpan endTime)
        {
            return await _dbSet
                .Where(cs => cs.StartTime >= startTime && cs.EndTime <= endTime)
                .Include(cs => cs.ClassRoom)
                .Include(cs => cs.Subject)
                .Include(cs => cs.Teacher)
                .OrderBy(cs => cs.DayOfWeek)
                .ThenBy(cs => cs.StartTime)
                .ToListAsync();
        }

        /// <summary>
        /// ✅ التحقق من وجود تعارض في الجدول (نفس الفصل في نفس اليوم ونفس الوقت)
        /// </summary>
        public async Task<bool> IsConflictExistsAsync(int classRoomId, int academicYearId, DayOfWeek day, TimeSpan startTime, int? excludeId = null)
        {
            var query = _dbSet.Where(cs =>
                cs.ClassRoomId == classRoomId &&
                cs.AcademicYearId == academicYearId &&
                cs.DayOfWeek == day &&
                cs.StartTime == startTime);

            if (excludeId.HasValue)
                query = query.Where(cs => cs.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        /// <summary>
        /// ✅ التحقق من وجود تعارض مع معلم (نفس المعلم في نفس اليوم ونفس الوقت)
        /// </summary>
        public async Task<bool> IsTeacherConflictExistsAsync(int teacherId, int academicYearId, DayOfWeek day, TimeSpan startTime, int? excludeId = null)
        {
            var query = _dbSet.Where(cs =>
                cs.TeacherId == teacherId &&
                cs.AcademicYearId == academicYearId &&
                cs.DayOfWeek == day &&
                cs.StartTime == startTime);

            if (excludeId.HasValue)
                query = query.Where(cs => cs.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        /// <summary>
        /// 📋 الحصول على جدول مدرسة معينة
        /// </summary>
        public async Task<IEnumerable<ClassSchedule>> GetBySchoolIdAsync(int schoolId)
        {
            return await _dbSet
                .Where(cs => cs.ClassRoom.GradeLevel.SchoolId == schoolId)
                .Include(cs => cs.ClassRoom)
                    .ThenInclude(c => c.GradeLevel)
                .Include(cs => cs.Subject)
                .Include(cs => cs.Teacher)
                    .ThenInclude(t => t.User)
                .OrderBy(cs => cs.DayOfWeek)
                .ThenBy(cs => cs.StartTime)
                .ToListAsync();
        }

        /// <summary>
        /// 📋 الحصول على الجدول الأسبوعي لفصل معين
        /// </summary>
        public async Task<Dictionary<DayOfWeek, IEnumerable<ClassSchedule>>> GetWeeklyScheduleAsync(int classRoomId)
        {
            var schedules = await _dbSet
                .Where(cs => cs.ClassRoomId == classRoomId)
                .Include(cs => cs.Subject)
                .Include(cs => cs.Teacher)
                    .ThenInclude(t => t.User)
                .OrderBy(cs => cs.DayOfWeek)
                .ThenBy(cs => cs.StartTime)
                .ToListAsync();

            return schedules
                .GroupBy(cs => cs.DayOfWeek)
                .ToDictionary(g => g.Key, g => g.AsEnumerable());
        }
    }
}