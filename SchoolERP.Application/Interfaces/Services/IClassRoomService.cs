using SchoolERP.Application.DTOs.ClassRooms;
using SchoolERP.Application.DTOs.Common;

namespace SchoolERP.Application.Interfaces.Services
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏫  واجهة خدمة الفصول الدراسية (IClassRoomService)
    /// 📌  الوظيفة: تعريف عمليات إدارة الفصول الدراسية
    /// 📦  الاستخدام: في ClassRoomsController
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IClassRoomService
    {
        /// <summary>
        /// 📋 الحصول على جميع الفصول الدراسية
        /// </summary>
        Task<ResponseDto<IEnumerable<ClassRoomDto>>> GetAllAsync();


        /// <summary>
        /// 🔍 الحصول على فصل بواسطة المعرف
        /// </summary>
        Task<ResponseDto<ClassRoomDetailsDto>> GetByIdAsync(int id);


        /// <summary>
        /// 📋 الحصول على الفصول التابعة لصف معين
        /// </summary>
        Task<ResponseDto<IEnumerable<ClassRoomDto>>> GetByGradeLevelIdAsync(int gradeLevelId);

        /// <summary>
        /// 📋 الحصول على الفصول التابعة لمدرسة معينة
        /// </summary>
        Task<ResponseDto<IEnumerable<ClassRoomDto>>> GetBySchoolIdAsync(int schoolId);

        /// <summary>
        /// 📋 الحصول على الفصول التي يشرف عليها معلم معين
        /// </summary>
        Task<ResponseDto<IEnumerable<ClassRoomDto>>> GetByTeacherIdAsync(int teacherId);

        /// <summary>
        /// 📋 الحصول على الفصول للقوائم المنسدلة
        /// </summary>
        Task<ResponseDto<IEnumerable<ClassRoomLookupDto>>> GetLookupAsync(int? gradeLevelId = null);

        /// <summary>
        /// 📊 الحصول على إحصائيات الفصل
        /// </summary>
        Task<ResponseDto<ClassRoomStatisticsDto>> GetStatisticsAsync(int classRoomId);

        /// <summary>
        /// ➕ إنشاء فصل جديد
        /// </summary>
        Task<ResponseDto<ClassRoomDto>> CreateAsync(CreateClassRoomDto createDto);

        /// <summary>
        /// ✏️ تحديث بيانات فصل
        /// </summary>
        Task<ResponseDto<ClassRoomDto>> UpdateAsync(int id, UpdateClassRoomDto updateDto);

        /// <summary>
        /// 🗑️ حذف فصل (Soft Delete)
        /// </summary>
        Task<ResponseDto> DeleteAsync(int id);

        /// <summary>
        /// ✅ التحقق من وجود فصل بنفس الاسم في الصف
        /// </summary>
        Task<ResponseDto<bool>> IsNameExistsAsync(int gradeLevelId, string name, int? excludeId = null);
    }
}