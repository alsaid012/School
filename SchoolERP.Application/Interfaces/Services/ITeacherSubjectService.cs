using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.TeacherSubjects;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🔗  واجهة خدمة ربط المعلم بالمادة (ITeacherSubjectService)
    /// 📌  الوظيفة: تعريف عمليات إدارة ربط المعلم بالمادة
    /// 📦  الاستخدام: في TeacherSubjectsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface ITeacherSubjectService
    {
        /// <summary>
        /// 📋 الحصول على جميع الروابط
        /// </summary>
        Task<ResponseDto<IEnumerable<TeacherSubjectDto>>> GetAllAsync();

        /// <summary>
        /// 🔍 الحصول على رابط بواسطة المعرف
        /// </summary>
        Task<ResponseDto<TeacherSubjectDto>> GetByIdAsync(int id);


        /// <summary>
        /// 📋 الحصول على روابط معلم معين
        /// </summary>
        Task<ResponseDto<IEnumerable<TeacherSubjectDto>>> GetByTeacherIdAsync(int teacherId);

        /// <summary>
        /// 📋 الحصول على روابط مادة معينة
        /// </summary>
        Task<ResponseDto<IEnumerable<TeacherSubjectDto>>> GetBySubjectIdAsync(int subjectId);

        /// <summary>
        /// 📋 الحصول على الروابط للقوائم المنسدلة
        /// </summary>
        Task<ResponseDto<IEnumerable<TeacherSubjectLookupDto>>> GetLookupAsync(int? teacherId = null);

    
        /// <summary>
        /// ➕ إنشاء رابط جديد
        /// </summary>
        Task<ResponseDto<TeacherSubjectDto>> CreateAsync(CreateTeacherSubjectDto createDto);

        /// <summary>
        /// ✏️ تحديث بيانات رابط
        /// </summary>
        Task<ResponseDto<TeacherSubjectDto>> UpdateAsync(int id, UpdateTeacherSubjectDto updateDto);

        /// <summary>
        /// 🗑️ حذف رابط
        /// </summary>
        Task<ResponseDto> DeleteAsync(int id);

        /// <summary>
        /// ✅ التحقق من وجود رابط مكرر
        /// </summary>
        Task<ResponseDto<bool>> IsExistsAsync(int teacherId, int subjectId);
    }
}