using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Students;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🧑‍🎓  واجهة خدمة الطلاب (IStudentService)
    /// 📌  الوظيفة: تعريف عمليات إدارة الطلاب
    /// 📦  الاستخدام: في StudentsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IStudentService
    {
        /// <summary>
        /// 📋 الحصول على جميع الطلاب
        /// </summary>
        Task<ResponseDto<IEnumerable<StudentDto>>> GetAllAsync();

        /// <summary>
        /// 🔍 الحصول على طالب بواسطة المعرف
        /// </summary>
        Task<ResponseDto<StudentDetailsDto>> GetByIdAsync(int id);

        /// <summary>
        /// 🔍 الحصول على طالب بواسطة الكود
        /// </summary>
        Task<ResponseDto<StudentDto>> GetByCodeAsync(string studentCode);

        /// <summary>
        /// 📋 الحصول على الطلاب في فصل معين
        /// </summary>
        Task<ResponseDto<IEnumerable<StudentDto>>> GetByClassRoomIdAsync(int classRoomId);

        /// <summary>
        /// 📋 الحصول على الطلاب في صف معين
        /// </summary>
        Task<ResponseDto<IEnumerable<StudentDto>>> GetByGradeLevelIdAsync(int gradeLevelId);

        /// <summary>
        /// 📋 الحصول على الطلاب في عام دراسي معين
        /// </summary>
        Task<ResponseDto<IEnumerable<StudentDto>>> GetByAcademicYearIdAsync(int academicYearId);

        /// <summary>
        /// 📋 الحصول على الطلاب تحت إشراف معلم معين
        /// </summary>
        Task<ResponseDto<IEnumerable<StudentDto>>> GetByTeacherIdAsync(int teacherId);

        /// <summary>
        /// 📋 الحصول على الطلاب المتخرجين
        /// </summary>
        Task<ResponseDto<IEnumerable<StudentDto>>> GetGraduatedStudentsAsync();

        /// <summary>
        /// 📋 الحصول على الطلاب النشطين
        /// </summary>
        Task<ResponseDto<IEnumerable<StudentDto>>> GetActiveStudentsAsync();

        /// <summary>
        /// 📋 الحصول على الطلاب للقوائم المنسدلة
        /// </summary>
        Task<ResponseDto<IEnumerable<StudentLookupDto>>> GetLookupAsync(int? classRoomId = null);
           
        /// <summary>
        /// 📊 الحصول على إحصائيات الطالب
        /// </summary>
        Task<ResponseDto<StudentStatisticsDto>> GetStatisticsAsync(int studentId);

        /// <summary>
        /// ➕ إنشاء طالب جديد
        /// </summary>
        Task<ResponseDto<StudentDto>> CreateAsync(CreateStudentDto createDto);

        /// <summary>
        /// ✏️ تحديث بيانات طالب
        /// </summary>
        Task<ResponseDto<StudentDto>> UpdateAsync(int id, UpdateStudentDto updateDto);

        /// <summary>
        /// 🗑️ حذف طالب (Soft Delete)
        /// </summary>
        Task<ResponseDto> DeleteAsync(int id);

        /// <summary>
        /// ✅ التحقق من وجود كود طالب
        /// </summary>
        Task<ResponseDto<bool>> IsStudentCodeExistsAsync(string studentCode);
    }
}