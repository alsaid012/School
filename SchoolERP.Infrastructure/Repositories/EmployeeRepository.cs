using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
        }

        //public async Task<Employee?> GetByCodeAsync(string code)
        //{
        //    return await _dbSet
        //        .FirstOrDefaultAsync(e => e.EmployeeCode == code);
        //}

        public async Task<IEnumerable<Employee>> GetBySchoolIdAsync(int schoolId)
        {
            return await _dbSet
                .Where(e => e.User.SchoolId == schoolId)
                .Include(e => e.User)
                .OrderBy(e => e.User.FullName)
                .ToListAsync();
        }

        public override async Task<Employee?> GetWithDetailsAsync(int employeeId)
        {
            return await _dbSet
                .Include(e => e.User)
                    .ThenInclude(u => u.Contacts)
                .Include(e => e.Attendances)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
        }

        public async Task<IEnumerable<Employee>> GetByJobTitleAsync(string jobTitle)
        {
            return await _dbSet
                .Where(e => e.JobTitle == jobTitle)
                .Include(e => e.User)
                .OrderBy(e => e.User.FullName)
                .ToListAsync();
        }

        public async Task<bool> IsEmployeeCodeExistsAsync(string employeeCode)
        {
            return await _dbSet
                .AnyAsync(e => e.EmployeeCode == employeeCode);
        }
    }
}