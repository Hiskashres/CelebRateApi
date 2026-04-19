using CelebRateApi.Data;
using CelebRateApi.DTOs;
using CelebRateApi.Models;
using Microsoft.AspNetCore.Identity;

namespace CelebRateApi.Services
{
    /// <summary>
    /// Handles all user-identity related business logic.
    /// </summary>
    /// <remarks>
    /// Typicaly uses ASP.NET Identity (UserManager) for database queries.
    /// </remarks>
    public class AuthService(UserManager<ApplicationUser> userManager, JwtService jwtService)
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly JwtService _jwtService = jwtService;

        /// <summary>
        /// Registers new User, assigns the default "User" role and creates token.
        /// Fails if any of them doesn't succeed.
        /// </summary>
        /// <remarks>
        /// Email must be unique and must meet the Email Policy guidlines.
        /// Password must meet the Password Policy guidlines.
        /// UserName is equal to Email
        /// </remarks>
        /// <returns> Tuple: success flag, error messages if any, token as a string. </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors, string? token)> 
            RegisterAsync(UserDTO dto)
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
                return (false, result.Errors.Select(e => e.Description), null);

            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
                return (false, roleResult.Errors.Select(e => e.Description), null);

            var token = await _jwtService.GenerateToken(user);
            if (token == null)
                return (false, new[] {"Failed to generate token"}, null);

            return (true, null, token);
        }

        /// <summary>
        /// Creates token.
        /// </summary>
        /// <returns> Tuple: success flag, error messages if any, token as a string. </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors, string? token)>
        LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return (false, new[] { "Invalid Email" }, null);

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                return (false, new[] { "Invalid Password" }, null);

            var token = await _jwtService.GenerateToken(user);
            if (token == null)
                return (false, new[] { "Failed to generate token" }, null);

            return (true, null, token);
        }

        /// <summary>
        /// Changes User's Password
        /// </summary>
        /// <remarks>
        /// Expects valid current password.
        /// New password must meet the Password Policy guidlines.
        /// </remarks>
        /// <returns> Tuple: indicates if succeeded and any error messages. </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors)> ChangePasswordAsync(PasswordDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return (false, new[] { "User not found" });

            var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);

            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description));

            return (true, null);
        }
    }
}
