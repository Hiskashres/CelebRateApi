using CelebRateApi.DTOs;
using CelebRateApi.Models;
using CelebRateApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;

namespace CelebRateApi.Controllers
{
    /// <summary>
    /// Handles all User HTTP requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(
        UserManager<ApplicationUser> userManager,
        UserService userService,
        IStringLocalizer<UserController> localizer,
        IAuthorizationService authorizationService) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly UserService _userService = userService;
        private readonly IStringLocalizer<UserController> _localizer = localizer;
        private readonly IAuthorizationService _authorizationService = authorizationService;

        [Authorize]
        [HttpPut("edit-profile")]
        public async Task<IActionResult> EditUserProfileAsync(UserDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
                return NotFound("User not found");

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, user, "IsOwner");
            if (!authorizationResult.Succeeded && !User.IsInRole("Administrator"))
                return Forbid();

            var result = await _userService.EditUserProfileAsync(dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("User updated");
        }

        [Authorize(policy: "RequireAdmin")]
        [HttpPut("change-userRole")]
        public async Task<IActionResult> ChangeUserRoleAsync(UserRoleDTO dto)
        {
            var result = await _userService.ChangeUserRoleAsync(dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("User changed");
        }

        [Authorize(policy: "RequireModerator")]
        [HttpDelete("delete-users")]
        public async Task<IActionResult> DeleteUsersAsync(string[] userIds)
        {
            var result = await _userService.DeleteUsersAsync(userIds);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("Users Deleted");
        }

        //[Authorize(Policy = "RequireUser")]
        [HttpGet("test")]
        public async Task<IActionResult> RoleTesr(/*[FromQuery] string? culture*/)
        {
            return Ok(_localizer["Test"]);
        }
    }
}
