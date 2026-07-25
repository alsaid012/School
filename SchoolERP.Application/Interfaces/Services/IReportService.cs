using SchoolERP.Application.DTOs.Common;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📊  واجهة خدمة التقارير (IReportService)
    /// 📌  الوظيفة: تعريف عمليات إنشاء التقارير المختلفة
    /// 📦  الاستخدام: في ReportsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// 📊 تقرير حضور الطلاب اليومي
        /// </summary>
        Task<ResponseDto<object>> GetDailyStudentAttendanceReportAsync(int schoolId, DateTime date);

        /// <summary>
        /// 📊 تقرير حضور الطلاب الشهري
        /// </summary>
        Task<ResponseDto<object>> GetMonthlyStudentAttendanceReportAsync(int schoolId, int month, int year);

        /// <summary>
        /// 📊 تقرير حضور الموظفين اليومي
        /// </summary>
        Task<ResponseDto<object>> GetDailyEmployeeAttendanceReportAsync(int schoolId, DateTime date);

        /// <summary>
        /// 📊 تقرير نتائج الامتحانات
        /// </summary>
        Task<ResponseDto<object>> GetExamResultsReportAsync(int examId);

        /// <summary>
        /// 📊 تقرير نتائج الطالب
        /// </summary>
        Task<ResponseDto<object>> GetStudentReportAsync(int studentId, int academicYearId);

        /// <summary>
        /// 📊 تقرير أداء المعلم
        /// </summary>
        Task<ResponseDto<object>> GetTeacherPerformanceReportAsync(int teacherId, int academicYearId);

        /// <summary>
        /// 📊 تقرير الجدول الأسبوعي للفصل
        /// </summary>
        Task<ResponseDto<object>> GetWeeklyScheduleReportAsync(int classRoomId);

        /// <summary>
        /// 📊 تقرير إحصائيات المدرسة
        /// </summary>
        Task<ResponseDto<object>> GetSchoolStatisticsReportAsync(int schoolId);

        /// <summary>
        /// 📊 تقرير توزيع الطلاب حسب الصفوف
        /// </summary>
        Task<ResponseDto<object>> GetStudentDistributionReportAsync(int schoolId);

        /// <summary>
        /// 📊 تقرير توزيع المعلمين حسب المواد
        /// </summary>
        Task<ResponseDto<object>> GetTeacherDistributionReportAsync(int schoolId);

        /// <summary>
        /// 📊 تقرير الغرف المدرسية الأسبوعي
        /// </summary>
        Task<ResponseDto<object>> GetWeeklyClassRoomScheduleReportAsync(int schoolId, int academicYearId);

        /// <summary>
        /// 📊 تصدير التقرير بصيغة PDF
        /// </summary>
        Task<ResponseDto<byte[]>> ExportToPdfAsync(string reportName, object data);

        /// <summary>
        /// 📊 تصدير التقرير بصيغة Excel
        /// </summary>
        Task<ResponseDto<byte[]>> ExportToExcelAsync(string reportName, object data);
    }
}