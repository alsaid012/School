namespace SchoolERP.Application.DTOs.Common
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 📄  نموذج الترقيم (Pagination DTO)
    /// 📌  الوظيفة: نقل بيانات الترقيم من العميل إلى الخادم
    /// 📦  الاستخدام: في جميع عمليات جلب القوائم
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class PaginationDto
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;
        private const int MaxPageSize = 100;

        /// <summary>
        /// رقم الصفحة الحالية (يبدأ من 1)
        /// </summary>
        /// <example>1</example>
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        /// <summary>
        /// عدد العناصر في كل صفحة
        /// </summary>
        /// <example>10</example>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 10 : (value > MaxPageSize ? MaxPageSize : value);
        }

        /// <summary>
        /// النص للبحث (اختياري)
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// حقل الترتيب
        /// </summary>
        /// <example>FullName</example>
        public string? SortBy { get; set; }

        /// <summary>
        /// اتجاه الترتيب (ASC / DESC)
        /// </summary>
        /// <example>ASC</example>
        public string? SortDirection { get; set; } = "ASC";

        /// <summary>
        /// حساب عدد العناصر التي سيتم تخطيها
        /// </summary>
        public int Skip => (PageNumber - 1) * PageSize;
    }

    /// <summary>
    /// 📄  نموذج نتيجة الترقيم (PagedResult DTO)
    /// 📌  الوظيفة: نقل البيانات مع معلومات الترقيم
    /// </summary>
    public class PagedResultDto<T>
    {
        /// <summary>
        /// قائمة العناصر في الصفحة الحالية
        /// </summary>
        public List<T> Items { get; set; } = new();

        /// <summary>
        /// إجمالي عدد العناصر
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// رقم الصفحة الحالية
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// حجم الصفحة
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// إجمالي عدد الصفحات
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        /// <summary>
        /// هل توجد صفحة سابقة؟
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// هل توجد صفحة تالية؟
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;
    }
}