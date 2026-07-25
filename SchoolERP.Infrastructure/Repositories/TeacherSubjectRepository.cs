using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    public class TeacherSubjectRepository : GenericRepository<TeacherSubject>, ITeacherSubjectRepository
    {
        public TeacherSubjectRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TeacherSubject>> GetByTeacherIdAsync(int teacherId)
        {
            return await _dbSet
                .Where(ts => ts.TeacherId == teacherId)
                .Include(ts => ts.Subject)
                    .ThenInclude(s => s.GradeLevel)
                .ToListAsync();
        }

        public async Task<IEnumerable<TeacherSubject>> GetBySubjectIdAsync(int subjectId)
        {
            return await _dbSet
                .Where(ts => ts.SubjectId == subjectId)
                .Include(ts => ts.Teacher)
                    .ThenInclude(t => t.User)
                .ToListAsync();
        }

        public override async Task<TeacherSubject?> GetWithDetailsAsync(int teacherSubjectId)
        {
            return await _dbSet
                .Include(ts => ts.Teacher)
                    .ThenInclude(t => t.User)
                .Include(ts => ts.Subject)
                    .ThenInclude(s => s.GradeLevel)
                .FirstOrDefaultAsync(ts => ts.Id == teacherSubjectId);
        }

        public async Task<bool> IsExistsAsync(int teacherId, int subjectId)
        {
            return await _dbSet
                .AnyAsync(ts => ts.TeacherId == teacherId && ts.SubjectId == subjectId);
        }
    }
}