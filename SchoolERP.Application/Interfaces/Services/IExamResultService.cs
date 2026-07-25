using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.ExamResults;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  واجهة خدمة نتائج الامتحانات (IExamResultService)
    /// 📌  الوظيفة: تعريف عمليات إدارة نتائج الامتحانات
    /// 📦  الاستخدام: في ExamResultsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IExamResultService
    {
        /// <summary>
        /// 📋 الحصول على جميع النتائج
        /// </summary>
        Task<ResponseDto<IEnumerable<ExamResultDto>>> GetAllAsync();

        /// <summary>
        /// 📋 الحصول على نتائج امتحان معين
        /// </summary>
        Task<ResponseDto<IEnumerable<ExamResultDto>>> GetByExamIdAsync(int examId);

        /// <summary>
        /// 📋 الحصول على نتائج طالب معين
        /// </summary>
        Task<ResponseDto<IEnumerable<ExamResultDto>>> GetByStudentIdAsync(int studentId);

        /// <summary>
        /// 📋 الحصول على نتائج طالب في عام دراسي معين
        /// </summary>
        Task<ResponseDto<IEnumerable<ExamResultDto>>> GetByStudentAndAcademicYearAsync(int studentId, int academicYearId);

        /// <summary>
        /// 📋 الحصول على نتائج فصل معين في امتحان معين
        /// </summary>
        Task<ResponseDto<IEnumerable<ExamResultDto>>> GetByClassRoomAndExamAsync(int classRoomId, int examId);

        /// <summary>
        /// 📋 الحصول على ترتيب الطلاب في امتحان معين
        /// </summary>
        Task<ResponseDto<IEnumerable<StudentRankDto>>> GetRankedResultsAsync(int examId);

        /// <summary>
        /// 📋 الحصول على النتائج للقوائم المنسدلة
        /// </summary>
        Task<ResponseDto<IEnumerable<ExamResultLookupDto>>> GetLookupAsync(int? examId = null);

        /// <summary>
        /// 🔍 الحصول على نتيجة بواسطة المعرف
        /// </summary>
        Task<ResponseDto<ExamResultDto>> GetByIdAsync(int id);

        /// <summary>
        /// 🔍 الحصول على نتيجة طالب في امتحان معين
        /// </summary>
        Task<ResponseDto<ExamResultDto>> GetByExamAndStudentAsync(int examId, int studentId);

        /// <summary>
        /// 📊 الحصول على إحصائيات النتائج
        /// </summary>
        Task<ResponseDto<ExamResultStatisticsDto>> GetStatisticsAsync(int examId);

        /// <summary>
        /// 📊 الحصول على متوسط درجات طالب
        /// </summary>
        Task<ResponseDto<object>> GetStudentAverageAsync(int studentId, int academicYearId);

        /// <summary>
        /// ➕ إضافة نتيجة جديدة
        /// </summary>
        Task<ResponseDto<ExamResultDto>> CreateAsync(CreateExamResultDto createDto);

        /// <summary>
        /// ➕➕ إضافة نتائج متعددة (دفعة واحدة)
        /// </summary>
        Task<ResponseDto<IEnumerable<ExamResultDto>>> CreateRangeAsync(IEnumerable<CreateExamResultDto> createDtos);

        /// <summary>
        /// ✏️ تحديث بيانات نتيجة
        /// </summary>
        Task<ResponseDto<ExamResultDto>> UpdateAsync(int id, UpdateExamResultDto updateDto);

        /// <summary>
        /// 🗑️ حذف نتيجة
        /// </summary>
        Task<ResponseDto> DeleteAsync(int id);

        /// <summary>
        /// ✅ التحقق من وجود نتيجة مكررة
        /// </summary>
        Task<ResponseDto<bool>> IsExistsAsync(int examId, int studentId, int? excludeId = null);
    }
}