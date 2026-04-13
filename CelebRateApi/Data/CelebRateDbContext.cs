using CelebRateApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CelebRateApi.Data
{
    public class CelebRateDbContext(DbContextOptions<CelebRateDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Celeb> Celebs => Set<Celeb>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Rate> Rates => Set<Rate>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<Language> Languages => Set<Language>();
        public DbSet<CelebCategory> CelebCategories => Set<CelebCategory>();
        public DbSet<CelebTranslation> CelebTranslations => Set<CelebTranslation>();
        public DbSet<CategoryTranslation> CategoryTranslations => Set<CategoryTranslation>();
        public DbSet<TagTranslation> TagTranslations => Set<TagTranslation>();
        public DbSet<CelebCategoryTranslation> CelebCategoryTranslations => Set<CelebCategoryTranslation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite keys
            modelBuilder.Entity<Rate>().HasKey(r => new { r.ApplicationUserId, r.CelebId, r.CategoryId });
            modelBuilder.Entity<CelebCategory>().HasKey(sc => new { sc.CelebId, sc.CategoryId });
            modelBuilder.Entity<CategoryTranslation>().HasKey(ct => new { ct.CategoryId, ct.LanguageId });
            modelBuilder.Entity<CelebCategoryTranslation>().HasKey(cct => new { cct.LanguageId, cct.CelebId, cct.CategoryId });
            modelBuilder.Entity<CelebTranslation>().HasKey(ct => new { ct.CelebId, ct.LanguageId });
            modelBuilder.Entity<TagTranslation>().HasKey(tt => new { tt.TagId, tt.LanguageId });

            // Relationships

            modelBuilder.Entity<CelebCategory>()
                .HasOne(cc => cc.Celeb)
                .WithMany(c => c.CelebCategories)
                .HasForeignKey(cc => cc.CelebId);

            modelBuilder.Entity<CelebCategory>()
                .HasOne(cc => cc.Category)
                .WithMany(c => c.CelebCategories)
                .HasForeignKey(cc => cc.CategoryId);

            modelBuilder.Entity<Rate>()
                .HasOne(r => r.ApplicationUser)
                .WithMany(u => u.Rates)
                .HasForeignKey(r => r.ApplicationUserId);

            modelBuilder.Entity<Rate>()
               .HasOne<CelebCategory>()
               .WithMany(cc => cc.Rates)
               .HasForeignKey(r => new { r.CelebId, r.CategoryId })
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CategoryTranslation>()
               .HasOne(ct => ct.Category)
               .WithMany(c => c.CategoryTranslations)
               .HasForeignKey(ct => ct.CategoryId);

            modelBuilder.Entity<CategoryTranslation>()
                .HasOne(ct => ct.Language)
                .WithMany(l => l.CategoryTranslations)
                .HasForeignKey(ct => ct.LanguageId);

            modelBuilder.Entity<CelebCategoryTranslation>()
               .HasOne(cct => cct.Language)
               .WithMany(l => l.CelebCategoryTranslations)
               .HasForeignKey(cct => cct.LanguageId);

            modelBuilder.Entity<CelebCategoryTranslation>()
                .HasOne(cct => cct.CelebCategory)
                .WithMany(cc => cc.CelebCategoryTranslations)
                .HasForeignKey(cct => new { cct.CelebId, cct.CategoryId });

            modelBuilder.Entity<CelebTranslation>()
               .HasOne(ct => ct.Celeb)
               .WithMany(c => c.CelebTranslations)
               .HasForeignKey(c => c.CelebId);

            modelBuilder.Entity<CelebTranslation>()
                .HasOne(ct => ct.Language)
                .WithMany(l => l.CelebTranslations)
                .HasForeignKey(ct => ct.LanguageId);

            modelBuilder.Entity<TagTranslation>()
               .HasOne(tt => tt.Tag)
               .WithMany(t => t.TagTranslations)
               .HasForeignKey(tt => tt.TagId);

            modelBuilder.Entity<CelebTranslation>()
                .HasOne(tt => tt.Language)
                .WithMany(l => l.CelebTranslations)
                .HasForeignKey(tt => tt.LanguageId);
        }
    }
}
