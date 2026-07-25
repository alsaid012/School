using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class TeacherSubjectConfiguration : IEntityTypeConfiguration<TeacherSubject>
    {
        public void Configure(EntityTypeBuilder<TeacherSubject> builder)
        {
            builder.HasKey(ts => ts.Id);
            
            // العلاقة مع Teacher (Many-to-One)
            builder.HasOne(ts => ts.Teacher)
                .WithMany(t => t.TeacherSubjects)
                .HasForeignKey(ts => ts.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // العلاقة مع Subject (Many-to-One)
            builder.HasOne(ts => ts.Subject)
                .WithMany(s => s.TeacherSubjects)
                .HasForeignKey(ts => ts.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // منع تكرار نفس المعلم لنفس المادة
            builder.HasIndex(ts => new { ts.TeacherId, ts.SubjectId })
                .IsUnique()
                .HasDatabaseName("IX_TeacherSubject_Teacher_Subject");
        }
    }
}