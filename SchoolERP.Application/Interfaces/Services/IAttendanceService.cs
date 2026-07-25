using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.EmployeeAttendances;
using SchoolERP.Application.DTOs.StudentAttendances;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✅  واجهة خدمة الحضور (IAttendanceService)
    /// 📌  الوظيفة: تعريف عمليات إدارة حضور الطلاب والموظفين
    /// 📦  الاستخدام: في AttendancesController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IAttendanceService
    {
        #region Student Attendance

        /// <summary>
        /// 📋 الحصول على جميع سجلات حضور الطلاب
        /// </summary>
        Task<ResponseDto<IEnumerable<StudentAttendanceDto>>> GetAllStudentAttendancesAsync();

        /// <summary>
        /// 📋 الحصول على سجلات حضور طالب معين
        /// </summary>
        Task<ResponseDto<IEnumerable<StudentAttendanceDto>>> GetStudentAttendancesByStudentIdAsync(int studentId);

        /// <summary>
        /// 📋 الحصول على سجلات حضور فصل معين في تاريخ محدد
        /// </summary>
        Task<ResponseDto<IEnumerable<StudentAttendanceDto>>> GetStudentAttendancesByClassRoomAndDateAsync(int classRoomId, DateTime date);

        /// <summary>
        /// 📋 الحصول على سجلات حضور صف معين في تاريخ محدد
        /// </summary>
        Task<ResponseDto<IEnumerable<StudentAttendanceDto>>> GetStudentAttendancesByGradeLevelAndDateAsync(int gradeLevelId, DateTime date);

        /// <summary>
        /// 📋 الحصول على تقرير الحضور اليومي لمدرسة معينة
        /// </summary>
        Task<ResponseDto<object>> GetDailyStudentAttendanceReportAsync(int schoolId, DateTime date);

        /// <summary>
        /// 📊 الحصول على إحصائيات حضور طالب معين
        /// </summary>
        Task<ResponseDto<StudentAttendanceStatisticsDto>> GetStudentAttendanceStatisticsAsync(int studentId, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// 🔍 الحصول على سجل حضور طالب بواسطة المعرف
        /// </summary>
        Task<ResponseDto<StudentAttendanceDto>> GetStudentAttendanceByIdAsync(int id);

        /// <summary>
        /// 🔍 الحصول على سجل حضور طالب في تاريخ محدد
        /// </summary>
        Task<ResponseDto<StudentAttendanceDto>> GetStudentAttendanceByStudentAndDateAsync(int studentId, DateTime date);

        /// <summary>
        /// ➕ إنشاء سجل حضور طالب جديد
        /// </summary>
        Task<ResponseDto<StudentAttendanceDto>> CreateStudentAttendanceAsync(CreateStudentAttendanceDto createDto);

        /// <summary>
        /// ✏️ تحديث سجل حضور طالب
        /// </summary>
        Task<ResponseDto<StudentAttendanceDto>> UpdateStudentAttendanceAsync(int id, UpdateStudentAttendanceDto updateDto);

        /// <summary>
        /// 🗑️ حذف سجل حضور طالب
        /// </summary>
        Task<ResponseDto> DeleteStudentAttendanceAsync(int id);

        #endregion

        #region Employee Attendance

        /// <summary>
        /// 📋 الحصول على جميع سجلات حضور الموظفين
        /// </summary>
        Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetAllEmployeeAttendancesAsync();

        /// <summary>
        /// 📋 الحصول على سجلات حضور موظف معين
        /// </summary>
        Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetEmployeeAttendancesByEmployeeIdAsync(int employeeId);

        /// <summary>
        /// 📋 الحصول على سجلات حضور مدرسة معينة في تاريخ محدد
        /// </summary>
        Task<ResponseDto<IEnumerable<EmployeeAttendanceDto>>> GetEmployeeAttendancesBySchoolAndDateAsync(int schoolId, DateTime date);

        /// <summary>
        /// 📊 الحصول على إحصائيات حضور موظف معين
        /// </summary>
        Task<ResponseDto<EmployeeAttendanceStatisticsDto>> GetEmployeeAttendanceStatisticsAsync(int employeeId, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// 🔍 الحصول على سجل حضور موظف بواسطة المعرف
        /// </summary>
        Task<ResponseDto<EmployeeAttendanceDto>> GetEmployeeAttendanceByIdAsync(int id);

        /// <summary>
        /// 🔍 الحصول على سجل حضور موظف في تاريخ محدد
        /// </summary>
        Task<ResponseDto<EmployeeAttendanceDto>> GetEmployeeAttendanceByEmployeeAndDateAsync(int employeeId, DateTime date);

        /// <summary>
        /// ➕ إنشاء سجل حضور موظف جديد
        /// </summary>
        Task<ResponseDto<EmployeeAttendanceDto>> CreateEmployeeAttendanceAsync(CreateEmployeeAttendanceDto createDto);

        /// <summary>
        /// ✏️ تحديث سجل حضور موظف
        /// </summary>
        Task<ResponseDto<EmployeeAttendanceDto>> UpdateEmployeeAttendanceAsync(int id, UpdateEmployeeAttendanceDto updateDto);

        /// <summary>
        /// 🗑️ حذف سجل حضور موظف
        /// </summary>
        Task<ResponseDto> DeleteEmployeeAttendanceAsync(int id);

        #endregion
    }
}