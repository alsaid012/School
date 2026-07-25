using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// 📚  واجهة مستودع الصفوف الدراسية (IGradeLevelRepository)
    /// </summary>
    public interface IGradeLevelRepository : IGenericRepository<GradeLevel>
    {
        /// <summary>
        /// 📋 الحصول على جميع الصفوف في مدرسة معينة
        /// </summary>
        Task<IEnumerable<GradeLevel>> GetBySchoolIdAsync(int schoolId);

        /// <summary>
        /// 📋 الحصول على الصفوف حسب المرحلة (ابتدائي/اعدادي/ثانوي)
        /// </summary>
        Task<IEnumerable<GradeLevel>> GetByStageAsync(GradeStage stage);

        ///// <summary>
        ///// 📋 الحصول على صف مع جميع الفصول والمواد
        ///// </summary>
        //Task<GradeLevel?> GetWithDetailsAsync(int gradeLevelId);

        /// <summary>
        /// ✅ التحقق من وجود صف بنفس الاسم في المدرسة
        /// </summary>
        Task<bool> IsNameExistsAsync(int schoolId, string name, int? excludeId = null);
    }
}