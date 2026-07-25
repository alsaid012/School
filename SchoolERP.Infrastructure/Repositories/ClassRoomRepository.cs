using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    public class ClassRoomRepository : GenericRepository<ClassRoom>, IClassRoomRepository
    {
        public ClassRoomRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ClassRoom>> GetByGradeLevelIdAsync(int gradeLevelId)
        {
            return await _dbSet
                .Where(c => c.GradeLevelId == gradeLevelId)
                .OrderBy(c => c.ClassName)
                .ToListAsync();
        }

        public async Task<IEnumerable<ClassRoom>> GetByTeacherIdAsync(int teacherId)
        {
            return await _dbSet
                .Where(c => c.TeacherId == teacherId)
                .Include(c => c.GradeLevel)
                .OrderBy(c => c.ClassName)
                .ToListAsync();
        }

        public override async Task<ClassRoom?> GetWithDetailsAsync(int classRoomId)
        {
            return await _dbSet
                .Include(c => c.GradeLevel)
                    .ThenInclude(g => g.School)
                .Include(c => c.Teacher)
                    .ThenInclude(t =>t !=null ? t.User:null!)
                .Include(c => c.Students)
                    .ThenInclude(s => s.User)
                .Include(c => c.Schedules)
                    .ThenInclude(cs => cs.Subject)
                .Include(c => c.Exams)
                .FirstOrDefaultAsync(c => c.Id == classRoomId);
        }

        public async Task<IEnumerable<ClassRoom>> GetBySchoolIdAsync(int schoolId)
        {
            return await _dbSet
                .Where(c => c.GradeLevel.SchoolId == schoolId)
                .Include(c => c.GradeLevel)
                .OrderBy(c => c.ClassName)
                .ToListAsync();
        }

        public async Task<bool> IsNameExistsAsync(int gradeLevelId, string name, int? excludeId = null)
        {
            var query = _dbSet.Where(c => c.GradeLevelId == gradeLevelId && c.ClassName == name);
            if (excludeId.HasValue)
                query = query.Where(c => c.Id != excludeId.Value);
            return await query.AnyAsync();
        }
    }
}