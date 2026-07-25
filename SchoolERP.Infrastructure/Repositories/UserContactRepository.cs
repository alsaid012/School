using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    public class UserContactRepository : GenericRepository<UserContact>, IUserContactRepository
    {
        public UserContactRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserContact>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(uc => uc.UserId == userId)
                .OrderByDescending(uc => uc.IsPrimary)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserContact>> GetByTypeAsync(ContactType type)
        {
            return await _dbSet
                .Where(uc => uc.ContactType == type)
                .Include(uc => uc.User)
                .ToListAsync();
        }

        public async Task<UserContact?> GetPrimaryContactAsync(int userId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.IsPrimary);
        }

        public async Task<bool> IsValueExistsAsync(string value, int? excludeId = null)
        {
            var query = _dbSet.Where(uc => uc.ContactValue == value);
            if (excludeId.HasValue)
                query = query.Where(uc => uc.Id != excludeId.Value);
            return await query.AnyAsync();
        }
    }
}