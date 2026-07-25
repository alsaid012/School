using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class EmployeeAttendanceConfiguration : IEntityTypeConfiguration<EmployeeAttendance>
    {
        public void Configure(EntityTypeBuilder<EmployeeAttendance> builder)
        {
            builder.HasKey(ea => ea.Id);
            
            builder.Property(ea => ea.Status)
                .HasMaxLength(20);
                
            builder.Property(ea => ea.Notes)
                .HasMaxLength(500);
            
            // العلاقة مع Employee (Many-to-One)
            builder.HasOne(ea => ea.Employee)
                .WithMany(e => e.Attendances)
                .HasForeignKey(ea => ea.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // منع تسجيل حضور مكرر لنفس الموظف في نفس اليوم
            builder.HasIndex(ea => new { ea.EmployeeId, ea.AttendanceDate })
                .IsUnique()
                .HasDatabaseName("IX_EmployeeAttendance_Employee_Date");
        }
    }
}