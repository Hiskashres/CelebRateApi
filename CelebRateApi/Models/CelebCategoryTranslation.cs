namespace CelebRateApi.Models
{
    public class CelebCategoryTranslation
    {
        public int CelebId { get; set; }
        public int CategoryId { get; set; }

        // Needs reconsideration
        public int LanguageId { get; set; }
    }
}
