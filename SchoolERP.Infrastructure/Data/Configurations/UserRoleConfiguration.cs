using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(ur => ur.Id);
            
            builder.Property(ur => ur.Notes)
                .HasMaxLength(500);
            
            // العلاقة مع User (Many-to-One)
            builder.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // منع تكرار نفس الدور لنفس المستخدم
            builder.HasIndex(ur => new { ur.UserId, ur.RoleType })
                .IsUnique()
                .HasDatabaseName("IX_UserRole_User_Role");
        }
    }
}