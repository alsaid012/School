using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// 🏢  واجهة مستودع الإدارات (IDepartmentRepository)
    /// </summary>
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        ///// <summary>
        ///// 🔍 البحث عن إدارة بواسطة الكود
        ///// </summary>
        //Task<Department?> GetByCodeAsync(string code);

        /// <summary>
        /// 📋 الحصول على جميع الإدارات التابعة لمحافظة معينة
        /// </summary>
        Task<IEnumerable<Department>> GetByGovernorateIdAsync(int governorateId);

        /// <summary>
        /// 📋 الحصول على إدارة مع جميع المدارس التابعة لها
        /// </summary>
        Task<Department?> GetWithSchoolsAsync(int departmentId);

        /// <summary>
        /// 📋 الحصول على جميع الإدارات مع المحافظات والمدارس
        /// </summary>
        Task<IEnumerable<Department>> GetAllWithDetailsAsync();

        ///// <summary>
        ///// ✅ التحقق من وجود إدارة بنفس الاسم
        ///// </summary>
        //Task<bool> IsNameExistsAsync(string name, int? excludeId = null);
    }
}