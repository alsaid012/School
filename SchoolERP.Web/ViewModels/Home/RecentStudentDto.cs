namespace SchoolERP.Web.ViewModels.Home
{
    public class RecentStudentDto
    {
        public int Id { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? AcademicYear { get; set; }
    }
}