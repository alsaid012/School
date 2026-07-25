using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.StudentAttendances;

namespace SchoolERP.Application.Interfaces.Services
{
    public interface IStudentAttendanceService
    {
        Task<ResponseDto<IEnumerable<StudentAttendanceDto>>> GetAllAsync();
        Task<ResponseDto<IEnumerable<StudentAttendanceDto>>> GetByStudentIdAsync(int studentId);
        Task<ResponseDto<IEnumerable<StudentAttendanceDto>>> GetByClassRoomAndDateAsync(int classRoomId, DateTime date);
        Task<ResponseDto<IEnumerable<StudentAttendanceDto>>> GetByGradeLevelAndDateAsync(int gradeLevelId, DateTime date);
        Task<ResponseDto<StudentAttendanceDto>> GetByIdAsync(int id);
        Task<ResponseDto<StudentAttendanceDto>> GetByStudentAndDateAsync(int studentId, DateTime date);
        Task<ResponseDto<StudentAttendanceStatisticsDto>> GetStatisticsAsync(int studentId, DateTime fromDate, DateTime toDate);
        Task<ResponseDto<object>> GetDailyReportAsync(int schoolId, DateTime date);
        Task<ResponseDto<StudentAttendanceDto>> CreateAsync(CreateStudentAttendanceDto createDto);
        Task<ResponseDto<StudentAttendanceDto>> UpdateAsync(int id, UpdateStudentAttendanceDto updateDto);
        Task<ResponseDto> DeleteAsync(int id);
    }
}