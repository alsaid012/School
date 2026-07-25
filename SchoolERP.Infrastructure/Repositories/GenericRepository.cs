using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Interfaces;
using SchoolERP.Infrastructure.Data;
using System.Linq.Expressions;

namespace SchoolERP.Infrastructure.Repositories
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏗️  المستودع العام (Generic Repository)
    /// 📌  الوظيفة: توفير عمليات CRUD أساسية لجميع الكيانات
    /// 🔄  الوراثة: تطبق واجهة IGenericRepository
    /// 📦  الاستخدام: تستخدم كقاعدة لجميع المستودعات الأخرى
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    /// <typeparam name="T">نوع الكيان (Entity)</typeparam>
    public class GenericRepository<T> : BaseRepository<T>, IGenericRepository<T> where T : class, IBaseEntity
    {
        //#region ════════════════════════════════════ الخصائص ════════════════════════════════════

        ///// <summary>
        ///// قاعدة البيانات (DbContext)
        ///// </summary>
        //protected readonly ApplicationDbContext _context;

        ///// <summary>
        ///// مجموعة الكيان في قاعدة البيانات (DbSet)
        ///// </summary>
        //protected readonly DbSet<T> _dbSet;

        //#endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        /// <summary>
        /// المُنشئ - يستقبل قاعدة البيانات ويجهز الـ DbSet
        /// </summary>
        /// <param name="context">قاعدة البيانات (ApplicationDbContext)</param>
        public GenericRepository(ApplicationDbContext context) : base(context)
        {
            //_context = context;
            //_dbSet = context.Set<T>();
        }

        #endregion
        #region ===================== كود القديم ===============================
        //#region ════════════════════════════════════ عمليات القراءة ════════════════════════════════════

        ///// <summary>
        ///// 🔍 الحصول على كيان بواسطة المعرف (ID)
        ///// </summary>
        ///// <param name="id">المعرف (Primary Key)</param>
        ///// <returns>الكيان المطلوب أو null إذا لم يوجد</returns>
        //public virtual async Task<T?> GetByIdAsync(int id)
        //{
        //    return await _dbSet.FindAsync(id);
        //}

        ///// <summary>
        ///// 📋 الحصول على جميع الكيانات
        ///// </summary>
        ///// <returns>قائمة بجميع الكيانات</returns>
        //public virtual async Task<IEnumerable<T>> GetAllAsync()
        //{
        //    return await _dbSet.ToListAsync();
        //}

        ///// <summary>
        ///// 🔎 البحث عن كيانات حسب شرط معين
        ///// </summary>
        ///// <param name="predicate">شرط البحث (Lambda Expression)</param>
        ///// <returns>قائمة الكيانات المطابقة للشرط</returns>
        //public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        //{
        //    return await _dbSet.Where(predicate).ToListAsync();
        //}

        ///// <summary>
        ///// ✅ التحقق من وجود كيان يحقق شرط معين
        ///// </summary>
        ///// <param name="predicate">شرط البحث</param>
        ///// <returns>true إذا وجد، false إذا لم يوجد</returns>
        //public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        //{
        //    return await _dbSet.AnyAsync(predicate);
        //}

        ///// <summary>
        ///// 📊 حساب عدد الكيانات (مع أو بدون شرط)
        ///// </summary>
        ///// <param name="predicate">شرط البحث (اختياري)</param>
        ///// <returns>عدد الكيانات</returns>
        //public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        //{
        //    if (predicate == null)
        //        return await _dbSet.CountAsync();

        //    return await _dbSet.CountAsync(predicate);
        //}

        //#endregion

        //#region ════════════════════════════════════ دوال عامة ════════════════════════════════════

        ///// <summary>
        ///// 🔍 البحث عن كيان بواسطة الكود (باستخدام Reflection)
        ///// </summary>
        //public virtual async Task<T?> GetByCodeAsync(string code)
        //{
        //    // نبحث عن خاصية Code أو SubjectCode أو TeacherCode أو EmployeeCode
        //    var property = typeof(T).GetProperty("Code") ??
        //                  typeof(T).GetProperty("SubjectCode") ??
        //                  typeof(T).GetProperty("TeacherCode") ??
        //                  typeof(T).GetProperty("EmployeeCode") ??
        //                  typeof(T).GetProperty("SchoolCode") ??
        //                  typeof(T).GetProperty("StudentCode");

        //    if (property == null)
        //        return null;

        //    var parameter = Expression.Parameter(typeof(T), "e");
        //    var propertyAccess = Expression.Property(parameter, property);
        //    var constant = Expression.Constant(code);
        //    var equals = Expression.Equal(propertyAccess, constant);
        //    var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);

        //    return await _dbSet.FirstOrDefaultAsync(lambda);
        //}

        ///// <summary>
        ///// 📋 الحصول على كيان مع جميع البيانات المرتبطة (يتم override في الـ Repository الخاص)
        ///// </summary>
        //public virtual async Task<T?> GetWithDetailsAsync(int id)
        //{
        //    return await GetByIdAsync(id);
        //}

        ///// <summary>
        ///// ✅ التحقق من وجود كيان بنفس الاسم
        ///// </summary>
        //public virtual async Task<bool> IsNameExistsAsync(string name, int? excludeId = null)
        //{
        //    var property = typeof(T).GetProperty("Name") ??
        //                  typeof(T).GetProperty("SubjectName") ??
        //                  typeof(T).GetProperty("SchoolName") ??
        //                  typeof(T).GetProperty("ClassName") ??
        //                  typeof(T).GetProperty("GradeName") ??
        //                  typeof(T).GetProperty("YearName") ??
        //                  typeof(T).GetProperty("FullName") ??
        //                  typeof(T).GetProperty("Username");

        //    if (property == null)
        //        return false;

        //    var parameter = Expression.Parameter(typeof(T), "e");
        //    var propertyAccess = Expression.Property(parameter, property);
        //    var constant = Expression.Constant(name);
        //    var equals = Expression.Equal(propertyAccess, constant);
        //    var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);

        //    var query = _dbSet.Where(lambda);
        //    if (excludeId.HasValue)
        //        query = query.Where(e => e.Id != excludeId.Value);

        //    return await query.AnyAsync();
        //}

        ///// <summary>
        ///// ✅ التحقق من وجود كيان بنفس الكود
        ///// </summary>
        //public virtual async Task<bool> IsCodeExistsAsync(string code, int? excludeId = null)
        //{
        //    var entity = await GetByCodeAsync(code);
        //    if (entity == null)
        //        return false;

        //    if (excludeId.HasValue && entity.Id == excludeId.Value)
        //        return false;

        //    return true;
        //}

        //#endregion


        //#region ════════════════════════════════════ عمليات الإضافة ════════════════════════════════════

        ///// <summary>
        ///// ➕ إضافة كيان جديد
        ///// </summary>
        ///// <param name="entity">الكيان المراد إضافته</param>
        ///// <returns>الكيان بعد الإضافة (مع المعرف الجديد)</returns>
        //public virtual async Task<T> AddAsync(T entity)
        //{
        //    await _dbSet.AddAsync(entity);
        //    return entity;
        //}

        ///// <summary>
        ///// ➕➕ إضافة مجموعة من الكيانات
        ///// </summary>
        ///// <param name="entities">قائمة الكيانات المراد إضافتها</param>
        ///// <returns>قائمة الكيانات بعد الإضافة</returns>
        //public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        //{
        //    await _dbSet.AddRangeAsync(entities);
        //    return entities;
        //}

        //#endregion

        //#region ════════════════════════════════════ عمليات التحديث ════════════════════════════════════

        ///// <summary>
        ///// ✏️ تحديث كيان موجود
        ///// </summary>
        ///// <param name="entity">الكيان المراد تحديثه</param>
        //public virtual Task UpdateAsync(T entity)
        //{
        //    _dbSet.Update(entity);
        //    return Task.CompletedTask;
        //}

        //#endregion

        //#region ════════════════════════════════════ عمليات الحذف ════════════════════════════════════

        ///// <summary>
        ///// 🗑️ حذف كيان (Hard Delete - حذف نهائي من قاعدة البيانات)
        ///// </summary>
        ///// <param name="entity">الكيان المراد حذفه</param>
        //public virtual Task DeleteAsync(T entity)
        //{
        //    _dbSet.Remove(entity);
        //    return Task.CompletedTask;
        //}

        ///// <summary>
        ///// 🗑️🗑️ حذف مجموعة من الكيانات (Hard Delete)
        ///// </summary>
        ///// <param name="entities">قائمة الكيانات المراد حذفها</param>
        //public virtual Task DeleteRangeAsync(IEnumerable<T> entities)
        //{
        //    _dbSet.RemoveRange(entities);
        //    return Task.CompletedTask;
        //}

        //#endregion
        #endregion

    }
}