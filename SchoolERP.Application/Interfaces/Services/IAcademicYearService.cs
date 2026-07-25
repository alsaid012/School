using SchoolERP.Application.DTOs.AcademicYears;
using SchoolERP.Application.DTOs.Common;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📆  واجهة خدمة العام الدراسي (IAcademicYearService)
    /// 📌  الوظيفة: تعريف عمليات إدارة الأعوام الدراسية
    /// 📦  الاستخدام: في AcademicYearsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲    /// </summary>
    public interface IAcademicYearService
    {
        /// <summary>
        /// 📋 الحصول على جميع الأعوام الدراسية
        /// </summary>
        Task<ResponseDto<IEnumerable<AcademicYearDto>>> GetAllAsync();

        /// <summary>
        /// 📋 الحصول على الأعوام الدراسية لمدرسة معينة
        /// </summary>
        Task<ResponseDto<IEnumerable<AcademicYearDto>>> GetBySchoolIdAsync(int schoolId);

        /// <summary>
        /// 📋 الحصول على العام الدراسي الحالي
        /// </summary>
        Task<ResponseDto<AcademicYearDto>> GetCurrentYearAsync(int schoolId);

        /// <summary>
        /// 📋 الحصول على الأعوام الدراسية للقوائم المنسدلة
        /// </summary>
        Task<ResponseDto<IEnumerable<AcademicYearLookupDto>>> GetLookupAsync(int? schoolId = null);

        /// <summary>
        /// 🔍 الحصول على عام دراسي بواسطة المعرف
        /// </summary>
        Task<ResponseDto<AcademicYearDetailsDto>> GetByIdAsync(int id);

        /// <summary>
        /// 📊 الحصول على إحصائيات العام الدراسي
        /// </summary>
        Task<ResponseDto<AcademicYearStatisticsDto>> GetStatisticsAsync(int academicYearId);

        /// <summary>
        /// ➕ إنشاء عام دراسي جديد
        /// </summary>
        Task<ResponseDto<AcademicYearDto>> CreateAsync(CreateAcademicYearDto createDto);

        /// <summary>
        /// ✏️ تحديث بيانات عام دراسي
        /// </summary>
        Task<ResponseDto<AcademicYearDto>> UpdateAsync(int id, UpdateAcademicYearDto updateDto);

        /// <summary>
        /// 🗑️ حذف عام دراسي (Soft Delete)
        /// </summary>
        Task<ResponseDto> DeleteAsync(int id);

        /// <summary>
        /// 🔄 تعيين عام دراسي كعام حالي
        /// </summary>
        Task<ResponseDto> SetCurrentYearAsync(int id);

        /// <summary>
        /// ✅ التحقق من وجود عام دراسي بنفس الاسم
        /// </summary>
        Task<ResponseDto<bool>> IsNameExistsAsync(int schoolId, string name, int? excludeId = null);
    }
}