using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.EmployeeAttendances;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ✅  واجهة خدمة حضور الموظفين (IEmployeeAttendanceService)
    /// </summary>
    public interface IEmployeeAttendanceService
    {
        Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetAllAsync();
        Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetByEmployeeIdAsync(int employeeId);
        Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetByDepartmentAsync(string department);
        Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetByDateAsync(DateTime date);
        Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetFilteredAsync(EmployeeAttendanceFilterDto filter);
        Task<ResponseDto<EmployeeAttendanceDto>> GetByIdAsync(int id);
        Task<ResponseDto<EmployeeAttendanceDto>> GetByEmployeeAndDateAsync(int employeeId, DateTime date);
        Task<ResponseDto<EmployeeAttendanceStatisticsDto>> GetStatisticsAsync(int employeeId, DateTime fromDate, DateTime toDate);
        Task<ResponseDto<object>> GetDailyReportAsync(int schoolId, DateTime date);
        Task<ResponseDto<EmployeeAttendanceDto>> CreateAsync(CreateEmployeeAttendanceDto createDto);
        Task<ResponseDto<EmployeeAttendanceDto>> UpdateAsync(int id, UpdateEmployeeAttendanceDto updateDto);
        Task<ResponseDto> DeleteAsync(int id);
    }
}



//using SchoolERP.Application.DTOs.Common;
//using SchoolERP.Application.DTOs.EmployeeAttendances;

//namespace SchoolERP.Application.Interfaces.Services
//{
//    public interface IEmployeeAttendanceService
//    {
//        Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetAllAsync();
//        Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetByEmployeeIdAsync(int employeeId);
//        Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetBySchoolAndDateAsync(int schoolId, DateTime date);
//        Task<ResponseDto<EmployeeAttendanceDto>> GetByIdAsync(int id);
//        Task<ResponseDto<EmployeeAttendanceDto>> GetByEmployeeAndDateAsync(int employeeId, DateTime date);
//        Task<ResponseDto<EmployeeAttendanceStatisticsDto>> GetStatisticsAsync(int employeeId, DateTime fromDate, DateTime toDate);
//        Task<ResponseDto<EmployeeAttendanceDto>> CreateAsync(CreateEmployeeAttendanceDto createDto);
//        Task<ResponseDto<EmployeeAttendanceDto>> UpdateAsync(int id, UpdateEmployeeAttendanceDto updateDto);
//        Task<ResponseDto> DeleteAsync(int id);
//    }
//}