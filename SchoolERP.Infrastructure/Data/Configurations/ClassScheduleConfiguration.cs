using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class ClassScheduleConfiguration : IEntityTypeConfiguration<ClassSchedule>
    {
        public void Configure(EntityTypeBuilder<ClassSchedule> builder)
        {
            builder.HasKey(cs => cs.Id);
            
            builder.Property(cs => cs.Notes)
                .HasMaxLength(500);
                
            // Unique constraint to prevent conflicts
            builder.HasIndex(cs => new { cs.ClassRoomId, cs.DayOfWeek, cs.PeriodNumber, cs.AcademicYearId })
                .IsUnique();
                
            // Relationships
            builder.HasOne(cs => cs.AcademicYear)
                .WithMany(ay => ay.Schedules)
                .HasForeignKey(cs => cs.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(cs => cs.ClassRoom)
                .WithMany(c => c.Schedules)
                .HasForeignKey(cs => cs.ClassRoomId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(cs => cs.Subject)
                .WithMany(s => s.Schedules)
                .HasForeignKey(cs => cs.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(cs => cs.Teacher)
                .WithMany(t => t.Schedules)
                .HasForeignKey(cs => cs.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ التحقق من أن وقت البداية أقل من وقت النهاية
            builder.ToTable(tb => tb.HasCheckConstraint(
                "CK_ClassSchedule_TimeRange",
                "StartTime < EndTime"));

        }
    }
}