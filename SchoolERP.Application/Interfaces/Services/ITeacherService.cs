using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Teachers;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 👨‍🏫  واجهة خدمة المعلمين (ITeacherService)
    /// 📌  الوظيفة: تعريف عمليات إدارة المعلمين
    /// 📦  الاستخدام: في TeachersController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface ITeacherService
    {
        /// <summary>
        /// 📋 الحصول على جميع المعلمين
        /// </summary>
        Task<ResponseDto<IEnumerable<TeacherDto>>> GetAllAsync();

        /// <summary>
        /// 🔍 الحصول على معلم بواسطة المعرف
        /// </summary>
        Task<ResponseDto<TeacherDetailsDto>> GetByIdAsync(int id);

        /// <summary>
        /// 🔍 الحصول على معلم بواسطة الكود
        /// </summary>
        Task<ResponseDto<TeacherDto>> GetByCodeAsync(string teacherCode);

        /// <summary>
        /// 📋 الحصول على المعلمين في مدرسة معينة
        /// </summary>
        Task<ResponseDto<IEnumerable<TeacherDto>>> GetBySchoolIdAsync(int schoolId);

        /// <summary>
        /// 📋 الحصول على المعلمين حسب التخصص
        /// </summary>
        Task<ResponseDto<IEnumerable<TeacherDto>>> GetBySpecializationAsync(string specialization);

        /// <summary>
        /// 📋 الحصول على معلمي الفصل
        /// </summary>
        Task<ResponseDto<IEnumerable<TeacherDto>>> GetHomeroomTeachersAsync();

        /// <summary>
        /// 📋 الحصول على المعلمين الذين يدرسون مادة معينة
        /// </summary>
        Task<ResponseDto<IEnumerable<TeacherDto>>> GetBySubjectIdAsync(int subjectId);

        /// <summary>
        /// 📋 الحصول على المعلمين للقوائم المنسدلة
        /// </summary>
        Task<ResponseDto<IEnumerable<TeacherLookupDto>>> GetLookupAsync(int? schoolId = null);

    

        /// <summary>
        /// 📊 الحصول على إحصائيات المعلم
        /// </summary>
        Task<ResponseDto<TeacherStatisticsDto>> GetStatisticsAsync(int teacherId);

        /// <summary>
        /// ➕ إنشاء معلم جديد
        /// </summary>
        Task<ResponseDto<TeacherDto>> CreateAsync(CreateTeacherDto createDto);

        /// <summary>
        /// ✏️ تحديث بيانات معلم
        /// </summary>
        Task<ResponseDto<TeacherDto>> UpdateAsync(int id, UpdateTeacherDto updateDto);

        /// <summary>
        /// 🗑️ حذف معلم (Soft Delete)
        /// </summary>
        Task<ResponseDto> DeleteAsync(int id);

        /// <summary>
        /// ✅ التحقق من وجود كود معلم
        /// </summary>
        Task<ResponseDto<bool>> IsTeacherCodeExistsAsync(string teacherCode);
    }
}