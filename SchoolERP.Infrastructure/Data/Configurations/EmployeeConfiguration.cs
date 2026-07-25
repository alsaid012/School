using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.EmployeeCode)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.HasIndex(e => e.EmployeeCode)
                .IsUnique();
                
            builder.Property(e => e.JobTitle)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(e => e.Department)
                .HasMaxLength(100);
                
            builder.Property(e => e.Salary)
                .HasPrecision(18, 2);
            
           
            builder.HasOne(e => e.User)
                .WithMany(u => u.Employees)  
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}