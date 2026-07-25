using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    public class AcademicYearRepository : GenericRepository<AcademicYear>, IAcademicYearRepository
    {
        /// <summary>
        /// 📆  مستودع العام الدراسي (AcademicYearRepository)
        /// 📌  الوظيفة: تنفيذ عمليات قاعدة البيانات الخاصة بالعام الدراسي
        /// 🔄  الوراثة: يرث من BaseRepository
        /// </summary>
        public AcademicYearRepository(ApplicationDbContext context) : base(context)
        {
        }
        /// <summary>
        /// 📋 الحصول على العام الدراسي الحالي لمدرسة معينة
        /// </summary>
        public async Task<AcademicYear?> GetCurrentYearAsync(int schoolId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ay => ay.SchoolId == schoolId && ay.IsCurrent);
        }

        /// <summary>
        /// 📋 الحصول على جميع الأعوام الدراسية لمدرسة معينة
        /// </summary>
        public async Task<IEnumerable<AcademicYear>> GetBySchoolIdAsync(int schoolId)
        {
            return await _dbSet
                .Where(ay => ay.SchoolId == schoolId)
                .OrderByDescending(ay => ay.StartDate)
                .ToListAsync();
        }
        /// <summary>
        /// 📋 الحصول على عام دراسي مع جميع بياناته (Override)
        /// </summary>
        public override async Task<AcademicYear?> GetWithDetailsAsync(int academicYearId)
        {
            return await _dbSet
                .Include(ay => ay.School)
                .Include(ay => ay.Students)
                    .ThenInclude(s => s.ClassRoom)
                .Include(ay => ay.Schedules)
                .Include(ay => ay.Exams)
                .FirstOrDefaultAsync(ay => ay.Id == academicYearId);
        }

        public async Task<bool> IsNameExistsAsync(int schoolId, string name, int? excludeId = null)
        {
            var query = _dbSet.Where(ay => ay.SchoolId == schoolId && ay.YearName == name);
            if (excludeId.HasValue)
                query = query.Where(ay => ay.Id != excludeId.Value);
            return await query.AnyAsync();
        }
    }
}