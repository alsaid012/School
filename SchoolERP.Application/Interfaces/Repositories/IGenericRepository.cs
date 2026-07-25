using System.Linq.Expressions;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 🏗️  الواجهة العامة للمستودع (IGenericRepository)
    /// 📌  الوظيفة: تعريف العمليات الأساسية (CRUD) لجميع الكيانات
    /// 🔄  الاستخدام: تستخدم في طبقة الخدمات (Services)
    /// 📦  الميزة: فصل العقد عن التنفيذ (Contracts vs Implementation)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    /// <typeparam name="T">نوع الكيان (Entity)</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        #region ════════════════════════════════════ عمليات القراءة ════════════════════════════════════

        /// <summary>
        /// 🔍 الحصول على كيان بواسطة المعرف (ID)
        /// </summary>
        /// <param name="id">المعرف (Primary Key)</param>
        /// <returns>الكيان المطلوب أو null إذا لم يوجد</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// 📋 الحصول على جميع الكيانات
        /// </summary>
        /// <returns>قائمة بجميع الكيانات</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// 🔎 البحث عن كيانات حسب شرط معين
        /// </summary>
        /// <param name="predicate">شرط البحث (Lambda Expression)</param>
        /// <returns>قائمة الكيانات المطابقة للشرط</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// ✅ التحقق من وجود كيان يحقق شرط معين
        /// </summary>
        /// <param name="predicate">شرط البحث</param>
        /// <returns>true إذا وجد، false إذا لم يوجد</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 📊 حساب عدد الكيانات (مع أو بدون شرط)
        /// </summary>
        /// <param name="predicate">شرط البحث (اختياري)</param>
        /// <returns>عدد الكيانات</returns>
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        #endregion

        #region ════════════════════════════════════ دوال عامة مشتركة ════════════════════════════════════
        //=================================================
        /// <summary>
        /// 🔍 البحث عن كيان بواسطة الكود (إذا كان الكيان يدعم Code)
        /// </summary>
        Task<T?> GetByCodeAsync(string code);

        /// <summary>
        /// 📋 الحصول على كيان مع جميع البيانات المرتبطة (Includes)
        /// </summary>
        Task<T?> GetWithDetailsAsync(int id);

        /// <summary>
        /// ✅ التحقق من وجود كيان بنفس الاسم
        /// </summary>
        Task<bool> IsNameExistsAsync(string name, int? excludeId = null);
        //Task<bool> IsNameExistsAsync(int id,string name, int? excludeId = null);

        /// <summary>
        /// ✅ التحقق من وجود كيان بنفس الكود
        /// </summary>
        Task<bool> IsCodeExistsAsync(string code, int? excludeId = null);
        //=================================================


        /// <summary>
        /// 📋 جلب جميع الكيانات مع Includes
        /// </summary>
        Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);

        #endregion

        #region ════════════════════════════════════ عمليات الإضافة ════════════════════════════════════

        /// <summary>
        /// ➕ إضافة كيان جديد
        /// </summary>
        /// <param name="entity">الكيان المراد إضافته</param>
        /// <returns>الكيان بعد الإضافة (مع المعرف الجديد)</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// ➕➕ إضافة مجموعة من الكيانات
        /// </summary>
        /// <param name="entities">قائمة الكيانات المراد إضافتها</param>
        /// <returns>قائمة الكيانات بعد الإضافة</returns>
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        #endregion

        #region ════════════════════════════════════ عمليات التحديث ════════════════════════════════════

        /// <summary>
        /// ✏️ تحديث كيان موجود
        /// </summary>
        /// <param name="entity">الكيان المراد تحديثه</param>
        Task UpdateAsync(T entity);

        #endregion

        #region ════════════════════════════════════ عمليات الحذف ════════════════════════════════════

        /// <summary>
        /// 🗑️ حذف كيان (Hard Delete - حذف نهائي)
        /// </summary>
        /// <param name="entity">الكيان المراد حذفه</param>
        Task DeleteAsync(T entity);

        /// <summary>
        /// 🗑️🗑️ حذف مجموعة من الكيانات (Hard Delete)
        /// </summary>
        /// <param name="entities">قائمة الكيانات المراد حذفها</param>
        Task DeleteRangeAsync(IEnumerable<T> entities);

        #endregion

        /// <summary>
        /// 📋 الحصول على جميع السجلات مع البيانات المرتبطة
        /// </summary>
        Task<IEnumerable<T>> GetAllWithDetailsAsync(params Expression<Func<T, object>>[] includes);

    }
}