using CelebRateApi.DTOs;
using CelebRateApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CelebRateApi.Controllers
{
    [Authorize(policy: "RequireModerator")]
    [Route("api/[controller]")]
    [ApiController]
    public class TagController(TagService tagService) : ControllerBase
    {
        private readonly TagService _tagService = tagService;

        [HttpPost("add-tag")]
        public async Task<IActionResult> CreateNewTagAsync(TagDTO dto)
        {
            var result = await _tagService.CreateNewTagAsync(dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("Tag created");
        }

        [HttpDelete("delete-tags")]
        public async Task<IActionResult> DeleteTagsAsync(int[] tagIds)
        {
            var result = await _tagService.DeleteTagsAsync(tagIds);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("Tags deleted");
        }

        [HttpPost("add-tagTranslation")]
        public async Task<IActionResult> CreateNewTagTranslationAsync(TagDTO dto)
        {
            var result = await _tagService.CreateNewTagTranslationAsync(dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("TagTranslation created");
        }

        [HttpPut("edit-tagTranslation")]
        public async Task<IActionResult> EditTagTranslationAsync(TagDTO dto)
        {
            var result = await _tagService.EditTagTranslationAsync(dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("TagTranslation updated");
        }


        [HttpDelete("delete-tagTranslations")]
        public async Task<IActionResult> DeleteTagbTranslationsAsync(TagDTO[] dtos)
        {
            var result = await _tagService.DeleteTagTranslationsAsync(dtos);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("TagTranslations Deleted");
        }
    }
}
