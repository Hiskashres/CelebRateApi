namespace CelebRateApi.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<TagTranslation> TagTranslations { get; set; } = [];

    }
}
