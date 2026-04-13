namespace CelebRateApi.DTOs
{
    public class UserDTO
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool Male { get; set; }
        public int Age { get; set; }
        public int ZipCode { get; set; }
    }
}
