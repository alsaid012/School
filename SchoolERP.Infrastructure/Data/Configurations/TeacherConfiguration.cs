using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(t => t.Id);
            
            builder.Property(t => t.TeacherCode)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.HasIndex(t => t.TeacherCode)
                .IsUnique();
                
            builder.Property(t => t.Qualification)
                .HasMaxLength(200);
                
            builder.Property(t => t.Specialization)
                .HasMaxLength(200);
                
            builder.Property(t => t.Salary)
                .HasPrecision(18, 2);
            
           
            builder.HasOne(t => t.User)
                .WithMany(u => u.Teachers)  
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
           
        }
    }
}