using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class AcademicYearConfiguration : IEntityTypeConfiguration<AcademicYear>
    {
        public void Configure(EntityTypeBuilder<AcademicYear> builder)
        {
            builder.HasKey(ay => ay.Id);
            
            builder.Property(ay => ay.YearName)
                .IsRequired()
                .HasMaxLength(20);
            
            // العلاقة مع School (Many-to-One)
            builder.HasOne(ay => ay.School)
                .WithMany(s => s.AcademicYears)
                .HasForeignKey(ay => ay.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // منع تكرار نفس العام الدراسي لنفس المدرسة
            builder.HasIndex(ay => new { ay.SchoolId, ay.YearName })
                .IsUnique()
                .HasDatabaseName("IX_AcademicYear_School_Year");
            
            
        }
    }
}