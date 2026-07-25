using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.HasKey(s => s.Id);
            
            builder.Property(s => s.SubjectName)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(s => s.SubjectCode)
                .HasMaxLength(20);
                
            builder.HasIndex(s => s.SubjectCode)
                .IsUnique()
                .HasDatabaseName("IX_Subject_Code");
                
            builder.Property(s => s.Description)
                .HasMaxLength(500);
            
            // العلاقة مع GradeLevel (Many-to-One)
            builder.HasOne(s => s.GradeLevel)
                .WithMany(g => g.Subjects)
                .HasForeignKey(s => s.GradeLevelId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // منع تكرار نفس المادة لنفس الصف
            builder.HasIndex(s => new { s.GradeLevelId, s.SubjectName })
                .IsUnique()
                .HasDatabaseName("IX_Subject_GradeLevel_SubjectName");
            
          
        }
    }
}