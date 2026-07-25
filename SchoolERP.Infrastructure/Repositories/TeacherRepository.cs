using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(ApplicationDbContext context) : base(context)
        {
        }

        //public async Task<Teacher?> GetByCodeAsync(string code)
        //{
        //    return await _dbSet
        //        .FirstOrDefaultAsync(t => t.TeacherCode == code);
        //}
        public override async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await _dbSet
                .Include(t => t.User)  // ✅ تأكد من وجود Include
                .ToListAsync();
        }

        public async Task<IEnumerable<Teacher>> GetBySchoolIdAsync(int schoolId)
        {
            return await _dbSet
                .Where(t => t.User.SchoolId == schoolId)
                .Include(t => t.User)
                .OrderBy(t => t.User.FullName)
                .ToListAsync();
        }

        public override async Task<Teacher?> GetWithDetailsAsync(int teacherId)
        {
            return await _dbSet
                .Include(t => t.User)
                    .ThenInclude(u => u.Contacts)
                .Include(t => t.TeacherSubjects)
                    .ThenInclude(ts => ts.Subject)
                        .ThenInclude(s => s.GradeLevel)
                .Include(t => t.Schedules)
                    .ThenInclude(cs => cs.ClassRoom)
                .Include(t => t.Schedules)
                    .ThenInclude(cs => cs.Subject)
                .Include(t => t.ClassRooms)
                .Include(t => t.Exams)
                .FirstOrDefaultAsync(t => t.Id == teacherId);
        }

        public async Task<IEnumerable<Teacher>> GetBySpecializationAsync(string specialization)
        {
            return await _dbSet
                .Where(t => t.Specialization == specialization)
                .Include(t => t.User)
                .OrderBy(t => t.User.FullName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Teacher>> GetHomeroomTeachersAsync()
        {
            return await _dbSet
                .Where(t => t.IsHomeroomTeacher)
                .Include(t => t.User)
                .OrderBy(t => t.User.FullName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Teacher>> GetBySubjectIdAsync(int subjectId)
        {
            return await _dbSet
                .Where(t => t.TeacherSubjects.Any(ts => ts.SubjectId == subjectId))
                .Include(t => t.User)
                .Include(t => t.TeacherSubjects)
                .OrderBy(t => t.User.FullName)
                .ToListAsync();
        }

        public async Task<bool> IsTeacherCodeExistsAsync(string teacherCode)
        {
            return await _dbSet
                .AnyAsync(t => t.TeacherCode == teacherCode);
        }

    }
}