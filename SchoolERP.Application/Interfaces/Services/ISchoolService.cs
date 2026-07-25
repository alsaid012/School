using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Schools;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏫  واجهة خدمة المدارس (ISchoolService)
    /// 📌  الوظيفة: تعريف عمليات إدارة المدارس
    /// 📦  الاستخدام: في SchoolsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface ISchoolService
    {
        /// <summary>
        /// 📋 الحصول على جميع المدارس
        /// </summary>
        Task<ResponseDto<IEnumerable<SchoolDto>>> GetAllAsync();

        /// <summary>
        /// 🔍 الحصول على مدرسة بواسطة المعرف
        /// </summary>
        Task<ResponseDto<SchoolDetailsDto>> GetByIdAsync(int id);

        /// <summary>
        /// 🔍 الحصول على مدرسة بواسطة الكود
        /// </summary>
        Task<ResponseDto<SchoolDto>> GetByCodeAsync(string code);

        /// <summary>
        /// 📋 الحصول على المدارس التابعة لإدارة معينة
        /// </summary>
        Task<ResponseDto<IEnumerable<SchoolDto>>> GetByDepartmentIdAsync(int departmentId);

        /// <summary>
        /// 📋 الحصول على المدارس للقوائم المنسدلة
        /// </summary>
        Task<ResponseDto<IEnumerable<SchoolLookupDto>>> GetLookupAsync(int? departmentId = null);

        /// <summary>
        /// 📊 الحصول على إحصائيات المدرسة
        /// </summary>
        Task<ResponseDto<SchoolStatisticsDto>> GetStatisticsAsync(int schoolId);

        /// <summary>
        /// ➕ إنشاء مدرسة جديدة
        /// </summary>
        Task<ResponseDto<SchoolDto>> CreateAsync(CreateSchoolDto createDto);

        /// <summary>
        /// ✏️ تحديث بيانات مدرسة
        /// </summary>
        Task<ResponseDto<SchoolDto>> UpdateAsync(int id, UpdateSchoolDto updateDto);

        /// <summary>
        /// 🗑️ حذف مدرسة (Soft Delete)
        /// </summary>
        Task<ResponseDto> DeleteAsync(int id);

        /// <summary>
        /// ✅ التحقق من وجود مدرسة بنفس الاسم
        /// </summary>
        Task<ResponseDto<bool>> IsNameExistsAsync(string name, int? excludeId = null);

        Task<ResponseDto<bool>> IsCodeExistsAsync(string code, int? excludeId = null);

    }
}