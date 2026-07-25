using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Enums;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserRole>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(ur => ur.UserId == userId)
                .OrderByDescending(ur => ur.IsPrimary)
                .ToListAsync();
        }

        public async Task<UserRole?> GetPrimaryRoleAsync(int userId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.IsPrimary);
        }

        public async Task<IEnumerable<UserRole>> GetByRoleTypeAsync(UserType roleType)
        {
            return await _dbSet
                .Where(ur => ur.RoleType == roleType)
                .Include(ur => ur.User)
                .OrderBy(ur => ur.User.FullName)
                .ToListAsync();
        }

        public async Task<bool> IsExistsAsync(int userId, UserType roleType)
        {
            return await _dbSet
                .AnyAsync(ur => ur.UserId == userId && ur.RoleType == roleType);
        }
    }
}