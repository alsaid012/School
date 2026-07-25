using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    public class StudentAttendanceRepository : GenericRepository<StudentAttendance>, IStudentAttendanceRepository
    {
        public StudentAttendanceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<StudentAttendance?> GetByStudentAndDateAsync(int studentId, DateTime date)
        {
            return await _dbSet
                .FirstOrDefaultAsync(sa => sa.StudentId == studentId && sa.AttendanceDate.Date == date.Date);
        }

        public async Task<IEnumerable<StudentAttendance>> GetByStudentIdAsync(int studentId)
        {
            return await _dbSet
                .Where(sa => sa.StudentId == studentId)
                .OrderByDescending(sa => sa.AttendanceDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentAttendance>> GetByClassRoomAndDateAsync(int classRoomId, DateTime date)
        {
            return await _dbSet
                .Where(sa => sa.Student.ClassRoomId == classRoomId && sa.AttendanceDate.Date == date.Date)
                .Include(sa => sa.Student)
                    .ThenInclude(s => s.User)
                .OrderBy(sa => sa.Student.User.FullName)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentAttendance>> GetByGradeLevelAndDateAsync(int gradeLevelId, DateTime date)
        {
            return await _dbSet
                .Where(sa => sa.Student != null && sa.Student.ClassRoom != null && sa.Student.ClassRoom.GradeLevelId == gradeLevelId && sa.AttendanceDate.Date == date.Date)
                .Include(sa => sa.Student)
                    .ThenInclude(s => s.User)
                .OrderBy(sa => sa.Student.User.FullName)
                .ToListAsync();
        }

        public async Task<object> GetStatisticsAsync(int studentId, DateTime fromDate, DateTime toDate)
        {
            var attendances = await _dbSet
                .Where(sa => sa.StudentId == studentId && sa.AttendanceDate >= fromDate && sa.AttendanceDate <= toDate)
                .ToListAsync();

            var totalDays = attendances.Count;
            var present = attendances.Count(sa => sa.Status == AttendanceStatus.Present);
            var absent = attendances.Count(sa => sa.Status == AttendanceStatus.Absent);
            var late = attendances.Count(sa => sa.Status == AttendanceStatus.Late);
            var excused = attendances.Count(sa => sa.Status == AttendanceStatus.Excused);

            return new
            {
                الطالب = studentId,
                من_تاريخ = fromDate,
                إلى_تاريخ = toDate,
                إجمالي_الأيام = totalDays,
                حاضر = present,
                غائب = absent,
                متأخر = late,
                معذور = excused,
                نسبة_الحضور = totalDays > 0 ? (double)present / totalDays * 100 : 0
            };
        }

        public async Task<object> GetDailyReportAsync(int schoolId, DateTime date)
        {
            var attendances = await _dbSet
                .Where(sa => sa.Student.User.SchoolId == schoolId && sa.AttendanceDate.Date == date.Date)
                .Include(sa => sa.Student)
                    .ThenInclude(s => s.User)
                .Include(sa => sa.Student)
                    .ThenInclude(s => s.ClassRoom)
                .ToListAsync();

            var total = attendances.Count;
            var present = attendances.Count(sa => sa.Status == AttendanceStatus.Present);
            var absent = attendances.Count(sa => sa.Status == AttendanceStatus.Absent);
            var late = attendances.Count(sa => sa.Status == AttendanceStatus.Late);
            var excused = attendances.Count(sa => sa.Status == AttendanceStatus.Excused);

            var byClass = attendances
                .GroupBy(sa => sa.Student.ClassRoom?.ClassName ?? "بدون فصل")
                .Select(g => new
                {
                    الفصل = g.Key,
                    إجمالي = g.Count(),
                    حاضر = g.Count(sa => sa.Status == AttendanceStatus.Present),
                    غائب = g.Count(sa => sa.Status == AttendanceStatus.Absent),
                    متأخر = g.Count(sa => sa.Status == AttendanceStatus.Late)
                })
                .ToList();

            return new
            {
                التاريخ = date,
                إجمالي_الطلاب = total,
                حاضر = present,
                غائب = absent,
                متأخر = late,
                معذور = excused,
                نسبة_الحضور = total > 0 ? (double)present / total * 100 : 0,
                تفاصيل_حسب_الفصل = byClass
            };
        }
    }
}