using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// 🏫  واجهة مستودع الفصول الدراسية (IClassRoomRepository)
    /// </summary>
    public interface IClassRoomRepository : IGenericRepository<ClassRoom>
    {
        /// <summary>
        /// 📋 الحصول على جميع الفصول في صف معين
        /// </summary>
        Task<IEnumerable<ClassRoom>> GetByGradeLevelIdAsync(int gradeLevelId);

        /// <summary>
        /// 📋 الحصول على الفصول التي يشرف عليها معلم معين
        /// </summary>
        Task<IEnumerable<ClassRoom>> GetByTeacherIdAsync(int teacherId);

        ///// <summary>
        ///// 📋 الحصول على فصل مع جميع بياناته
        ///// </summary>
        //Task<ClassRoom?> GetWithDetailsAsync(int classRoomId);

        /// <summary>
        /// 📋 الحصول على جميع الفصول في مدرسة معينة
        /// </summary>
        Task<IEnumerable<ClassRoom>> GetBySchoolIdAsync(int schoolId);

        /// <summary>
        /// ✅ التحقق من وجود فصل بنفس الاسم في الصف
        /// </summary>
        Task<bool> IsNameExistsAsync(int gradeLevelId, string name, int? excludeId = null);
    }
}