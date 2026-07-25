using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// 📖  واجهة مستودع المواد الدراسية (ISubjectRepository)
    /// </summary>
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        ///// <summary>
        ///// 🔍 البحث عن مادة بواسطة الكود
        ///// </summary>
        //Task<Subject?> GetByCodeAsync(string code);

        /// <summary>
        /// 📋 الحصول على جميع المواد في صف معين
        /// </summary>
        Task<IEnumerable<Subject>> GetByGradeLevelIdAsync(int gradeLevelId);

        ///// <summary>
        ///// 📋 الحصول على مادة مع جميع بياناتها
        ///// </summary>
        //Task<Subject?> GetWithDetailsAsync(int subjectId);

        /// <summary>
        /// 📋 الحصول على المواد التي يدرسها معلم معين
        /// </summary>
        Task<IEnumerable<Subject>> GetByTeacherIdAsync(int teacherId);

        /// <summary>
        /// ✅ التحقق من وجود مادة بنفس الاسم في الصف
        /// </summary>
        Task<bool> IsNameExistsAsync(int gradeLevelId, string name, int? excludeId = null);
    }
}