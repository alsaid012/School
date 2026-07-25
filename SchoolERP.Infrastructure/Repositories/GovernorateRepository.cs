using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    /// <summary>
    /// 📍  مستودع المحافظات (GovernorateRepository)
    /// 📌  الوظيفة: تنفيذ عمليات قاعدة البيانات الخاصة بالمحافظات
    /// </summary>
    public class GovernorateRepository : GenericRepository<Governorate>, IGovernorateRepository
    {
        public GovernorateRepository(ApplicationDbContext context) : base(context)
        {
        }

        //public async Task<Governorate?> GetByCodeAsync(string code)
        //{
        //    return await _dbSet
        //        .FirstOrDefaultAsync(g => g.Code == code);
        //}

        public async Task<IEnumerable<Governorate>> GetAllWithDepartmentsAsync()
        {
            return await _dbSet
                .Include(g => g.Departments)
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        //public async Task<bool> IsNameExistsAsync(string name, int? excludeId = null)
        //{
        //    var query = _dbSet.Where(g => g.Name == name);
        //    if (excludeId.HasValue)
        //        query = query.Where(g => g.Id != excludeId.Value);
        //    return await query.AnyAsync();
        //}

        //public async Task<bool> IsCodeExistsAsync(string code, int? excludeId = null)
        //{
        //    var query = _dbSet.Where(g => g.Code == code);
        //    if (excludeId.HasValue)
        //        query = query.Where(g => g.Id != excludeId.Value);
        //    return await query.AnyAsync();
        //}
    }
}