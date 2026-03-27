using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CelebRateApi.Models
{
    public class CelebCategory
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int CelebId { get; set; }
        public Celeb Celeb { get; set; } = null!;

        public int Order { get; set; }
       
        public ICollection<Rate> Rates { get; set; } = [];
        public ICollection<CelebCategoryTranslation> CelebCategoryTranslations { get; set; } = [];

        // Needs reconsideration
        public ICollection<CelebTag> CelebTags { get; set; } = [];
    }
}
