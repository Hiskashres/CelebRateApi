namespace CelebRateApi.Models
{
    public class CelebCategoryTranslation
    {
        public int CelebId { get; set; }
        public int CategoryId { get; set; }
        public int LanguageId { get; set; }

        public Language Language { get; set; } = null!;
        public CelebCategory CelebCategory { get; set; } = null!;
        public CategoryTranslation CategoryTranslation { get; set; } = null!;

        public string Specialty1 { get; set; } = null!;
        public string Specialty2 { get; set; } = null!;
        public string Specialty3 { get; set; } = null!;
    }
}
