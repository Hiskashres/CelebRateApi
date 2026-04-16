using CelebRateApi.Data;
using CelebRateApi.DTOs;
using CelebRateApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CelebRateApi.Services
{
    /// <summary>
    /// Handles all User related business logic.
    /// </summary>
    /// <remarks>
    /// Typicaly uses ASP.NET Identity (UserManager) for database queries.
    /// </remarks>
    public class UserService(UserManager<ApplicationUser> userManager, CelebRateDbContext context)
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly CelebRateDbContext _context = context;

        /// <summary>
        /// Edits an existing User profile.
        /// </summary>
        /// <remarks>
        /// Email must be unique and must meet the Email Policy guidlines.
        /// The default UserName is equal to Email, Only a Moderator or Admin can have a different UserName.
        /// </remarks>
        /// <returns> Tuple: indicates if succeeded and any error messages. </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors)> EditUserProfileAsync(UserDTO dto)
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

        /// <summary>
        /// Changes User's Role
        /// </summary>
        /// <remarks>
        /// User can have only one Role at a time.
        /// Removes all current User's roles before adding the new one.
        /// </remarks>
        /// <returns> Tuple: indicates if succeeded and any error messages. </returns>
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

        /// <summary>
        /// Deletes a list of Users
        /// Deletes directly from the database, because UserManager hardly handles list-deletion
        /// </summary>
        /// <returns> Tuple: indicates if succeeded and any error messages. </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors)> DeleteUsersAsync(string[] userIds)
        {
            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();
            if (!users.Any())
                return (false, new[] {"Users not found"});

            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync();

            return (true, null);
        }
    }
}
