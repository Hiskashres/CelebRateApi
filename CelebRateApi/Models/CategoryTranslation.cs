namespace CelebRateApi.Models
{
    public class CategoryTranslation
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int LanguageId { get; set; }
        public Language Language { get; set; } = null!;

        public string CategoryName { get; set; } = null!;
        public string Description { get; set; } = null!;

    }
}
