using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(u => u.Username)
                .IsUnique();

            builder.Property(u => u.NationalId)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(u => u.NationalId)
                .IsUnique();

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(u => u.Email)
              .HasMaxLength(100);

            builder.Property(u => u.Address)
                .HasMaxLength(500);

            builder.Property(u => u.ProfilePicture)
                .HasMaxLength(255);
            builder.Property(u => u.Gender)
               .HasMaxLength(10);


            builder.HasOne(u => u.School)
                .WithMany(s => s.Users)
                .HasForeignKey(u => u.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);

        
        }
    }
}