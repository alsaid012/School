using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class ClassRoomConfiguration : IEntityTypeConfiguration<ClassRoom>
    {
        public void Configure(EntityTypeBuilder<ClassRoom> builder)
        {
            builder.HasKey(c => c.Id);
            
            builder.Property(c => c.ClassName)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(c => c.ClassCode)
                .HasMaxLength(20);
                
            builder.HasIndex(c => c.ClassCode)
                .IsUnique();
                
            builder.Property(c => c.RoomNumber)
                .HasMaxLength(20);
                
            builder.Property(c => c.Notes)
                .HasMaxLength(500);
                
            // العلاقات
            builder.HasOne(c => c.GradeLevel)
                .WithMany(g => g.ClassRooms)
                .HasForeignKey(c => c.GradeLevelId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(c => c.Teacher)
                .WithMany(t => t.ClassRooms)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);
                
          
                
            
        }
    }
}