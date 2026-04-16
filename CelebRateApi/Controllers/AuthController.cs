using CelebRateApi.DTOs;
using CelebRateApi.Models;
using CelebRateApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CelebRateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        UserManager<ApplicationUser> userManager,
        AuthService authService,
        IAuthorizationService authorizationService) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly AuthService _authService = authService;
        private readonly IAuthorizationService _authorizationService = authorizationService;

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(UserDTO dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(new { message = "User registered", result.token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(new { result.token });
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

            var result = await _authService.ChangePasswordAsync(dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("Password updated");
        }
    }
}
