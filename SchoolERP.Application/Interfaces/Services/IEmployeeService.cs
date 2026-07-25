using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Employees;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 👨‍💼  واجهة خدمة الموظفين (IEmployeeService)
    /// 📌  الوظيفة: تعريف عمليات إدارة الموظفين
    /// 📦  الاستخدام: في EmployeesController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// 📋 الحصول على جميع الموظفين
        /// </summary>
        Task<ResponseDto<IEnumerable<EmployeeDto>>> GetAllAsync();

        /// <summary>
        /// 🔍 الحصول على موظف بواسطة المعرف
        /// </summary>
        Task<ResponseDto<EmployeeDetailsDto>> GetByIdAsync(int id);

        /// <summary>
        /// 🔍 الحصول على موظف بواسطة الكود
        /// </summary>
        Task<ResponseDto<EmployeeDto>> GetByCodeAsync(string employeeCode);

        /// <summary>
        /// 📋 الحصول على الموظفين في مدرسة معينة
        /// </summary>
        Task<ResponseDto<IEnumerable<EmployeeDto>>> GetBySchoolIdAsync(int schoolId);

        /// <summary>
        /// 📋 الحصول على الموظفين حسب المسمى الوظيفي
        /// </summary>
        Task<ResponseDto<IEnumerable<EmployeeDto>>> GetByJobTitleAsync(string jobTitle);

        /// <summary>
        /// 📋 الحصول على الموظفين للقوائم المنسدلة
        /// </summary>
        Task<ResponseDto<IEnumerable<EmployeeLookupDto>>> GetLookupAsync(int? schoolId = null);

   
        /// <summary>
        /// 📊 الحصول على إحصائيات الموظف
        /// </summary>
        Task<ResponseDto<EmployeeStatisticsDto>> GetStatisticsAsync(int employeeId);

        /// <summary>
        /// ➕ إنشاء موظف جديد
        /// </summary>
        Task<ResponseDto<EmployeeDto>> CreateAsync(CreateEmployeeDto createDto);

        /// <summary>
        /// ✏️ تحديث بيانات موظف
        /// </summary>
        Task<ResponseDto<EmployeeDto>> UpdateAsync(int id, UpdateEmployeeDto updateDto);

        /// <summary>
        /// 🗑️ حذف موظف (Soft Delete)
        /// </summary>
        Task<ResponseDto> DeleteAsync(int id);

        /// <summary>
        /// ✅ التحقق من وجود كود موظف
        /// </summary>
        Task<ResponseDto<bool>> IsEmployeeCodeExistsAsync(string employeeCode);
    }
}