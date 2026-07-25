using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// 🔗  واجهة مستودع ربط المعلم بالمواد (ITeacherSubjectRepository)
    /// </summary>
    public interface ITeacherSubjectRepository : IGenericRepository<TeacherSubject>
    {
        /// <summary>
        /// 📋 الحصول على جميع المواد التي يدرسها معلم معين
        /// </summary>
        Task<IEnumerable<TeacherSubject>> GetByTeacherIdAsync(int teacherId);

        /// <summary>
        /// 📋 الحصول على جميع المعلمين الذين يدرسون مادة معينة
        /// </summary>
        Task<IEnumerable<TeacherSubject>> GetBySubjectIdAsync(int subjectId);

        ///// <summary>
        ///// 📋 الحصول على الربط مع البيانات
        ///// </summary>
        //Task<TeacherSubject?> GetWithDetailsAsync(int teacherSubjectId);

        /// <summary>
        /// ✅ التحقق من وجود ربط مكرر
        /// </summary>
        Task<bool> IsExistsAsync(int teacherId, int subjectId);
    }
}