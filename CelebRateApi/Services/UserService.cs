using CelebRateApi.Data;
using CelebRateApi.DTOs;
using CelebRateApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CelebRateApi.Services
{
    public class UserService(UserManager<ApplicationUser> userManager, CelebRateDbContext _context)
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

        public async Task<(bool Success, IEnumerable<string>? Errors)> EditUserAsync(UserDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return (false, new[] { "User not found" });

            var roles = await _userManager.GetRolesAsync(user);

            user.UserName = roles.Contains("Moderator") || roles.Contains("Admin")
                ? dto.UserName : dto.Email;

            var emailResult = await _userManager.SetEmailAsync(user, dto.Email);
            if (!emailResult.Succeeded)
                return (false, emailResult.Errors.Select(e => e.Description));

            user.NormalizedEmail = _userManager.NormalizeEmail(dto.Email);
            user.NormalizedUserName = _userManager.NormalizeName(user.UserName);
            user.Age = dto.Age;
            user.Male = dto.Male;
            user.ZipCode = dto.ZipCode;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description));

            return (true, null);
        }

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

        public async Task<(bool Success, IEnumerable<string>? Errors)> ChangeUserRoleAsync(UserRoleDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return (false, new[] { "User not found" });

            var validRoles = new[] { "User", "Moderator", "Admin" };
            if (!validRoles.Contains(dto.Role))
                return (false, new[] { "Invalid role" });

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return (false, removeResult.Errors.Select(e => e.Description));

            var addResult = await _userManager.AddToRoleAsync(user, dto.Role);
            if (!addResult.Succeeded)
                return (false, addResult.Errors.Select(e => e.Description));

            return (true, null);
        }

        public async Task<(bool Success, IEnumerable<string>? Errors)> DeleteUsersAsync(string[] userIds)
        {
            // Creates list of all instances to delete
            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            // If no instances in the list, return false
            if (!users.Any())
                return (false, new[] {"Users not found"});

            // Removes from DB all instances in the list
            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync();

            // Else, if something was deleted, return true

            return (true, null);
        }
    }
}
