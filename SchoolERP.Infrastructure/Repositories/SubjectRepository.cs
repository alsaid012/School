using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(ApplicationDbContext context) : base(context)
        {
        }

        //public async Task<Subject?> GetByCodeAsync(string code)
        //{
        //    return await _dbSet
        //        .FirstOrDefaultAsync(s => s.SubjectCode == code);
        //}

        public async Task<IEnumerable<Subject>> GetByGradeLevelIdAsync(int gradeLevelId)
        {
            return await _dbSet
                .Where(s => s.GradeLevelId == gradeLevelId)
                .OrderBy(s => s.SubjectName)
                .ToListAsync();
        }

        public override async Task<Subject?> GetWithDetailsAsync(int subjectId)
        {
            return await _dbSet
                .Include(s => s.GradeLevel)
                    .ThenInclude(g => g.School)
                .Include(s => s.TeacherSubjects)
                    .ThenInclude(ts => ts.Teacher)
                        .ThenInclude(t => t.User)
                .Include(s => s.Schedules)
                    .ThenInclude(cs => cs.ClassRoom)
                .Include(s => s.Exams)
                .FirstOrDefaultAsync(s => s.Id == subjectId);
        }

        public async Task<IEnumerable<Subject>> GetByTeacherIdAsync(int teacherId)
        {
            return await _dbSet
                .Where(s => s.TeacherSubjects.Any(ts => ts.TeacherId == teacherId))
                .Include(s => s.GradeLevel)
                .OrderBy(s => s.SubjectName)
                .ToListAsync();
        }

        public async Task<bool> IsNameExistsAsync(int gradeLevelId, string name, int? excludeId = null)
        {
            var query = _dbSet.Where(s => s.GradeLevelId == gradeLevelId && s.SubjectName == name);
            if (excludeId.HasValue)
                query = query.Where(s => s.Id != excludeId.Value);
            return await query.AnyAsync();
        }
    }
}