namespace CelebRateApi.Models
{
    public class CelebTranslation
    {
        public int CelebId { get; set; }
        public Celeb Celeb { get; set; } = null!;
        public int LanguageId { get; set; }
        public Language Language { get; set; } = null!;

        // Needs reconsideration
        public int CaregoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
