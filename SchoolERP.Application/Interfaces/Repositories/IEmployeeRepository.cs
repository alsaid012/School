using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// 👨‍💼  واجهة مستودع الموظفين (IEmployeeRepository)
    /// </summary>
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        ///// <summary>
        ///// 🔍 البحث عن موظف بواسطة الكود
        ///// </summary>
        //Task<Employee?> GetByCodeAsync(string code);

        /// <summary>
        /// 📋 الحصول على جميع الموظفين في مدرسة معينة
        /// </summary>
        Task<IEnumerable<Employee>> GetBySchoolIdAsync(int schoolId);

        ///// <summary>
        ///// 📋 الحصول على موظف مع جميع بياناته
        ///// </summary>
        //Task<Employee?> GetWithDetailsAsync(int employeeId);

        /// <summary>
        /// 📋 الحصول على الموظفين حسب المسمى الوظيفي
        /// </summary>
        Task<IEnumerable<Employee>> GetByJobTitleAsync(string jobTitle);

        Task<bool> IsEmployeeCodeExistsAsync(string employeeCode);
    }
}