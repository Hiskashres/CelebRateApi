using CelebRateApi.Data;
using CelebRateApi.DTOs;
using CelebRateApi.Models;
using Microsoft.AspNetCore.Identity;

namespace CelebRateApi.Services
{
    public class UserService(UserManager<ApplicationUser> userManager)
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<(bool Success, IEnumerable<string>? Errors)> CreateNewUserAsync(UserDTO dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                Age = dto.Age,
                Male = dto.Male,
                ZipCode = dto.ZipCode
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description));

            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
                return (false, roleResult.Errors.Select(e => e.Description));

            return (true, null);
        }
    }
}
