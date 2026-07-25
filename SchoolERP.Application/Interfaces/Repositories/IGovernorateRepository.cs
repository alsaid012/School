using SchoolERP.Domain.Entities;

namespace SchoolERP.Application.Interfaces.Repositories
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📍  واجهة مستودع المحافظات (IGovernorateRepository)
    /// 📌  الوظيفة: تعريف العمليات الخاصة بالمحافظات
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public interface IGovernorateRepository : IGenericRepository<Governorate>
    {
        ///// <summary>
        ///// 🔍 البحث عن محافظة بواسطة الكود
        ///// </summary>
        //Task<Governorate?> GetByCodeAsync(string code);

        /// <summary>
        /// 📋 الحصول على جميع المحافظات مع الإدارات التابعة لها
        /// </summary>
        Task<IEnumerable<Governorate>> GetAllWithDepartmentsAsync();

        ///// <summary>
        ///// ✅ التحقق من وجود محافظة بنفس الاسم
        ///// </summary>
        //Task<bool> IsNameExistsAsync(string name, int? excludeId = null);

        ///// <summary>
        ///// ✅ التحقق من وجود محافظة بنفس الكود
        ///// </summary>
        //Task<bool> IsCodeExistsAsync(string code, int? excludeId = null);
    }
}