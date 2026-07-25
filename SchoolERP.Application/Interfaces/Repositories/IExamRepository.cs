using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// 📝  واجهة مستودع الامتحانات (IExamRepository)
    /// </summary>
    public interface IExamRepository : IGenericRepository<Exam>
    {
        /// <summary>
        /// 📋 الحصول على جميع الامتحانات في عام دراسي معين
        /// </summary>
        Task<IEnumerable<Exam>> GetByAcademicYearIdAsync(int academicYearId);

        /// <summary>
        /// 📋 الحصول على جميع الامتحانات لمادة معينة
        /// </summary>
        Task<IEnumerable<Exam>> GetBySubjectIdAsync(int subjectId);

        /// <summary>
        /// 📋 الحصول على جميع الامتحانات لفصل معين
        /// </summary>
        Task<IEnumerable<Exam>> GetByClassRoomIdAsync(int classRoomId);

        /// <summary>
        /// 📋 الحصول على الامتحانات القادمة
        /// </summary>
        Task<IEnumerable<Exam>> GetUpcomingExamsAsync(DateTime fromDate);

        ///// <summary>
        ///// 📋 الحصول على امتحان مع جميع بياناته
        ///// </summary>
        //Task<Exam?> GetWithDetailsAsync(int examId);

        /// <summary>
        /// 📋 الحصول على امتحانات معلم معين
        /// </summary>
        Task<IEnumerable<Exam>> GetByTeacherIdAsync(int teacherId);
    }
}