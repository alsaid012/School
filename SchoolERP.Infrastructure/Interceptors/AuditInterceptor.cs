using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SchoolERP.Domain.Interfaces;

namespace SchoolERP.Infrastructure.Interceptors
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📝  Interceptor لتسجيل Audit (من أنشأ ومتى)
    /// 📌  الوظيفة: تعيين CreatedAt, UpdatedAt, CreatedBy, UpdatedBy تلقائياً
    /// 🔄  يعمل قبل حفظ البيانات في قاعدة البيانات
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly string _currentUser;

        /// <summary>
        /// المُنشئ - يستقبل اسم المستخدم الحالي
        /// </summary>
        /// <param name="currentUser">اسم المستخدم الحالي (من الـ Token أو الـ Context)</param>
        public AuditInterceptor(string currentUser = "System")
        {
            _currentUser = currentUser;
        }

        /// <summary>
        /// 🔄 يتم استدعاؤها قبل حفظ التغييرات (متزامن)
        /// </summary>
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            UpdateAuditFields(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        /// <summary>
        /// 🔄 يتم استدعاؤها قبل حفظ التغييرات (غير متزامن)
        /// </summary>
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            UpdateAuditFields(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        /// <summary>
        /// ✏️ تحديث حقول Audit للكيانات التي تطبق IAuditableEntity
        /// </summary>
        private void UpdateAuditFields(DbContext? context)
        {
            if (context == null) return;

            var entries = context.ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                // ✅ للكيانات التي تطبق IAuditableEntity
                if (entry.Entity is IAuditableEntity entity)
                {
                    var now = DateTime.Now;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = now;
                        entity.CreatedBy = _currentUser;
                        entity.IsActive = true;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entity.UpdatedAt = now;
                        entity.UpdatedBy = _currentUser;
                    }
                }

                // ✅ للكيانات التي تطبق ISoftDeleteEntity
                if (entry.Entity is ISoftDeleteEntity softDeleteEntity)
                {
                    if (entry.State == EntityState.Modified && softDeleteEntity.IsDeleted)
                    {
                        softDeleteEntity.DeletedAt = DateTime.Now;
                        softDeleteEntity.DeletedBy = _currentUser;
                    }
                }
            }
        }
    }
}