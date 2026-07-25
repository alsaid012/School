using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    public class ExamRepository : GenericRepository<Exam>, IExamRepository
    {
        public ExamRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Exam>> GetByAcademicYearIdAsync(int academicYearId)
        {
            return await _dbSet
                .Where(e => e.AcademicYearId == academicYearId)
                .Include(e => e.Subject)
                .Include(e => e.Teacher)
                    .ThenInclude(t =>t !=null? t.User:null!)
                .OrderBy(e => e.ExamDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exam>> GetBySubjectIdAsync(int subjectId)
        {
            return await _dbSet
                .Where(e => e.SubjectId == subjectId)
                .Include(e => e.AcademicYear)
                .Include(e => e.Teacher)
                .OrderBy(e => e.ExamDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exam>> GetByClassRoomIdAsync(int classRoomId)
        {
            return await _dbSet
                .Where(e => e.ClassRoomId == classRoomId)
                .Include(e => e.Subject)
                .Include(e => e.Teacher)
                .OrderBy(e => e.ExamDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exam>> GetUpcomingExamsAsync(DateTime fromDate)
        {
            return await _dbSet
                .Where(e => e.ExamDate >= fromDate)
                .Include(e => e.Subject)
                .Include(e => e.ClassRoom)
                    .ThenInclude(c => c != null ? c.GradeLevel : null!)
                .OrderBy(e => e.ExamDate)
                .ToListAsync();
        }

        public override async Task<Exam?> GetWithDetailsAsync(int examId)
        {
            return await _dbSet
                .Include(e => e.AcademicYear)
                .Include(e => e.Subject)
                    .ThenInclude(s => s.GradeLevel)
                .Include(e => e.ClassRoom)
                     .ThenInclude(c => c != null ? c.GradeLevel : null!)
                .Include(e => e.Teacher)
                    .ThenInclude(t => t != null ? t.User : null!)
                .Include(e => e.Results)
                    .ThenInclude(r => r.Student)
                        .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(e => e.Id == examId);
        }

        public async Task<IEnumerable<Exam>> GetByTeacherIdAsync(int teacherId)
        {
            return await _dbSet
                .Where(e => e.TeacherId == teacherId)
                .Include(e => e.Subject)
                .Include(e => e.ClassRoom)
                .OrderBy(e => e.ExamDate)
                .ToListAsync();
        }
    }
}