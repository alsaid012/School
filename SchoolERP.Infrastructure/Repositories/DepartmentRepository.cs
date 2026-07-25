using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Department?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(d => d.Governorate)
                .Include(d => d.Schools)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        //public async Task<Department?> GetByCodeAsync(string code)
        //{
        //    return await _dbSet
        //        .FirstOrDefaultAsync(d => d.Code == code);
        //}

        public async Task<IEnumerable<Department>> GetByGovernorateIdAsync(int governorateId)
        {
            return await _dbSet
                .Where(d => d.GovernorateId == governorateId)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<Department?> GetWithSchoolsAsync(int departmentId)
        {
            return await _dbSet
                .Include(d => d.Schools)
                .FirstOrDefaultAsync(d => d.Id == departmentId);
        }

        public async Task<IEnumerable<Department>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(d => d.Governorate)
                .Include(d => d.Schools)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        //public async Task<bool> IsNameExistsAsync(string name, int? excludeId = null)
        //{
        //    var query = _dbSet.Where(d => d.Name == name);
        //    if (excludeId.HasValue)
        //        query = query.Where(d => d.Id != excludeId.Value);
        //    return await query.AnyAsync();
        //}
    }
}