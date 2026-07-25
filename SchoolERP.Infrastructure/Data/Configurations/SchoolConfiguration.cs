using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class SchoolConfiguration : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder.HasKey(s => s.Id);
            
            builder.Property(s => s.SchoolName)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(s => s.SchoolCode)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.HasIndex(s => s.SchoolCode)
                .IsUnique();
                
            builder.Property(s => s.Address)
                .HasMaxLength(500);
                
            builder.Property(s => s.Phone)
                .HasMaxLength(20);
                
            builder.Property(s => s.Email)
                .HasMaxLength(100);
                
            builder.Property(s => s.PrincipalName)
                .HasMaxLength(100);
                
            // Relationships
            builder.HasOne(s => s.Department)
                .WithMany(d => d.Schools)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}