using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Entities;
using SchoolERP.Infrastructure.Data;

namespace SchoolERP.Infrastructure.Repositories
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🧑‍🎓  مستودع الطلاب (StudentRepository)
    /// 📌  الوظيفة: تنفيذ عمليات قاعدة البيانات الخاصة بالطلاب
    /// 🔄  الوراثة: ترث من GenericRepository وتطبق IStudentRepository
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        #region ════════════════════════════════════ البناء ════════════════════════════════════

        /// <summary>
        /// المُنشئ - يستقبل قاعدة البيانات ويمررها إلى القاعدة
        /// </summary>
        /// <param name="context">قاعدة البيانات (ApplicationDbContext)</param>
        public StudentRepository(ApplicationDbContext context) : base(context)
        {
        }

        #endregion

        #region ════════════════════════════════════ البحث عن طالب ════════════════════════════════════

        /// <summary>
        /// 🔍 البحث عن طالب بواسطة كود الطالب
        /// </summary>
        /// <param name="studentCode">كود الطالب (مثل: STU-2024-001)</param>
        /// <returns>الطالب أو null إذا لم يوجد</returns>
        public async Task<Student?> GetStudentByCodeAsync(string studentCode)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.StudentCode == studentCode);
        }

        /// <summary>
        /// 📋 الحصول على طالب مع جميع بياناته المرتبطة
        /// </summary>
        /// <remarks>
        /// يتم جلب البيانات التالية مع الطالب:
        /// - User (المستخدم) مع School (المدرسة) و Contacts (جهات الاتصال)
        /// - AcademicYear (العام الدراسي)
        /// - ClassRoom (الفصل) مع GradeLevel (الصف)
        /// - ExamResults (نتائج الامتحانات) مع Exam (الامتحان) و Subject (المادة)
        /// - Attendances (سجل الحضور)
        /// </remarks>
        /// <param name="studentId">معرف الطالب</param>
        /// <returns>الطالب مع البيانات المرتبطة أو null</returns>
        public async Task<Student?> GetStudentWithDetailsAsync(int studentId)
        {
            return await _dbSet
                .Include(s => s.User)
                    .ThenInclude(u => u != null ? u.School : null!)
                .Include(s => s.User)
                    .ThenInclude(u => u != null ? u.Contacts : null!)
                .Include(s => s.AcademicYear)
                .Include(s => s.ClassRoom)
                    .ThenInclude(c => c != null ? c.GradeLevel : null!)
                .Include(s => s.ExamResults)
                    .ThenInclude(er => er != null ? er.Exam : null!)
                        .ThenInclude(e => e != null ? e.Subject : null!)
                .Include(s => s.Attendances)
                .FirstOrDefaultAsync(s => s.Id == studentId);
        }

        #endregion

        #region ════════════════════════════════════ جلب قوائم الطلاب ════════════════════════════════════

        /// <summary>
        /// 📋 الحصول على جميع الطلاب في فصل معين
        /// </summary>
        /// <remarks>
        /// يتم جلب بيانات المستخدم مع الطالب وترتيبهم حسب الاسم
        /// </remarks>
        /// <param name="classRoomId">معرف الفصل</param>
        /// <returns>قائمة الطلاب مرتبة حسب الاسم</returns>
        public async Task<IEnumerable<Student>> GetStudentsByClassRoomAsync(int classRoomId)
        {
            return await _dbSet
                .Where(s => s.ClassRoomId == classRoomId)
                .Include(s => s.User)
                .OrderBy(s => s.User != null ? s.User.FullName : "")
                .ToListAsync();
        }

        /// <summary>
        /// 📋 الحصول على جميع الطلاب في صف معين
        /// </summary>
        /// <remarks>
        /// يتم البحث عن الطلاب من خلال الفصل الدراسي المرتبط بالصف
        /// </remarks>
        /// <param name="gradeLevelId">معرف الصف الدراسي</param>
        /// <returns>قائمة الطلاب</returns>
        public async Task<IEnumerable<Student>> GetStudentsByGradeLevelAsync(int gradeLevelId)
        {
            return await _dbSet
                .Where(s => s.ClassRoom != null && s.ClassRoom.GradeLevelId == gradeLevelId)
                .Include(s => s.User)
                .Include(s => s.ClassRoom)
                 .OrderBy(s => s.User != null ? s.User.FullName : "")
                .ToListAsync();
        }

        /// <summary>
        /// 📋 الحصول على جميع الطلاب في عام دراسي معين
        /// </summary>
        /// <param name="academicYearId">معرف العام الدراسي</param>
        /// <returns>قائمة الطلاب مرتبة حسب الكود</returns>
        public async Task<IEnumerable<Student>> GetStudentsByAcademicYearAsync(int academicYearId)
        {
            return await _dbSet
                .Where(s => s.AcademicYearId == academicYearId)
                .Include(s => s.User)
                .Include(s => s.ClassRoom)
                    .ThenInclude(c => c != null ? c.GradeLevel : null!)
                .OrderBy(s => s.StudentCode)
                .ToListAsync();
        }

        /// <summary>
        /// 📋 الحصول على جميع الطلاب تحت إشراف معلم معين
        /// </summary>
        /// <remarks>
        /// يتم البحث عن الطلاب في الفصول التي يشرف عليها المعلم
        /// </remarks>
        /// <param name="teacherId">معرف المعلم</param>
        /// <returns>قائمة الطلاب</returns>
        public async Task<IEnumerable<Student>> GetStudentsByTeacherAsync(int teacherId)
        {
            return await _dbSet
                .Where(s => s.ClassRoom != null && s.ClassRoom.TeacherId == teacherId)
                .Include(s => s.User)
                .Include(s => s.ClassRoom)
                .OrderBy(s => s.User != null ? s.User.FullName : "")
                .ToListAsync();
        }

        /// <summary>
        /// 🎓 الحصول على جميع الطلاب المتخرجين
        /// </summary>
        /// <returns>قائمة الطلاب المتخرجين مرتبة حسب الكود</returns>
        public async Task<IEnumerable<Student>> GetGraduatedStudentsAsync()
        {
            return await _dbSet
                .Where(s => s.IsGraduated)
                .Include(s => s.User)
                .OrderBy(s => s.StudentCode)
                .ToListAsync();
        }

        /// <summary>
        /// ✅ الحصول على جميع الطلاب النشطين (غير المتخرجين)
        /// </summary>
        /// <returns>قائمة الطلاب النشطين مرتبة حسب الكود</returns>
        public async Task<IEnumerable<Student>> GetActiveStudentsAsync()
        {
            return await _dbSet
                .Where(s => !s.IsGraduated && s.IsActive)
                .Include(s => s.User)
                .Include(s => s.ClassRoom)
                    .ThenInclude(c => c != null ? c.GradeLevel : null!)
                .OrderBy(s => s.StudentCode)
                .ToListAsync();
        }

        /// <summary>
        /// 📋 الحصول على الطلاب مع سجلات الحضور الخاصة بهم
        /// </summary>
        /// <param name="academicYearId">معرف العام الدراسي</param>
        /// <returns>قائمة الطلاب مع سجلات الحضور</returns>
        public async Task<IEnumerable<Student>> GetStudentsWithAttendancesAsync(int academicYearId)
        {
            return await _dbSet
                .Where(s => s.AcademicYearId == academicYearId)
                .Include(s => s.User)
                .Include(s => s.ClassRoom)
                .Include(s => s.Attendances)
                .OrderBy(s => s.User != null ? s.User.FullName : "")
                .ToListAsync();
        }

        #endregion

        #region ════════════════════════════════════ التحقق من الوجود ════════════════════════════════════

        /// <summary>
        /// ✅ التحقق من وجود كود طالب مكرر
        /// </summary>
        /// <param name="studentCode">كود الطالب</param>
        /// <returns>true إذا كان موجود، false إذا لم يوجد</returns>
        public async Task<bool> StudentCodeExistsAsync(string studentCode)
        {
            return await _dbSet
                .AnyAsync(s => s.StudentCode == studentCode);
        }

        #endregion

        #region ════════════════════════════════════ إحصائيات الطلاب ════════════════════════════════════

        /// <summary>
        /// 📊 الحصول على إحصائيات الطلاب لمدرسة معينة
        /// </summary>
        /// <remarks>
        /// تشمل الإحصائيات:
        /// - إجمالي عدد الطلاب
        /// - عدد الطلاب المتخرجين
        /// - عدد الطلاب النشطين
        /// - توزيع الطلاب حسب الصفوف الدراسية
        /// </remarks>
        /// <param name="schoolId">معرف المدرسة</param>
        /// <returns>كائن يحتوي على الإحصائيات</returns>
        public async Task<object> GetStudentsStatisticsAsync(int schoolId)
        {
            // إجمالي عدد الطلاب في المدرسة
            var totalStudents = await _dbSet
                 .Where(s => s.User != null && s.User.SchoolId == schoolId)
                .CountAsync();

            // عدد الطلاب المتخرجين
            var graduatedStudents = await _dbSet
                  .Where(s => s.User != null && s.User.SchoolId == schoolId && !s.IsGraduated)
                .CountAsync();

            // عدد الطلاب النشطين (غير المتخرجين)
            var activeStudents = await _dbSet
                .Where(s => s.User != null && s.User.SchoolId == schoolId && !s.IsGraduated)
                .CountAsync();

            // توزيع الطلاب حسب الصفوف الدراسية
            var studentsByGrade = await _dbSet
                     .Where(s => s.User != null && s.User.SchoolId == schoolId && !s.IsGraduated)
                .GroupBy(s => s.ClassRoom != null && s.ClassRoom.GradeLevel != null ? s.ClassRoom.GradeLevel.GradeName : "بدون صف")
                .Select(g => new
                {
                    الصف = g.Key,
                    العدد = g.Count()
                })
                .ToListAsync();

            return new
            {
                إجمالي_الطلاب = totalStudents,
                عدد_المتخرجين = graduatedStudents,
                عدد_النشطين = activeStudents,
                توزيع_حسب_الصف = studentsByGrade
            };
        }

        #endregion
    }
}