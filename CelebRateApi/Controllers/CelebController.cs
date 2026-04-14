using CelebRateApi.DTOs;
using CelebRateApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CelebRateApi.Controllers
{
    [Authorize(policy: "RequireModerator")]
    [Route("api/[controller]")]
    [ApiController]
    public class CelebController(CelebService celebService) : ControllerBase
    {
        private readonly CelebService _celebService = celebService;

        [HttpPost("add-celeb")]
        public async Task<IActionResult> CreateNewCelebAsync(CelebDTO dto)
        {
            var result = await _celebService.CreateNewCelebAsync(dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("Celeb created");
        }

        [HttpDelete("delete-celebs")]
        public async Task<IActionResult> DeleteCelebsAsync(int[] celebIds)
        {
            var result = await _celebService.DeleteCelebsAsync(celebIds);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("Celebs Deleted");
        }

        [HttpPost("add-celebTranslation")]
        public async Task<IActionResult> CreateNewCelebTranslationAsync(CelebDTO dto)
        {
            var result = await _celebService.CreateNewCelebTranslationAsync(dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("CelebTranslation created");
        }

        [HttpPut("edit-celebTranslation")]
        public async Task<IActionResult> EditCelebTranslationAsync(CelebDTO dto)
        {
            var result = await _celebService.EditCelebTranslationAsync(dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("CelebTranslation updated");
        }

        [HttpDelete("delete-celebTranslations")]
        public async Task<IActionResult> DeleteCelebTranslationsAsync(CelebDTO[] dtos)
        {
            var result = await _celebService.DeleteCelebTranslationsAsync(dtos);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("CelebTranslations Deleted");
        }
    }
}
