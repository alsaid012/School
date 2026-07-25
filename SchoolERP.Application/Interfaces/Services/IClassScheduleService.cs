using SchoolERP.Application.DTOs.ClassSchedules;
using SchoolERP.Application.DTOs.Common;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📅  واجهة خدمة جدول الحصص (IClassScheduleService)
    /// 📌  الوظيفة: تعريف عمليات إدارة جدول الحصص
    /// 📦  الاستخدام: في ClassSchedulesController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IClassScheduleService
    {
        /// <summary>
        /// 📋 الحصول على جميع جداول الحصص
        /// </summary>
        Task<ResponseDto<IEnumerable<ClassScheduleDto>>> GetAllAsync();

        /// <summary>
        /// 📋 الحصول على جميع جداول الحصص مع الفلترة
        /// </summary>
        /// <param name="filter">معايير الفلترة</param>
        Task<ResponseDto<IEnumerable<ClassScheduleDto>>> GetFilteredAsync(ClassScheduleFilterDto filter);

        /// <summary>
        /// 📋 الحصول على جداول فصل معين (مع إمكانية تحديد السنة الدراسية)
        /// </summary>
        /// <param name="classRoomId">معرف الفصل</param>
        /// <param name="academicYearId">معرف السنة الدراسية (اختياري)</param>
        Task<ResponseDto<IEnumerable<ClassScheduleDto>>> GetByClassRoomIdAsync(int classRoomId, int? academicYearId = null);

        /// <summary>
        /// 📋 الحصول على جداول معلم معين (مع إمكانية تحديد السنة الدراسية)
        /// </summary>
        /// <param name="teacherId">معرف المعلم</param>
        /// <param name="academicYearId">معرف السنة الدراسية (اختياري)</param>
        Task<ResponseDto<IEnumerable<ClassScheduleDto>>> GetByTeacherIdAsync(int teacherId, int? academicYearId = null);

        /// <summary>
        /// 📋 الحصول على جداول مادة معينة (مع إمكانية تحديد السنة الدراسية)
        /// </summary>
        /// <param name="subjectId">معرف المادة</param>
        /// <param name="academicYearId">معرف السنة الدراسية (اختياري)</param>
        Task<ResponseDto<IEnumerable<ClassScheduleDto>>> GetBySubjectIdAsync(int subjectId, int? academicYearId = null);

        /// <summary>
        /// 📋 الحصول على جداول عام دراسي معين
        /// </summary>
        /// <param name="academicYearId">معرف السنة الدراسية</param>
        Task<ResponseDto<IEnumerable<ClassScheduleDto>>> GetByAcademicYearIdAsync(int academicYearId);

        /// <summary>
        /// 📋 الحصول على الجدول الأسبوعي لفصل معين
        /// </summary>
        /// <param name="classRoomId">معرف الفصل</param>
        /// <param name="academicYearId">معرف السنة الدراسية (اختياري)</param>
        Task<ResponseDto<Dictionary<string, IEnumerable<ClassScheduleDto>>>> GetWeeklyScheduleAsync(int classRoomId, int? academicYearId = null);

        /// <summary>
        /// 📋 الحصول على جداول الحصص للقوائم المنسدلة
        /// </summary>
        /// <param name="classRoomId">معرف الفصل (اختياري)</param>
        Task<ResponseDto<IEnumerable<ClassScheduleLookupDto>>> GetLookupAsync(int? classRoomId = null);

        /// <summary>
        /// 🔍 الحصول على جدول بواسطة المعرف
        /// </summary>
        /// <param name="id">معرف الجدول</param>
        Task<ResponseDto<ClassScheduleDto>> GetByIdAsync(int id);

        /// <summary>
        /// ✅ التحقق من وجود تعارض في الوقت لنفس الفصل
        /// </summary>
        /// <param name="classRoomId">معرف الفصل</param>
        /// <param name="academicYearId">معرف السنة الدراسية</param>
        /// <param name="dayOfWeek">اليوم</param>
        /// <param name="startTime">وقت البداية</param>
        /// <param name="endTime">وقت النهاية</param>
        /// <param name="excludeId">معرف الجدول المستثنى (للتحديث)</param>
        Task<ResponseDto<bool>> IsConflictExistsAsync(
            int classRoomId,
            int academicYearId,
            DayOfWeek dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime,
            int? excludeId = null);

        /// <summary>
        /// ✅ التحقق من وجود تعارض في وقت المعلم
        /// </summary>
        /// <param name="teacherId">معرف المعلم</param>
        /// <param name="academicYearId">معرف السنة الدراسية</param>
        /// <param name="dayOfWeek">اليوم</param>
        /// <param name="startTime">وقت البداية</param>
        /// <param name="endTime">وقت النهاية</param>
        /// <param name="excludeId">معرف الجدول المستثنى (للتحديث)</param>
        Task<ResponseDto<bool>> IsTeacherConflictExistsAsync(
            int teacherId,
            int academicYearId,
            DayOfWeek dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime,
            int? excludeId = null);

        /// <summary>
        /// ✅ التحقق من وجود تعارض في رقم الحصة لنفس الفصل
        /// </summary>
        /// <param name="classRoomId">معرف الفصل</param>
        /// <param name="academicYearId">معرف السنة الدراسية</param>
        /// <param name="dayOfWeek">اليوم</param>
        /// <param name="periodNumber">رقم الحصة</param>
        /// <param name="excludeId">معرف الجدول المستثنى (للتحديث)</param>
        Task<ResponseDto<bool>> IsPeriodConflictExistsAsync(
            int classRoomId,
            int academicYearId,
            DayOfWeek dayOfWeek,
            int periodNumber,
            int? excludeId = null);

        /// <summary>
        /// ➕ إنشاء جدول جديد
        /// </summary>
        /// <param name="createDto">بيانات الجدول الجديد</param>
        Task<ResponseDto<ClassScheduleDto>> CreateAsync(CreateClassScheduleDto createDto);

        /// <summary>
        /// ✏️ تحديث بيانات جدول
        /// </summary>
        /// <param name="id">معرف الجدول</param>
        /// <param name="updateDto">بيانات التحديث</param>
        Task<ResponseDto<ClassScheduleDto>> UpdateAsync(int id, UpdateClassScheduleDto updateDto);

        /// <summary>
        /// 🗑️ حذف جدول
        /// </summary>
        /// <param name="id">معرف الجدول</param>
        Task<ResponseDto> DeleteAsync(int id);

        /// <summary>
        /// ✅ تفعيل/تعطيل جدول
        /// </summary>
        /// <param name="id">معرف الجدول</param>
        Task<ResponseDto> ToggleStatusAsync(int id);

        /// <summary>
        /// 📊 الحصول على إحصاءات الحصص
        /// </summary>
        /// <param name="academicYearId">معرف السنة الدراسية (اختياري)</param>
        Task<ResponseDto<ClassScheduleStatisticsDto>> GetStatisticsAsync(int? academicYearId = null);
    }
}














//using SchoolERP.Application.DTOs.ClassSchedules;
//using SchoolERP.Application.DTOs.Common;

//namespace SchoolERP.Application.Interfaces.Services
//{
//    /// <summary>
//    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
//    /// 📅  واجهة خدمة جدول الحصص (IClassScheduleService)
//    /// 📌  الوظيفة: تعريف عمليات إدارة جدول الحصص
//    /// 📦  الاستخدام: في ClassSchedulesController
//    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
//    /// </summary>
//    public interface IClassScheduleService
//    {
//        /// <summary>
//        /// 📋 الحصول على جميع جداول الحصص
//        /// </summary>
//        Task<ResponseDto<IEnumerable<ClassScheduleDto>>> GetAllAsync();

//        /// <summary>
//        /// 📋 الحصول على جداول فصل معين
//        /// </summary>
//        Task<ResponseDto<IEnumerable<ClassScheduleDto>>> GetByClassRoomIdAsync(int classRoomId);

//        /// <summary>
//        /// 📋 الحصول على جداول معلم معين
//        /// </summary>
//        Task<ResponseDto<IEnumerable<ClassScheduleDto>>> GetByTeacherIdAsync(int teacherId);

//        /// <summary>
//        /// 📋 الحصول على جداول مادة معينة
//        /// </summary>
//        Task<ResponseDto<IEnumerable<ClassScheduleDto>>> GetBySubjectIdAsync(int subjectId);

//        /// <summary>
//        /// 📋 الحصول على جداول عام دراسي معين
//        /// </summary>
//        Task<ResponseDto<IEnumerable<ClassScheduleDto>>> GetByAcademicYearIdAsync(int academicYearId);

//        /// <summary>
//        /// 📋 الحصول على الجدول الأسبوعي لفصل معين
//        /// </summary>
//        Task<ResponseDto<Dictionary<string, IEnumerable<ClassScheduleDto>>>> GetWeeklyScheduleAsync(int classRoomId);

//        /// <summary>
//        /// 📋 الحصول على جداول الحصص للقوائم المنسدلة
//        /// </summary>
//        Task<ResponseDto<IEnumerable<ClassScheduleLookupDto>>> GetLookupAsync(int? classRoomId = null);

//        /// <summary>
//        /// 🔍 الحصول على جدول بواسطة المعرف
//        /// </summary>
//        Task<ResponseDto<ClassScheduleDto>> GetByIdAsync(int id);

//        /// <summary>
//        /// ➕ إنشاء جدول جديد
//        /// </summary>
//        Task<ResponseDto<ClassScheduleDto>> CreateAsync(CreateClassScheduleDto createDto);

//        /// <summary>
//        /// ✏️ تحديث بيانات جدول
//        /// </summary>
//        Task<ResponseDto<ClassScheduleDto>> UpdateAsync(int id, UpdateClassScheduleDto updateDto);

//        /// <summary>
//        /// 🗑️ حذف جدول
//        /// </summary>
//        Task<ResponseDto> DeleteAsync(int id);

//        /// <summary>
//        /// ✅ التحقق من وجود تعارض في الجدول
//        /// </summary>
//        Task<ResponseDto<bool>> IsConflictExistsAsync(int classRoomId, int academicYearId, DayOfWeek day, TimeSpan startTime, int? excludeId = null);

//        /// <summary>
//        /// ✅ التحقق من وجود تعارض مع معلم
//        /// </summary>
//        Task<ResponseDto<bool>> IsTeacherConflictExistsAsync(int teacherId, int academicYearId, DayOfWeek day, TimeSpan startTime, int? excludeId = null);
//    }
//}