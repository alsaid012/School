using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// 👨‍🏫  واجهة مستودع المعلمين (ITeacherRepository)
    /// </summary>
    public interface ITeacherRepository : IGenericRepository<Teacher>
    {
        ///// <summary>
        ///// 🔍 البحث عن معلم بواسطة الكود
        ///// </summary>
        //Task<Teacher?> GetByCodeAsync(string code);

        /// <summary>
        /// 📋 الحصول على جميع المعلمين في مدرسة معينة
        /// </summary>
        Task<IEnumerable<Teacher>> GetBySchoolIdAsync(int schoolId);

        ///// <summary>
        ///// 📋 الحصول على معلم مع جميع بياناته
        ///// </summary>
        //Task<Teacher?> GetWithDetailsAsync(int teacherId);

        /// <summary>
        /// 📋 الحصول على المعلمين حسب التخصص
        /// </summary>
        Task<IEnumerable<Teacher>> GetBySpecializationAsync(string specialization);

        /// <summary>
        /// 📋 الحصول على معلمي الفصل (Homeroom Teachers)
        /// </summary>
        Task<IEnumerable<Teacher>> GetHomeroomTeachersAsync();

        /// <summary>
        /// 📋 الحصول على المعلمين الذين يدرسون مادة معينة
        /// </summary>
        Task<IEnumerable<Teacher>> GetBySubjectIdAsync(int subjectId);

        Task<bool> IsTeacherCodeExistsAsync(string teacherCode);
    }
}