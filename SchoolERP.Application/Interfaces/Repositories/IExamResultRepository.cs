using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  واجهة مستودع نتائج الامتحانات (IExamResultRepository)
    /// 📌  الوظيفة: تعريف العمليات الخاصة بنتائج الامتحانات
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IExamResultRepository : IGenericRepository<ExamResult>
    {
        /// <summary>
        /// 📋 الحصول على جميع نتائج امتحان معين
        /// </summary>
        Task<IEnumerable<ExamResult>> GetByExamIdAsync(int examId);

        /// <summary>
        /// 📋 الحصول على جميع نتائج طالب معين
        /// </summary>
        Task<IEnumerable<ExamResult>> GetByStudentIdAsync(int studentId);

        /// <summary>
        /// 📋 الحصول على نتيجة طالب في امتحان معين
        /// </summary>
        Task<ExamResult?> GetByExamAndStudentAsync(int examId, int studentId);

        /// <summary>
        /// 📋 الحصول على نتائج طالب في عام دراسي معين
        /// </summary>
        Task<IEnumerable<ExamResult>> GetByStudentAndAcademicYearAsync(int studentId, int academicYearId);

        /// <summary>
        /// 📋 الحصول على نتائج فصل معين في امتحان معين
        /// </summary>
        Task<IEnumerable<ExamResult>> GetByClassRoomAndExamAsync(int classRoomId, int examId);

        /// <summary>
        /// 📊 الحصول على ترتيب الطلاب في امتحان معين
        /// </summary>
        Task<IEnumerable<ExamResult>> GetRankedResultsAsync(int examId);

        /// <summary>
        /// 📊 الحصول على إحصائيات امتحان معين
        /// </summary>
        Task<object> GetExamStatisticsAsync(int examId);

        ///// <summary>
        ///// 📋 الحصول على نتيجة مع جميع البيانات المرتبطة
        ///// </summary>
        //Task<ExamResult?> GetWithDetailsAsync(int examResultId);

        /// <summary>
        /// ✅ التحقق من وجود نتيجة مكررة
        /// </summary>
        Task<bool> IsExistsAsync(int examId, int studentId, int? excludeId = null);

        /// <summary>
        /// 📊 الحصول على متوسط درجات طالب في مواد معينة
        /// </summary>
        Task<object> GetStudentAverageAsync(int studentId, int academicYearId);
    }
}