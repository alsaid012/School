using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class GradeLevelConfiguration : IEntityTypeConfiguration<GradeLevel>
    {
        public void Configure(EntityTypeBuilder<GradeLevel> builder)
        {
            builder.HasKey(g => g.Id);
            
            builder.Property(g => g.GradeName)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(g => g.Description)
                .HasMaxLength(500);
            
            // العلاقة مع School (Many-to-One)
            builder.HasOne(g => g.School)
                .WithMany(s => s.GradeLevels)
                .HasForeignKey(g => g.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // منع تكرار نفس الصف لنفس المدرسة
            builder.HasIndex(g => new { g.SchoolId, g.GradeNumber, g.GradeStage })
                .IsUnique()
                .HasDatabaseName("IX_GradeLevel_School_Grade");
           
        }
    }
}