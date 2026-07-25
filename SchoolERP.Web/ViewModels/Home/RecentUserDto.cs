namespace SchoolERP.Web.ViewModels.Home
{
    public class RecentUserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string UserType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}