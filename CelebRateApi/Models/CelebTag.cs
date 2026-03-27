namespace CelebRateApi.Models
{
    public class CelebTag
    {
        public int CelebId { get; set; }
        public Celeb Celeb { get; set; } = null!;
        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;


        // Needs reconsideration
        public int Category { get; set; }
        public CelebCategory CelebCategory { get; set; } = null!;

        public int Order { get; set; }
    }
}
