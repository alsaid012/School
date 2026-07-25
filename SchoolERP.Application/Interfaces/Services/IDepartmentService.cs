using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Departments;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏢  واجهة خدمة الإدارات التعليمية (IDepartmentService)
    /// 📌  الوظيفة: تعريف عمليات إدارة الإدارات التعليمية
    /// 📦  الاستخدام: في DepartmentsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IDepartmentService
    {
        /// <summary>
        /// 📋 الحصول على جميع الإدارات
        /// </summary>
        Task<ResponseDto<IEnumerable<DepartmentDto>>> GetAllAsync();

        /// <summary>
        /// 📋 الحصول على جميع الإدارات مع التفاصيل
        /// </summary>
        Task<ResponseDto<IEnumerable<DepartmentDetailsDto>>> GetAllWithDetailsAsync();

        /// <summary>
        /// 📋 الحصول على الإدارات التابعة لمحافظة معينة
        /// </summary>
        Task<ResponseDto<IEnumerable<DepartmentDto>>> GetByGovernorateIdAsync(int governorateId);

        /// <summary>
        /// 📋 الحصول على الإدارات للقوائم المنسدلة
        /// </summary>
        Task<ResponseDto<IEnumerable<DepartmentLookupDto>>> GetLookupAsync(int? governorateId = null);

        /// <summary>
        /// 🔍 الحصول على إدارة بواسطة المعرف
        /// </summary>
        Task<ResponseDto<DepartmentDetailsDto>> GetByIdAsync(int id);

        /// <summary>
        /// 🔍 الحصول على إدارة بواسطة الكود
        /// </summary>
        Task<ResponseDto<DepartmentDto>> GetByCodeAsync(string code);

        /// <summary>
        /// ➕ إنشاء إدارة جديدة
        /// </summary>
        Task<ResponseDto<DepartmentDto>> CreateAsync(CreateDepartmentDto createDto);

        /// <summary>
        /// ✏️ تحديث بيانات إدارة
        /// </summary>
        Task<ResponseDto<DepartmentDto>> UpdateAsync(int id, UpdateDepartmentDto updateDto);

        /// <summary>
        /// 🗑️ حذف إدارة (Soft Delete)
        /// </summary>
        Task<ResponseDto> DeleteAsync(int id);

        /// <summary>
        /// ✅ التحقق من وجود إدارة بنفس الاسم
        /// </summary>
        Task<ResponseDto<bool>> IsNameExistsAsync(string name, int? excludeId = null);

        Task<ResponseDto<bool>> IsCodeExistsAsync(string code, int? excludeId = null);

    }
}