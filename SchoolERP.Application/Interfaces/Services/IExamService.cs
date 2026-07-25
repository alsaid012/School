using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Exams;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📝  واجهة خدمة الامتحانات (IExamService)
    /// 📌  الوظيفة: تعريف عمليات إدارة الامتحانات
    /// 📦  الاستخدام: في ExamsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IExamService
    {
        /// <summary>
        /// 📋 الحصول على جميع الامتحانات
        /// </summary>
        Task<ResponseDto<IEnumerable<ExamDto>>> GetAllAsync();

        /// <summary>
        /// 📋 الحصول على امتحانات عام دراسي معين
        /// </summary>
        Task<ResponseDto<IEnumerable<ExamDto>>> GetByAcademicYearIdAsync(int academicYearId);

        /// <summary>
        /// 📋 الحصول على امتحانات مادة معينة
        /// </summary>
        Task<ResponseDto<IEnumerable<ExamDto>>> GetBySubjectIdAsync(int subjectId);

        /// <summary>
        /// 📋 الحصول على امتحانات فصل معين
        /// </summary>
        Task<ResponseDto<IEnumerable<ExamDto>>> GetByClassRoomIdAsync(int classRoomId);

        /// <summary>
        /// 📋 الحصول على امتحانات معلم معين
        /// </summary>
        Task<ResponseDto<IEnumerable<ExamDto>>> GetByTeacherIdAsync(int teacherId);

        /// <summary>
        /// 📋 الحصول على الامتحانات القادمة
        /// </summary>
        Task<ResponseDto<IEnumerable<ExamDto>>> GetUpcomingExamsAsync(DateTime fromDate);

        /// <summary>
        /// 📋 الحصول على الامتحانات للقوائم المنسدلة
        /// </summary>
        Task<ResponseDto<IEnumerable<ExamLookupDto>>> GetLookupAsync(int? academicYearId = null);

        /// <summary>
        /// 🔍 الحصول على امتحان بواسطة المعرف
        /// </summary>
        Task<ResponseDto<ExamDetailsDto>> GetByIdAsync(int id);

        /// <summary>
        /// 📊 الحصول على إحصائيات الامتحان
        /// </summary>
        Task<ResponseDto<ExamStatisticsDto>> GetStatisticsAsync(int examId);

        /// <summary>
        /// ➕ إنشاء امتحان جديد
        /// </summary>
        Task<ResponseDto<ExamDto>> CreateAsync(CreateExamDto createDto);

        /// <summary>
        /// ✏️ تحديث بيانات امتحان
        /// </summary>
        Task<ResponseDto<ExamDto>> UpdateAsync(int id, UpdateExamDto updateDto);

        /// <summary>
        /// 🗑️ حذف امتحان
        /// </summary>
        Task<ResponseDto> DeleteAsync(int id);
    }
}