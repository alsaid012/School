using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    public class EmployeeAttendanceRepository : GenericRepository<EmployeeAttendance>, IEmployeeAttendanceRepository
    {
        public EmployeeAttendanceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<EmployeeAttendance?> GetByEmployeeAndDateAsync(int employeeId, DateTime date)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ea => ea.EmployeeId == employeeId && ea.AttendanceDate.Date == date.Date);
        }

        public async Task<IEnumerable<EmployeeAttendance>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _dbSet
                .Where(ea => ea.EmployeeId == employeeId)
                .OrderByDescending(ea => ea.AttendanceDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeAttendance>> GetBySchoolAndDateAsync(int schoolId, DateTime date)
        {
            return await _dbSet
                .Where(ea => ea.Employee.User.SchoolId == schoolId && ea.AttendanceDate.Date == date.Date)
                .Include(ea => ea.Employee)
                    .ThenInclude(e => e.User)
                .OrderBy(ea => ea.Employee.User.FullName)
                .ToListAsync();
        }

        public async Task<object> GetStatisticsAsync(int employeeId, DateTime fromDate, DateTime toDate)
        {
            var attendances = await _dbSet
                .Where(ea => ea.EmployeeId == employeeId && ea.AttendanceDate >= fromDate && ea.AttendanceDate <= toDate)
                .ToListAsync();

            var totalDays = attendances.Count;
            var present = attendances.Count(ea => ea.Status == AttendanceStatus.Present);
            var absent = attendances.Count(ea => ea.Status == AttendanceStatus.Absent);
            var late = attendances.Count(ea => ea.Status == AttendanceStatus.Late);
            var excused = attendances.Count(ea => ea.Status == AttendanceStatus.Excused);

            return new
            {
                الموظف = employeeId,
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
    }
}