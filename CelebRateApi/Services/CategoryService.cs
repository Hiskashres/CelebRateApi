using CelebRateApi.Data;
using CelebRateApi.DTOs;
using CelebRateApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CelebRateApi.Services
{
    /// <summary>
    /// Handles all Category and CategoryTranslation related business logic.
    /// </summary>
    public class CategoryService(CelebRateDbContext _context)
    {
        /// <summary>
        /// Creates new CategoryTranslation.
        /// </summary>
        /// <returns> Tuple: indicates if succeeded and any error messages </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors)> CreateNewCategoryTranslationAsync(CategoryDTO dto)
        {
            if (!_context.Categories.Any(c => c.CategoryId == dto.CategoryId))
                return (false, new[] { "Category not found" });

            if (!_context.Languages.Any(l => l.LanguageId == dto.LanguageId))
                return (false, new[] { "Language not found" });

            if (_context.CategoryTranslations.Any(cl => cl.CategoryId == dto.CategoryId && cl.LanguageId == dto.LanguageId))
                return (false, new[] { "CategoryTranslation already exists" });

            var categoryTranslation = new CategoryTranslation
            {
                CategoryId = dto.CategoryId,
                LanguageId = dto.LanguageId,
                CategoryName = dto.CategoryName,
                Description = dto.Description
            };

            await _context.CategoryTranslations.AddAsync(categoryTranslation);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return (false, new[] { "Creation failed" });

            return (true, null);
        }

        /// <summary>
        /// Edits an existing CategoryTranslation.
        /// </summary>
        /// <returns> Tuple: indicates if succeeded and any error messages </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors)> EditCategoryTranslationAsync(CategoryDTO dto)
        {
            var categoryTranslation = await _context.CategoryTranslations
                .FindAsync(dto.CategoryId, dto.LanguageId);
            if (categoryTranslation == null)
                return (false, new[] { "CategoryTranslation not found" });

            categoryTranslation.CategoryName = dto.CategoryName;
            categoryTranslation.Description = dto.Description;

            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return (false, new[] { "Update failed" });

            return (true, null);
        }


        /// <summary>
        /// Deletes a list of CelebTranslations.
        /// </summary>
        /// <returns> Tuple: indicates if succeeded and any error messages </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors)> DeleteCategoryTranslationsAsync(CategoryDTO[] dtos)
        {
            // .AsEnumerable() is needed beause SQL can't handle dtos.Any().
            var categoryTranslations = _context.CategoryTranslations
                .AsEnumerable()
                .Where(ct => dtos.Any(d =>
                    d.CategoryId == ct.CategoryId &&
                    d.LanguageId == ct.LanguageId))
                .ToList();

            if (!categoryTranslations.Any())
                return (false, new[] { "No celebTranslation found" });

            _context.CategoryTranslations.RemoveRange(categoryTranslations);
            var translationResult = await _context.SaveChangesAsync();
            if (translationResult == 0)
                return (false, new[] { "Deletion failed" });

            return (true, null);
        }
    }
}
