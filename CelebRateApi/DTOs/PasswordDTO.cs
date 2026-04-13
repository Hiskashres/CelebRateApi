namespace CelebRateApi.DTOs
{
    public class PasswordDTO
    {
        public string UserId { get; set; } = null!;
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
