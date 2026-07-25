using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class StudentAttendanceConfiguration : IEntityTypeConfiguration<StudentAttendance>
    {
        public void Configure(EntityTypeBuilder<StudentAttendance> builder)
        {
            builder.HasKey(sa => sa.Id);
            
            builder.Property(sa => sa.Status)
                .HasMaxLength(20);
                
            builder.Property(sa => sa.Notes)
                .HasMaxLength(500);
            
            // العلاقة مع Student (Many-to-One)
            builder.HasOne(sa => sa.Student)
                .WithMany(s => s.Attendances)
                .HasForeignKey(sa => sa.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // منع تسجيل حضور مكرر لنفس الطالب في نفس اليوم
            builder.HasIndex(sa => new { sa.StudentId, sa.AttendanceDate })
                .IsUnique()
                .HasDatabaseName("IX_StudentAttendance_Student_Date");
        }
    }
}