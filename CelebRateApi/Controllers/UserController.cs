using CelebRateApi.DTOs;
using CelebRateApi.Models;
using CelebRateApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;

namespace CelebRateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(
        UserManager<ApplicationUser> userManager,
        JwtService jwtService,
        UserService userService,
        IStringLocalizer<UserController> localizer,
        IAuthorizationService authorizationService) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly JwtService _jwtService = jwtService;
        private readonly UserService _userService = userService;
        private readonly IStringLocalizer<UserController> _localizer = localizer;
        private readonly IAuthorizationService _authorizationService = authorizationService;

        [HttpPost("add-user")]
        public async Task<IActionResult> CreateNewUserAsync(UserDTO dto)
        {
            var result = await _userService.CreateNewUserAsync(dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("User created");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return Unauthorized("Invalid email");

            // validates password and returns if not correct
            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized("Invalid password");

            var token = _jwtService.GenerateToken(user);

            return Ok(new { token });
        }

        [Authorize]
        [HttpPut("edit-user")]
        public async Task<IActionResult> EditUserAsync(UserDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
                return NotFound("User not found");

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, user, "IsOwner");
            if (!authorizationResult.Succeeded && !User.IsInRole("Administrator"))
                return Forbid();

            var result = await _userService.EditUserAsync(dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("User updated");
        }
        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePasswordAsync(PasswordDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
                return NotFound("User not found");

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, user, "IsOwner");
            if (!authorizationResult.Succeeded && !User.IsInRole("Administrator"))
                return Forbid();

            var result = await _userService.ChangePasswordAsync(dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("Password updated");
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
            //var user = await _userManager.FindByIdAsync(dto.UserId);

            //if (user == null)
            //    return NotFound("User not found");

            //var authorizationResult = await _authorizationService.AuthorizeAsync(User, user, "IsOwner");
            //if (!authorizationResult.Succeeded && !User.IsInRole("Administrator"))
            //    return Forbid();

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
