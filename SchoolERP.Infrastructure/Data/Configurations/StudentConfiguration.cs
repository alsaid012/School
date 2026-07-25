using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);
            
            builder.Property(s => s.StudentCode)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.HasIndex(s => s.StudentCode)
                .IsUnique();
                
            builder.Property(s => s.ParentName)
                .HasMaxLength(100);
                
            builder.Property(s => s.ParentPhone)
                .HasMaxLength(20);
                
            builder.Property(s => s.ParentEmail)
                .HasMaxLength(100);
            
            
            builder.HasOne(s => s.User)
                .WithMany(u => u.Students)  
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasOne(s => s.AcademicYear)
                .WithMany(ay => ay.Students)
                .HasForeignKey(s => s.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(s => s.ClassRoom)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.ClassRoomId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}