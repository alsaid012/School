using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    public class SchoolRepository : GenericRepository<School>, ISchoolRepository
    {
        public SchoolRepository(ApplicationDbContext context) : base(context)
        {
        }

        //public async Task<School?> GetByCodeAsync(string code)
        //{
        //    return await _dbSet
        //        .FirstOrDefaultAsync(s => s.SchoolCode == code);
        //}

        public async Task<IEnumerable<School>> GetByDepartmentIdAsync(int departmentId)
        {
            return await _dbSet
                .Where(s => s.DepartmentId == departmentId)
                .OrderBy(s => s.SchoolName)
                .ToListAsync();
        }

        public override async Task<School?> GetWithDetailsAsync(int schoolId)
        {
            return await _dbSet
                .Include(s => s.Department)
                    .ThenInclude(d => d.Governorate)
                .Include(s => s.Users)
                .Include(s => s.GradeLevels)
                    .ThenInclude(g => g.ClassRooms)
                .Include(s => s.AcademicYears)
                .FirstOrDefaultAsync(s => s.Id == schoolId);
        }

        public async Task<object> GetStatisticsAsync(int schoolId)
        {
            var school = await GetWithDetailsAsync(schoolId);

            return new
            {
                اسم_المدرسة = school?.SchoolName,
                عدد_الطلاب = school?.Users.Count(u => u.UserType == UserType.Student) ?? 0,
                عدد_المعلمين = school?.Users.Count(u => u.UserType == UserType.Teacher) ?? 0,
                عدد_الموظفين = school?.Users.Count(u => u.UserType == UserType.Employee) ?? 0,
                عدد_الفصول = school?.GradeLevels.Sum(g => g.ClassRooms.Count) ?? 0,
                عدد_الصفوف = school?.GradeLevels.Count ?? 0
            };
        }

        //public async Task<bool> IsNameExistsAsync(string name, int? excludeId = null)
        //{
        //    var query = _dbSet.Where(s => s.SchoolName == name);
        //    if (excludeId.HasValue)
        //        query = query.Where(s => s.Id != excludeId.Value);
        //    return await query.AnyAsync();
        //}
    }
}