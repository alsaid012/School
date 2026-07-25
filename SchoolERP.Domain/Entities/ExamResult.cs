namespace SchoolERP.Domain.Entities
{
    public class ExamResult : BaseEntity
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public int Score { get; set; }
        public string? Grade { get; set; }
        public decimal? Percentage { get; set; }
        public string? Remarks { get; set; }
        
        // Navigation Properties
        public virtual Exam Exam { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}