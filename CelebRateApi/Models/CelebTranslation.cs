namespace CelebRateApi.Models
{
    public class CelebTranslation
    {
        public int CelebId { get; set; }
        public Celeb Celeb { get; set; } = null!;
        public int LanguageId { get; set; }
        public Language Language { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
