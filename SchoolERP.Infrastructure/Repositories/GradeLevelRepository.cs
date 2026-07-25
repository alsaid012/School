using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    public class GradeLevelRepository : GenericRepository<GradeLevel>, IGradeLevelRepository
    {
        public GradeLevelRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<GradeLevel>> GetBySchoolIdAsync(int schoolId)
        {
            return await _dbSet
                .Where(g => g.SchoolId == schoolId)
                .OrderBy(g => g.GradeNumber)
                .ToListAsync();
        }

        public async Task<IEnumerable<GradeLevel>> GetByStageAsync(GradeStage stage)
        {
            return await _dbSet
                .Where(g => g.GradeStage == stage)
                .Include(g => g.School)
                .OrderBy(g => g.GradeNumber)
                .ToListAsync();
        }

        public override async Task<GradeLevel?> GetWithDetailsAsync(int gradeLevelId)
        {
            return await _dbSet
                .Include(g => g.School)
                .Include(g => g.ClassRooms)
                    .ThenInclude(c => c.Students)
                .Include(g => g.Subjects)
                .FirstOrDefaultAsync(g => g.Id == gradeLevelId);
        }

        public async Task<bool> IsNameExistsAsync(int schoolId, string name, int? excludeId = null)
        {
            var query = _dbSet.Where(g => g.SchoolId == schoolId && g.GradeName == name);
            if (excludeId.HasValue)
                query = query.Where(g => g.Id != excludeId.Value);
            return await query.AnyAsync();
        }
    }
}