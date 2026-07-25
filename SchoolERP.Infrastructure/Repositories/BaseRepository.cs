using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Domain.Interfaces;
using SchoolERP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolERP.Infrastructure.Repositories
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏗️  المستودع الأساسي (BaseRepository)
    /// 📌  الوظيفة: قاعدة مشتركة تحتوي على دوال عامة لجميع الـ Repositories
    /// 🔄  الوراثة: ترث منها جميع الـ Repositories
    /// 📦  الميزة: تقليل التكرار وتوحيد الدوال المشتركة
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    /// <typeparam name="T">نوع الكيان (Entity)</typeparam>
    public abstract class BaseRepository<T> : IGenericRepository<T> where T : class, IBaseEntity
    {
        #region ════════════════════════════════════ الخصائص ════════════════════════════════════

        /// <summary>
        /// قاعدة البيانات (DbContext)
        /// </summary>
        protected readonly ApplicationDbContext _context;

        /// <summary>
        /// مجموعة الكيان في قاعدة البيانات (DbSet)
        /// </summary>
        protected readonly DbSet<T> _dbSet;

        #endregion

        #region ════════════════════════════════════ البناء ════════════════════════════════════

        /// <summary>
        /// المُنشئ - يستقبل قاعدة البيانات
        /// </summary>
        /// <param name="context">قاعدة البيانات (ApplicationDbContext)</param>
        protected BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        #endregion

        #region ════════════════════════════════════ عمليات CRUD الأساسية ════════════════════════════════════

        /// <summary>
        /// 🔍 الحصول على كيان بواسطة المعرف (ID)
        /// </summary>
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// 📋 الحصول على جميع الكيانات
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// 🔎 البحث عن كيانات حسب شرط معين
        /// </summary>
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// ➕ إضافة كيان جديد
        /// </summary>
        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// ➕➕ إضافة مجموعة من الكيانات
        /// </summary>
        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        /// <summary>
        /// ✏️ تحديث كيان موجود
        /// </summary>
        public virtual Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 🗑️ حذف كيان (Hard Delete)
        /// </summary>
        public virtual Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 🗑️🗑️ حذف مجموعة من الكيانات (Hard Delete)
        /// </summary>
        public virtual Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        /// <summary>
        /// ✅ التحقق من وجود كيان يحقق شرط معين
        /// </summary>
        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        /// <summary>
        /// 📊 حساب عدد الكيانات (مع أو بدون شرط)
        /// </summary>
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _dbSet.CountAsync();
            return await _dbSet.CountAsync(predicate);
        }

        #endregion

        #region ════════════════════════════════════ دوال عامة مشتركة ════════════════════════════════════

        /// <summary>
        /// 🔍 البحث عن كيان بواسطة الكود (باستخدام Reflection)
        /// </summary>
        /// <remarks>
        /// يتم البحث في الخصائص التالية:
        /// - Code, SubjectCode, TeacherCode, EmployeeCode, SchoolCode, StudentCode
        /// </remarks>
        public virtual async Task<T?> GetByCodeAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            var property = typeof(T).GetProperty("Code") ??
                          typeof(T).GetProperty("SubjectCode") ??
                          typeof(T).GetProperty("TeacherCode") ??
                          typeof(T).GetProperty("EmployeeCode") ??
                          typeof(T).GetProperty("SchoolCode") ??
                          typeof(T).GetProperty("StudentCode") ??
                          typeof(T).GetProperty("GovernorateCode") ??
                          typeof(T).GetProperty("DepartmentCode") ??
                          typeof(T).GetProperty("ClassCode");

            if (property == null)
                return null;

            var parameter = Expression.Parameter(typeof(T), "e");
            var propertyAccess = Expression.Property(parameter, property);
            var constant = Expression.Constant(code);
            var equals = Expression.Equal(propertyAccess, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);

            return await _dbSet.FirstOrDefaultAsync(lambda);
        }

        /// <summary>
        /// 📋 الحصول على كيان مع جميع البيانات المرتبطة (يتم Override في الـ Repository الخاص)
        /// </summary>
        public virtual async Task<T?> GetWithDetailsAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        /// <summary>
        /// ✅ التحقق من وجود كيان بنفس الاسم
        /// </summary>
        public virtual async Task<bool> IsNameExistsAsync(string name, int? excludeId = null)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            var property = typeof(T).GetProperty("Name") ??
                          typeof(T).GetProperty("SubjectName") ??
                          typeof(T).GetProperty("SchoolName") ??
                          typeof(T).GetProperty("ClassName") ??
                          typeof(T).GetProperty("GradeName") ??
                          typeof(T).GetProperty("YearName") ??
                          typeof(T).GetProperty("FullName") ??
                          typeof(T).GetProperty("Username");

            if (property == null)
                return false;

            var parameter = Expression.Parameter(typeof(T), "e");
            var propertyAccess = Expression.Property(parameter, property);
            var constant = Expression.Constant(name);
            var equals = Expression.Equal(propertyAccess, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);

            var query = _dbSet.Where(lambda);
            if (excludeId.HasValue)
                query = query.Where(e => e.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        /// <summary>
        /// ✅ التحقق من وجود كيان بنفس الكود
        /// </summary>
        public virtual async Task<bool> IsCodeExistsAsync(string code, int? excludeId = null)
        {
            if (string.IsNullOrEmpty(code))
                return false;

            var entity = await GetByCodeAsync(code);
            if (entity == null)
                return false;

            if (excludeId.HasValue && entity.Id == excludeId.Value)
                return false;

            return true;
        }


        /// <summary>
        /// 📋 جلب جميع الكيانات مع Includes
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }
        #endregion

        #region ════════════════════════════════════ دوال مساعدة ════════════════════════════════════

        /// <summary>
        /// 🔧 مساعد لإضافة Includes بسهولة (لجلب البيانات المرتبطة)
        /// </summary>
        /// <param name="includes">مصفوفة من الـ Includes</param>
        /// <returns>IQueryable مع الـ Includes</returns>
        protected IQueryable<T> IncludeMultiple(params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }

        /// <summary>
        /// 🔧 مساعد لإضافة Includes مع ThenIncludes
        /// </summary>
        protected IQueryable<T> IncludeMultiple(Func<IQueryable<T>, IQueryable<T>> includeFunc)
        {
            return includeFunc(_dbSet);
        }


        #endregion


        /// <summary>
        /// 📋 الحصول على جميع السجلات مع البيانات المرتبطة
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllWithDetailsAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }
    }
}