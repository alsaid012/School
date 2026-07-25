using Microsoft.EntityFrameworkCore;
using SchoolERP.Domain.Interfaces;
using System.Linq.Expressions;

namespace SchoolERP.Infrastructure.Extensions
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🗑️  إضافات الحذف المنطقي (Soft Delete Extensions)
    /// 📌  الوظيفة: توفير دوال مساعدة للتعامل مع Soft Delete
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public static class SoftDeleteExtensions
    {
        /// <summary>
        /// 📋 الحصول على البيانات بما في ذلك المحذوفة
        /// </summary>
        public static IQueryable<T> WithDeleted<T>(this IQueryable<T> query) where T : class, ISoftDeleteEntity
        {
            return query.IgnoreQueryFilters();
        }

        /// <summary>
        /// 📋 الحصول على البيانات المحذوفة فقط
        /// </summary>
        public static IQueryable<T> OnlyDeleted<T>(this IQueryable<T> query) where T : class, ISoftDeleteEntity
        {
            return query.IgnoreQueryFilters().Where(e => e.IsDeleted);
        }

        /// <summary>
        /// 🔍 البحث في البيانات بما فيها المحذوفة
        /// </summary>
        public static async Task<T?> FindByIdWithDeletedAsync<T>(this DbSet<T> dbSet, int id)
            where T : class, ISoftDeleteEntity
        {
            return await dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <summary>
        /// 🔄 استعادة البيانات المحذوفة (للكيانات التي ترث من SoftDeleteBaseEntity)
        /// </summary>
        public static async Task<bool> RestoreAsync<T>(this DbSet<T> dbSet, int id)
            where T : class, ISoftDeleteEntity, new()
        {
            var entity = await dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null || !entity.IsDeleted)
                return false;

            // ✅ استخدام Reflection لاستدعاء Restore
            var restoreMethod = entity.GetType().GetMethod("Restore");
            if (restoreMethod != null)
            {
                restoreMethod.Invoke(entity, null);
            }
            else
            {
                // ✅ طريقة بديلة: تعيين الخصائص مباشرة
                var isDeletedProperty = entity.GetType().GetProperty("IsDeleted");
                var deletedAtProperty = entity.GetType().GetProperty("DeletedAt");
                var deletedByProperty = entity.GetType().GetProperty("DeletedBy");
                var deleteReasonProperty = entity.GetType().GetProperty("DeleteReason");
                var isActiveProperty = entity.GetType().GetProperty("IsActive");
                var updatedAtProperty = entity.GetType().GetProperty("UpdatedAt");

                if (isDeletedProperty != null)
                    isDeletedProperty.SetValue(entity, false);
                if (deletedAtProperty != null)
                    deletedAtProperty.SetValue(entity, null);
                if (deletedByProperty != null)
                    deletedByProperty.SetValue(entity, null);
                if (deleteReasonProperty != null)
                    deleteReasonProperty.SetValue(entity, null);
                if (isActiveProperty != null)
                    isActiveProperty.SetValue(entity, true);
                if (updatedAtProperty != null)
                    updatedAtProperty.SetValue(entity, DateTime.Now);
            }

            return true;
        }

        /// <summary>
        /// 🗑️ حذف نهائي (Hard Delete)
        /// </summary>
        public static async Task<bool> HardDeleteAsync<T>(this DbSet<T> dbSet, int id)
            where T : class, ISoftDeleteEntity
        {
            var entity = await dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null)
                return false;

            dbSet.Remove(entity);
            return true;
        }

        /// <summary>
        /// 🗑️🗑️ حذف نهائي لمجموعة من السجلات
        /// </summary>
        public static async Task<int> HardDeleteRangeAsync<T>(this DbSet<T> dbSet, Expression<Func<T, bool>> predicate)
            where T : class, ISoftDeleteEntity
        {
            var entities = await dbSet.IgnoreQueryFilters().Where(predicate).ToListAsync();
            if (!entities.Any())
                return 0;

            dbSet.RemoveRange(entities);
            return entities.Count;
        }
    }
}