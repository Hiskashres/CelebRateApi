namespace CelebRateApi.Models
{
    public class Language
    {
        public int LanguageId { get; set; }
        public string ShortName { get; set; } = null!;
        public string FullName { get; set; } = null!;

        public ICollection<ApplicationUser> ApplicationUsers { get; set; } = [];
        public ICollection<CelebTranslation> CelebTranslations { get; set; } = [];
        public ICollection<CelebCategoryTranslation> CelebCategoryTranslations { get; set; } = [];
        public ICollection<CategoryTranslation> CategoryTranslations { get; set; } = [];
        public ICollection<TagTranslation> TagTranslations { get; set; } = [];

    }
}
