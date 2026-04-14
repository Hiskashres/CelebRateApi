namespace CelebRateApi.DTOs
{
    public class CelebDTO
    {
        public int CelebId { get; set; }
        public int LanguageId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
