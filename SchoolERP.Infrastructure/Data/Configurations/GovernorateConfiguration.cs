using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class GovernorateConfiguration : IEntityTypeConfiguration<Governorate>
    {
        public void Configure(EntityTypeBuilder<Governorate> builder)
        {
            builder.HasKey(g => g.Id);
            
            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(g => g.Code)
                .HasMaxLength(20);
                
            builder.HasIndex(g => g.Code)
                .IsUnique()
                .HasDatabaseName("IX_Governorate_Code");
            
            
        }
    }
}