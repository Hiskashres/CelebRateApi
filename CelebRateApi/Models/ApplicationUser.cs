using Microsoft.AspNetCore.Identity;

namespace CelebRateApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? LanguageId { get; set; }
        public Language Language { get; set; } = null!;

        public bool Male { get; set; }
        public int Age { get; set; }
        public string ZipCode { get; set; } = null!;

        public ICollection<Rate> Rates { get; set; } = [];
    }
}
