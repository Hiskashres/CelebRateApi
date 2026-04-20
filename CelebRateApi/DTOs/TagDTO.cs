namespace CelebRateApi.DTOs
{
    public class TagDTO
    {
        public int TagId { get; set; }
        public int CategoryId { get; set; }
        public int LanguageId { get; set; }
        public string TagName { get; set; } = null!;
    }
}
