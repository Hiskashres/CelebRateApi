namespace CelebRateApi.Models
{
    public class Rate
    {
        public string UserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public int CelebId { get; set; }
        public int CategoryId { get; set; }

        public int Rate1 { get; set; }
        public int Rate2 { get; set; }
        public int Rate3 { get; set; }
    }
}
