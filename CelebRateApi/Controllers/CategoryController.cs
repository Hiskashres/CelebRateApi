using CelebRateApi.DTOs;
using CelebRateApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CelebRateApi.Controllers
{
    /// <summary>
    /// Handles all Category related HTTP requests.
    /// </summary>
    //[Authorize(policy: "RequireModerator")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(CategoryService categoryService) : ControllerBase
    {
        private readonly CategoryService _categoryService = categoryService;

        [HttpPost("add-categoryTranslation")]
        public async Task<IActionResult> CreateNewCategoryTranslationAsync(CategoryDTO dto)
        {
            var result = await _categoryService.CreateNewCategoryTranslationAsync(dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("CategoryTranslation created");
        }

        [HttpPut("edit-categoryTranslation")]
        public async Task<IActionResult> EditCategoryTranslationAsync(CategoryDTO dto)
        {
            var result = await _categoryService.EditCategoryTranslationAsync(dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("CategoryTranslation updated");
        }

        [HttpDelete("delete-categoryTranslations")]
        public async Task<IActionResult> DeleteCategoryTranslationsAsync(CategoryDTO[] dtos)
        {
            var result = await _categoryService.DeleteCategoryTranslationsAsync(dtos);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("CategoryTranslations Deleted");
        }
    }
}
