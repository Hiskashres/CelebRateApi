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
    public class UserController(UserManager<ApplicationUser> userManager, JwtService jwtService, UserService userService) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly JwtService _jwtService = jwtService;
        private readonly UserService _userService = userService;

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

        [Authorize(Policy = "RequireUser")]
        [HttpGet("test")]
        public async Task<IActionResult> RoleTesr()
        {
            return Ok("You're authorized");
        }
    }
}
