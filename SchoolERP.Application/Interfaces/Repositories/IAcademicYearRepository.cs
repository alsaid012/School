using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// 📆  واجهة مستودع العام الدراسي (IAcademicYearRepository)
    /// </summary>
    public interface IAcademicYearRepository : IGenericRepository<AcademicYear>
    {
        /// <summary>
        /// 📋 الحصول على العام الدراسي الحالي لمدرسة معينة
        /// </summary>
        Task<AcademicYear?> GetCurrentYearAsync(int schoolId);

        /// <summary>
        /// 📋 الحصول على جميع الأعوام الدراسية لمدرسة معينة
        /// </summary>
        Task<IEnumerable<AcademicYear>> GetBySchoolIdAsync(int schoolId);

        ///// <summary>
        ///// 📋 الحصول على عام دراسي مع جميع بياناته
        ///// </summary>
        //Task<AcademicYear?> GetWithDetailsAsync(int academicYearId);

      
    }
}