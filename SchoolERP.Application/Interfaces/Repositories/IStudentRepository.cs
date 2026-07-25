using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🧑‍🎓  واجهة مستودع الطلاب (IStudentRepository)
    /// 📌  الوظيفة: تعريف العمليات الخاصة بالطلاب
    /// 🔄  الوراثة: ترث من IGenericRepository
    /// 📦  الاستخدام: تستخدم في طبقة الخدمات (Services)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IStudentRepository : IGenericRepository<Student>
    {
        #region ════════════════════════════════════ البحث عن طالب ════════════════════════════════════

        /// <summary>
        /// 🔍 البحث عن طالب بواسطة كود الطالب
        /// </summary>
        /// <param name="studentCode">كود الطالب (مثل: STU-2024-001)</param>
        /// <returns>الطالب أو null إذا لم يوجد</returns>
        Task<Student?> GetStudentByCodeAsync(string studentCode);

        /// <summary>
        /// 📋 الحصول على طالب مع جميع بياناته المرتبطة
        /// </summary>
        /// <remarks>
        /// يتم جلب البيانات التالية مع الطالب:
        /// - User (المستخدم) ← School (المدرسة) ← Contacts
        /// - AcademicYear (العام الدراسي)
        /// - ClassRoom (الفصل) ← GradeLevel (الصف)
        /// - ExamResults (نتائج الامتحانات) ← Exam (الامتحان) ← Subject (المادة)
        /// - Attendances (سجل الحضور)
        /// </remarks>
        /// <param name="studentId">معرف الطالب</param>
        /// <returns>الطالب مع البيانات المرتبطة أو null</returns>
        Task<Student?> GetStudentWithDetailsAsync(int studentId);

        #endregion

        #region ════════════════════════════════════ جلب قوائم الطلاب ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع الطلاب في فصل معين
        /// </summary>
        /// <param name="classRoomId">معرف الفصل</param>
        /// <returns>قائمة الطلاب مرتبة حسب الاسم</returns>
        Task<IEnumerable<Student>> GetStudentsByClassRoomAsync(int classRoomId);

        /// <summary>
        /// 📋 الحصول على جميع الطلاب في صف معين
        /// </summary>
        /// <param name="gradeLevelId">معرف الصف الدراسي</param>
        /// <returns>قائمة الطلاب</returns>
        Task<IEnumerable<Student>> GetStudentsByGradeLevelAsync(int gradeLevelId);

        /// <summary>
        /// 📋 الحصول على جميع الطلاب في عام دراسي معين
        /// </summary>
        /// <param name="academicYearId">معرف العام الدراسي</param>
        /// <returns>قائمة الطلاب مرتبة حسب الكود</returns>
        Task<IEnumerable<Student>> GetStudentsByAcademicYearAsync(int academicYearId);

        /// <summary>
        /// 📋 الحصول على جميع الطلاب تحت إشراف معلم معين
        /// </summary>
        /// <param name="teacherId">معرف المعلم</param>
        /// <returns>قائمة الطلاب</returns>
        Task<IEnumerable<Student>> GetStudentsByTeacherAsync(int teacherId);

        /// <summary>
        /// 🎓 الحصول على جميع الطلاب المتخرجين
        /// </summary>
        /// <returns>قائمة الطلاب المتخرجين</returns>
        Task<IEnumerable<Student>> GetGraduatedStudentsAsync();

        /// <summary>
        /// ✅ الحصول على جميع الطلاب النشطين (غير المتخرجين)
        /// </summary>
        /// <returns>قائمة الطلاب النشطين</returns>
        Task<IEnumerable<Student>> GetActiveStudentsAsync();

        /// <summary>
        /// 📋 الحصول على الطلاب مع سجلات الحضور الخاصة بهم
        /// </summary>
        /// <param name="academicYearId">معرف العام الدراسي</param>
        /// <returns>قائمة الطلاب مع سجلات الحضور</returns>
        Task<IEnumerable<Student>> GetStudentsWithAttendancesAsync(int academicYearId);

        #endregion

        #region ════════════════════════════════════ التحقق من الوجود ════════════════════════════════════

        /// <summary>
        /// ✅ التحقق من وجود كود طالب مكرر
        /// </summary>
        /// <param name="studentCode">كود الطالب</param>
        /// <returns>true إذا كان موجود، false إذا لم يوجد</returns>
        Task<bool> StudentCodeExistsAsync(string studentCode);

        #endregion
    }
}