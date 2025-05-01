using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Text.Json;
using WEB.Models;

namespace WEB.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Template> Templates { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<FormResponse> Responses { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships
            builder.Entity<Template>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.HasMany(t => t.Questions)
                      .WithOne(q => q.Template)
                      .HasForeignKey(q => q.TemplateId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.Owner)
                      .WithMany()
                      .HasForeignKey(t => t.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Question>(entity =>
            {
                entity.Property(q => q.Options)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                        v => JsonSerializer.Deserialize<List<string>>(v, JsonSerializerOptions.Default))
                    .HasColumnType("jsonb"); // Только для PostgreSQL
            });
            builder.Entity<FormResponse>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.HasMany(r => r.Answers)
                      .WithOne()
                      .HasForeignKey(a => a.ResponseId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Template)
                      .WithMany()
                      .HasForeignKey(r => r.TemplateId);

                entity.HasOne(r => r.Respondent)
                      .WithMany()
                      .HasForeignKey(r => r.RespondentId);
            });

            builder.Entity<Answer>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.HasOne(a => a.Question)
                      .WithMany()
                      .HasForeignKey(a => a.QuestionId);
            });

            // For PostgreSQL specific configurations
            if (Database.IsNpgsql())
            {
                // Enable case-insensitive text search
                builder.UseCollation("case_insensitive");

                // Configure JSON serialization for question options
                builder.Entity<Question>()
                    .Property(q => q.Options)
                    .HasColumnType("jsonb");
            }
        }
    }
}