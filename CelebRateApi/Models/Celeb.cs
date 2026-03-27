namespace CelebRateApi.Models
{
    public class Celeb
    {
        public int CelebId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public float Stars { get; set; }

        public ICollection<CelebCategory> CelebCategories { get; set; } = [];
        public ICollection<CelebTag> CelebTags { get; set; } = [];
        public ICollection<CelebTranslation> CelebTranslations { get; set; } = [];
    }
}
