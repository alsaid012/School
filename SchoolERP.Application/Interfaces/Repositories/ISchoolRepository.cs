using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// 🏫  واجهة مستودع المدارس (ISchoolRepository)
    /// </summary>
    public interface ISchoolRepository : IGenericRepository<School>
    {
        ///// <summary>
        ///// 🔍 البحث عن مدرسة بواسطة الكود
        ///// </summary>
        //Task<School?> GetByCodeAsync(string code);

        /// <summary>
        /// 📋 الحصول على جميع المدارس التابعة لإدارة معينة
        /// </summary>
        Task<IEnumerable<School>> GetByDepartmentIdAsync(int departmentId);

        ///// <summary>
        ///// 📋 الحصول على مدرسة مع جميع بياناتها
        ///// </summary>
        //Task<School?> GetWithDetailsAsync(int schoolId);

        /// <summary>
        /// 📊 الحصول على إحصائيات المدرسة
        /// </summary>
        Task<object> GetStatisticsAsync(int schoolId);

        ///// <summary>
        ///// ✅ التحقق من وجود مدرسة بنفس الاسم
        ///// </summary>
        //Task<bool> IsNameExistsAsync(string name, int? excludeId = null);
    }
}