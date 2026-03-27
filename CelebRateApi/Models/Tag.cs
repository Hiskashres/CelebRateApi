namespace CelebRateApi.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string TagName { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<CelebTag> CelebTags { get; set; } = [];
        public ICollection<TagTranslation> TagTranslations { get; set; } = [];

    }
}
