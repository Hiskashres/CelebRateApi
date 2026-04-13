namespace CelebRateApi.Models
{
    public class Celeb
    {
        public int CelebId { get; set; }
        public float Stars { get; set; }

        public ICollection<CelebCategory> CelebCategories { get; set; } = [];
        public ICollection<CelebTranslation> CelebTranslations { get; set; } = [];
    }
}
