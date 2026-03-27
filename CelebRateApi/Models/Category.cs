using Azure;

namespace CelebRateApi.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public int Order { get; set; }

        public ICollection<CelebCategory> CelebCategories { get; set; } = [];
        public ICollection<Tag> Tags { get; set; } = [];
        public ICollection<CategoryTranslation> CategoryTranslations { get; set; } = [];
    }
}
