using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    /// <summary>
    /// 📊  مستودع نتائج الامتحانات (ExamResultRepository)
    /// 📌  الوظيفة: تنفيذ عمليات قاعدة البيانات الخاصة بنتائج الامتحانات
    /// </summary>
    public class ExamResultRepository : GenericRepository<ExamResult>, IExamResultRepository
    {
        public ExamResultRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 📋 الحصول على جميع نتائج امتحان معين
        /// </summary>
        public async Task<IEnumerable<ExamResult>> GetByExamIdAsync(int examId)
        {
            return await _dbSet
                .Where(er => er.ExamId == examId)
                .Include(er => er.Student)
                    .ThenInclude(s => s.User)
                .OrderByDescending(er => er.Score)
                .ToListAsync();
        }

        /// <summary>
        /// 📋 الحصول على جميع نتائج طالب معين
        /// </summary>
        public async Task<IEnumerable<ExamResult>> GetByStudentIdAsync(int studentId)
        {
            return await _dbSet
                .Where(er => er.StudentId == studentId)
                .Include(er => er.Exam)
                    .ThenInclude(e => e.Subject)
                .Include(er => er.Exam)
                    .ThenInclude(e => e.AcademicYear)
                .OrderByDescending(er => er.Exam.ExamDate)
                .ToListAsync();
        }

        /// <summary>
        /// 📋 الحصول على نتيجة طالب في امتحان معين
        /// </summary>
        public async Task<ExamResult?> GetByExamAndStudentAsync(int examId, int studentId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(er => er.ExamId == examId && er.StudentId == studentId);
        }

        /// <summary>
        /// 📋 الحصول على نتائج طالب في عام دراسي معين
        /// </summary>
        public async Task<IEnumerable<ExamResult>> GetByStudentAndAcademicYearAsync(int studentId, int academicYearId)
        {
            return await _dbSet
                .Where(er => er.StudentId == studentId && er.Exam.AcademicYearId == academicYearId)
                .Include(er => er.Exam)
                    .ThenInclude(e => e.Subject)
                .OrderBy(er => er.Exam.ExamDate)
                .ToListAsync();
        }

        /// <summary>
        /// 📋 الحصول على نتائج فصل معين في امتحان معين
        /// </summary>
        public async Task<IEnumerable<ExamResult>> GetByClassRoomAndExamAsync(int classRoomId, int examId)
        {
            return await _dbSet
                .Where(er => er.ExamId == examId && er.Student.ClassRoomId == classRoomId)
                .Include(er => er.Student)
                    .ThenInclude(s => s.User)
                .OrderByDescending(er => er.Score)
                .ToListAsync();
        }

        /// <summary>
        /// 📊 الحصول على ترتيب الطلاب في امتحان معين
        /// </summary>
        public async Task<IEnumerable<ExamResult>> GetRankedResultsAsync(int examId)
        {
            return await _dbSet
                .Where(er => er.ExamId == examId)
                .Include(er => er.Student)
                    .ThenInclude(s => s.User)
                .OrderByDescending(er => er.Score)
                .ToListAsync();
        }

        /// <summary>
        /// 📊 الحصول على إحصائيات امتحان معين
        /// </summary>
        public async Task<object> GetExamStatisticsAsync(int examId)
        {
            var results = await _dbSet
                .Where(er => er.ExamId == examId)
                .ToListAsync();

            if (!results.Any())
                return new
                {
                    عدد_الطلاب = 0,
                    أعلى_درجة = 0,
                    أدنى_درجة = 0,
                    المتوسط = 0,
                    نسبة_النجاح = 0
                };

            var maxScore = results.Max(r => r.Score);
            var minScore = results.Min(r => r.Score);
            var average = results.Average(r => r.Score);
            var passingScore = results.FirstOrDefault()?.Exam.MaxScore * 0.5m ?? 0;
            var passed = results.Count(r => r.Score >= passingScore);

            return new
            {
                عدد_الطلاب = results.Count,
                أعلى_درجة = maxScore,
                أدنى_درجة = minScore,
                المتوسط = average,
                نسبة_النجاح = results.Count > 0 ? (double)passed / results.Count * 100 : 0,
                عدد_الناجحين = passed,
                عدد_الراسبين = results.Count - passed
            };
        }

        /// <summary>
        /// 📋 الحصول على نتيجة مع جميع البيانات المرتبطة
        /// </summary>
        public override async Task<ExamResult?> GetWithDetailsAsync(int examResultId)
        {
            return await _dbSet
                .Include(er => er.Exam)
                    .ThenInclude(e => e.Subject)
                .Include(er => er.Exam)
                    .ThenInclude(e => e.AcademicYear)
                .Include(er => er.Student)
                    .ThenInclude(s => s.User)
                .Include(er => er.Student)
                    .ThenInclude(s => s.ClassRoom)
                .FirstOrDefaultAsync(er => er.Id == examResultId);
        }

        /// <summary>
        /// ✅ التحقق من وجود نتيجة مكررة
        /// </summary>
        public async Task<bool> IsExistsAsync(int examId, int studentId, int? excludeId = null)
        {
            var query = _dbSet.Where(er => er.ExamId == examId && er.StudentId == studentId);
            if (excludeId.HasValue)
                query = query.Where(er => er.Id != excludeId.Value);
            return await query.AnyAsync();
        }

        /// <summary>
        /// 📊 الحصول على متوسط درجات طالب في مواد معينة
        /// </summary>
        public async Task<object> GetStudentAverageAsync(int studentId, int academicYearId)
        {
            var results = await _dbSet
                .Where(er => er.StudentId == studentId && er.Exam.AcademicYearId == academicYearId)
                .Include(er => er.Exam)
                    .ThenInclude(e => e.Subject)
                .ToListAsync();

            if (!results.Any())
                return new
                {
                    الطالب = studentId,
                    العام_الدراسي = academicYearId,
                    عدد_المواد = 0,
                    المتوسط_الكلي = 0,
                    تفاصيل_المواد = new List<object>()
                };

            var bySubject = results
                .GroupBy(er => er.Exam.SubjectId)
                .Select(g => new
                {
                    المادة = g.First().Exam.Subject.SubjectName,
                    عدد_الامتحانات = g.Count(),
                    المتوسط = g.Average(r => r.Score),
                    أعلى_درجة = g.Max(r => r.Score),
                    أدنى_درجة = g.Min(r => r.Score)
                })
                .ToList();

            return new
            {
                الطالب = studentId,
                العام_الدراسي = academicYearId,
                عدد_المواد = bySubject.Count,
                المتوسط_الكلي = results.Average(r => r.Score),
                تفاصيل_المواد = bySubject
            };
        }
    }
}