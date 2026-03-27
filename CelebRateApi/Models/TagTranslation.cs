namespace CelebRateApi.Models
{
    public class TagTranslation
    {
        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;

        public int LanguageId { get; set; }
        public Language Language { get; set; } = null!;

        public string TagName { get; set; } = null!;
    }
}
