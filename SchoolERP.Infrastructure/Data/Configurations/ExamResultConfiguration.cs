using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Infrastructure.Data.Configurations
{
    public class ExamResultConfiguration : IEntityTypeConfiguration<ExamResult>
    {
        public void Configure(EntityTypeBuilder<ExamResult> builder)
        {
            builder.HasKey(er => er.Id);
            
            builder.Property(er => er.Grade)
                .HasMaxLength(5);
                
            builder.Property(er => er.Remarks)
                .HasMaxLength(500);
                
            builder.Property(er => er.Percentage)
                .HasPrecision(5, 2);
            
            // العلاقة مع Exam (Many-to-One)
            builder.HasOne(er => er.Exam)
                .WithMany(e => e.Results)
                .HasForeignKey(er => er.ExamId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // العلاقة مع Student (Many-to-One)
            builder.HasOne(er => er.Student)
                .WithMany(s => s.ExamResults)
                .HasForeignKey(er => er.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // منع تكرار نفس الطالب لنفس الامتحان
            builder.HasIndex(er => new { er.ExamId, er.StudentId })
                .IsUnique()
                .HasDatabaseName("IX_ExamResult_Exam_Student");
        }
    }
}