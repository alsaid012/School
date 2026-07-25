using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class UserContactConfiguration : IEntityTypeConfiguration<UserContact>
    {
        public void Configure(EntityTypeBuilder<UserContact> builder)
        {
            builder.HasKey(uc => uc.Id);
            
            builder.Property(uc => uc.ContactValue)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(uc => uc.Notes)
                .HasMaxLength(500);
            
            // العلاقة مع User (Many-to-One)
            builder.HasOne(uc => uc.User)
                .WithMany(u => u.Contacts)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // منع تكرار نفس جهة الاتصال لنفس المستخدم
            builder.HasIndex(uc => new { uc.UserId, uc.ContactType, uc.ContactValue })
                .IsUnique()
                .HasDatabaseName("IX_UserContact_User_Type_Value");
        }
    }
}