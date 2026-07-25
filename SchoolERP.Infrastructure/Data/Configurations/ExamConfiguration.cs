using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.ExamName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Notes)
                .HasMaxLength(500);

            // ✅ إضافة IsActive
            builder.Property(e => e.IsActive)
                .HasDefaultValue(true);

            // ✅ العلاقة مع AcademicYear
            builder.HasOne(e => e.AcademicYear)
                .WithMany(ay => ay.Exams)
                .HasForeignKey(e => e.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ العلاقة مع Subject
            builder.HasOne(e => e.Subject)
                .WithMany(s => s.Exams)
                .HasForeignKey(e => e.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ العلاقة مع ClassRoom
            builder.HasOne(e => e.ClassRoom)
                .WithMany(c => c.Exams)
                .HasForeignKey(e => e.ClassRoomId)
                .OnDelete(DeleteBehavior.SetNull);

            // ✅ العلاقة مع Teacher
            builder.HasOne(e => e.Teacher)
                .WithMany(t => t.Exams)
                .HasForeignKey(e => e.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);

            // ✅ فهارس لتحسين الأداء
            builder.HasIndex(e => e.AcademicYearId);
            builder.HasIndex(e => e.SubjectId);
            builder.HasIndex(e => e.ClassRoomId);
            builder.HasIndex(e => e.TeacherId);
            builder.HasIndex(e => e.ExamDate);
        }
    }
}