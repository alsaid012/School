using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);
            
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(d => d.Code)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.HasIndex(d => d.Code)
                .IsUnique()
                .HasDatabaseName("IX_Department_Code");
                
            builder.Property(d => d.DirectorName)
                .HasMaxLength(100);
                
            builder.Property(d => d.Phone)
                .HasMaxLength(20);
                
            builder.Property(d => d.Email)
                .HasMaxLength(100);
                
            builder.Property(d => d.Address)
                .HasMaxLength(500);
            
           
            builder.HasOne(d => d.Governorate)
                .WithMany(g => g.Departments)
                .HasForeignKey(d => d.GovernorateId)
                .OnDelete(DeleteBehavior.Restrict);
            
           
        }
    }
}