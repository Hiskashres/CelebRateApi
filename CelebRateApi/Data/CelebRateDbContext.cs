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
        public DbSet<CelebTag> CelebsTags => Set<CelebTag>();
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
            modelBuilder.Entity<CelebTag>().HasKey(ct => new { ct.CelebId, ct.TagId });
            modelBuilder.Entity<CelebCategory>().HasKey(sc => new { sc.CelebId, sc.CategoryId });
            modelBuilder.Entity<CelebLanguage>().HasKey(sl => new { sl.CelebId, sl.LanguageId });

            // Relationships

            //modelBuilder.Entity<CelebCategory>()
            //    .HasOne(cc => cc.Celeb)
            //    .WithMany(c => c.CelebCategories)
            //    .HasForeignKey(cc => cc.CelebId);

            //modelBuilder.Entity<CelebCategory>()
            //    .HasOne(cc => cc.Category)
            //    .WithMany(c => c.CelebCategories)
            //    .HasForeignKey(cc => cc.CategoryId);

            //modelBuilder.Entity<Rate>()
            //    .HasOne(r => r.ApplicationUser)
            //    .WithMany(u => u.Rates)
            //    .HasForeignKey(r => r.UserId);

            //modelBuilder.Entity<Rate>()
            //   .HasOne<CelebCategory>()
            //   .WithMany(cc => cc.Rates)
            //   .HasForeignKey(r => new { r.CelebId, r.CategoryId });

            //modelBuilder.Entity<CelebTag>()
            //   .HasOne(c => c.Celeb)
            //   .WithMany(ct => ct.CelebTags)
            //   .HasForeignKey(c => c.CelebId);

            //modelBuilder.Entity<CelebTag>()
            //    .HasOne(ct => ct.Tag)
            //    .WithMany(t => t.CelebTags)
            //    .HasForeignKey(ct => ct.TagId);

            //modelBuilder.Entity<CelebLanguage>()
            //   .HasOne(c => c.Celeb)
            //   .WithMany(cl => cl.CelebLanguages)
            //   .HasForeignKey(c => c.CelebId);

            //modelBuilder.Entity<CelebLanguage>()
            //    .HasOne(cl => cl.Language)
            //    .WithMany(l => l.CelebLanguages)
            //    .HasForeignKey(cl => cl.LanguageId);

            //modelBuilder.Entity<Language>()
            //        .HasMany(l => l.ApplicationUsers)
            //        .WithOne(au => au.Language)
            //        .HasForeignKey(au => au.LanguageId);
        }
    }
}
