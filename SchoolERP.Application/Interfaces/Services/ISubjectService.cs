using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Subjects;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📖  واجهة خدمة المواد الدراسية (ISubjectService)
    /// 📌  الوظيفة: تعريف عمليات إدارة المواد الدراسية
    /// 📦  الاستخدام: في SubjectsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface ISubjectService
    {
        /// <summary>
        /// 📋 الحصول على جميع المواد الدراسية
        /// </summary>
        Task<ResponseDto<IEnumerable<SubjectDto>>> GetAllAsync();

        /// <summary>
        /// 🔍 الحصول على مادة بواسطة المعرف
        /// </summary>
        Task<ResponseDto<SubjectDetailsDto>> GetByIdAsync(int id);

        /// <summary>
        /// 🔍 الحصول على مادة بواسطة الكود
        /// </summary>
        Task<ResponseDto<SubjectDto>> GetByCodeAsync(string code);

        /// <summary>
        /// 📋 الحصول على المواد التابعة لصف معين
        /// </summary>
        Task<ResponseDto<IEnumerable<SubjectDto>>> GetByGradeLevelIdAsync(int gradeLevelId);

        /// <summary>
        /// 📋 الحصول على المواد التي يدرسها معلم معين
        /// </summary>
        Task<ResponseDto<IEnumerable<SubjectDto>>> GetByTeacherIdAsync(int teacherId);

        /// <summary>
        /// 📋 الحصول على المواد للقوائم المنسدلة
        /// </summary>
        Task<ResponseDto<IEnumerable<SubjectLookupDto>>> GetLookupAsync(int? gradeLevelId = null);

        /// <summary>
        /// 📊 الحصول على إحصائيات المادة
        /// </summary>
        Task<ResponseDto<SubjectStatisticsDto>> GetStatisticsAsync(int subjectId);

        /// <summary>
        /// ➕ إنشاء مادة جديدة
        /// </summary>
        Task<ResponseDto<SubjectDto>> CreateAsync(CreateSubjectDto createDto);

        /// <summary>
        /// ✏️ تحديث بيانات مادة
        /// </summary>
        Task<ResponseDto<SubjectDto>> UpdateAsync(int id, UpdateSubjectDto updateDto);

        /// <summary>
        /// 🗑️ حذف مادة (Soft Delete)
        /// </summary>
        Task<ResponseDto> DeleteAsync(int id);

        /// <summary>
        /// ✅ التحقق من وجود مادة بنفس الاسم في الصف
        /// </summary>
        Task<ResponseDto<bool>> IsNameExistsAsync(int gradeLevelId, string name, int? excludeId = null);

        /// <summary>
        /// 📋 الحصول على المواد مع Pagination
        /// </summary>
        Task<ResponseDto<PagedResultDto<SubjectDto>>> GetPagedAsync(PaginationDto pagination);
        /// <summary>
        /// 🔄 تفعيل / إلغاء تفعيل المادة
        /// </summary>
        Task<ResponseDto> ToggleActiveAsync(int id);
    }
}