using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.GradeLevels;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📚  واجهة خدمة الصفوف الدراسية (IGradeLevelService)
    /// 📌  الوظيفة: تعريف عمليات إدارة الصفوف الدراسية
    /// 📦  الاستخدام: في GradeLevelsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IGradeLevelService
    {
        /// <summary>
        /// 📋 الحصول على جميع الصفوف الدراسية
        /// </summary>
        Task<ResponseDto<IEnumerable<GradeLevelDto>>> GetAllAsync();

        /// <summary>
        /// 🔍 الحصول على صف بواسطة المعرف
        /// </summary>
        Task<ResponseDto<GradeLevelDto>> GetByIdAsync(int id);

        /// <summary>
        /// 📋 الحصول على الصفوف التابعة لمدرسة معينة
        /// </summary>
        Task<ResponseDto<IEnumerable<GradeLevelDto>>> GetBySchoolIdAsync(int schoolId);

        /// <summary>
        /// 📋 الحصول على الصفوف حسب المرحلة الدراسية
        /// </summary>
        Task<ResponseDto<IEnumerable<GradeLevelDto>>> GetByStageAsync(int stage);

        /// <summary>
        /// 📋 الحصول على الصفوف للقوائم المنسدلة
        /// </summary>
        Task<ResponseDto<IEnumerable<GradeLevelLookupDto>>> GetLookupAsync(int? schoolId = null);

      

        /// <summary>
        /// 📊 الحصول على إحصائيات الصف
        /// </summary>
        Task<ResponseDto<GradeLevelStatisticsDto>> GetStatisticsAsync(int gradeLevelId);

        /// <summary>
        /// ➕ إنشاء صف جديد
        /// </summary>
        Task<ResponseDto<GradeLevelDto>> CreateAsync(CreateGradeLevelDto createDto);

        /// <summary>
        /// ✏️ تحديث بيانات صف
        /// </summary>
        Task<ResponseDto<GradeLevelDto>> UpdateAsync(int id, UpdateGradeLevelDto updateDto);

        /// <summary>
        /// 🗑️ حذف صف (Soft Delete)
        /// </summary>
        Task<ResponseDto> DeleteAsync(int id);

        /// <summary>
        /// ✅ التحقق من وجود صف بنفس الاسم في المدرسة
        /// </summary>
        /// 
        Task<ResponseDto<bool>> IsNameExistsAsync(string name);

        Task<ResponseDto<bool>> IsNameExistsAsync(int schoolId, string name, int? excludeId = null);
    }
}