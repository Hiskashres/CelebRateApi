using CelebRateApi.Data;
using CelebRateApi.DTOs;
using CelebRateApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CelebRateApi.Services
{
    /// <summary>
    /// Handles all Tag and TegTranslation related business logic.
    /// </summary>
    public class TagService(CelebRateDbContext _context)
    {
        /// <summary>
        /// Creates a new Tag including its first TagTranslation.
        /// </summary>
        /// <returns> Tuple: indicates if succeeded and any error messages </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors)> CreateNewTagAsync(TagDTO dto)
        {
            if (!_context.Categories.Any(c => c.CategoryId == dto.CategoryId))
                return (false, new[] { "Category not found" });

            if (!_context.Languages.Any(l => l.LanguageId == dto.LanguageId))
                return (false, new[] { "Language not found" });

            var tag = new Tag
            {
                CategoryId = dto.CategoryId,
                TagTranslations =
                [
                    new() {
                        LanguageId = dto.LanguageId,
                        TagName = dto.TagName
                    }
                ]
            };

            await _context.Tags.AddAsync(tag);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return (false, new[] { "Creation failed" });

            return (true, null);
        }

        /// <summary>
        /// Deletes a list of Tags.
        /// </summary>
        /// <returns> Tuple: indicates if succeeded and any error messages </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors)> DeleteTagsAsync(int[] tagIds)
        {
            var tags = await _context.Tags
                .Where(t => tagIds.Contains(t.TagId))
                .ToListAsync();

            if (!tags.Any())
                return (false, new[] { "No tag found" });

            _context.Tags.RemoveRange(tags);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return (false, new[] { "Deletion failed" });

            return (true, null);
        }

        /// <summary>
        /// Creates a new TagTranslation for an existing Tag.
        /// </summary>
        /// <returns> Tuple: indicates if succeeded and any error messages </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors)> CreateNewTagTranslationAsync(TagDTO dto)
        {
            if (!_context.Tags.Any(t => t.TagId == dto.TagId))
                return (false, new[] { "Tag not found" });

            if (!_context.Languages.Any(l => l.LanguageId == dto.LanguageId))
                return (false, new[] { "Language not found" });

            if (_context.TagTranslations.Any(cl => cl.TagId == dto.TagId && cl.LanguageId == dto.LanguageId))
                return (false, new[] { "TagTranslation already exists" });

            var tagTranslation = new TagTranslation
            {
                TagId = dto.TagId,
                LanguageId = dto.LanguageId,
                TagName = dto.TagName,
            };

            await _context.TagTranslations.AddAsync(tagTranslation);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return (false, new[] { "Creation failed" });

            return (true, null);
        }

        /// <summary>
        /// Edits an existing TagTranslation.
        /// </summary>
        /// <returns> Tuple: indicates if succeeded and any error messages </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors)> EditTagTranslationAsync(TagDTO dto)
        {
            var tagTranslation = await _context.TagTranslations
                .FindAsync(dto.TagId, dto.LanguageId);
            if (tagTranslation == null)
                return (false, new[] { "TagTranslation not found" });

            tagTranslation.TagName = dto.TagName;

            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return (false, new[] { "Update failed" });

            return (true, null);
        }

        /// <summary>
        /// 1. Deletes a list of TagTranslations.
        /// 2. Deletes a Tag that remains without any TagTranslations.
        /// Fails if TagTranslation OR Tag deletion doesn't succeed.
        /// </summary>
        /// <returns> Tuple: indicates if succeeded and any error messages </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors)> DeleteTagTranslationsAsync(TagDTO[] dtos)
        {
            // Ensures That TagTranslation-deletion is rolled back if Tag-deletion fails. 
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // .AsEnumerable() is needed beause SQL can't handle dtos.Any().
                var tagTranslations = _context.TagTranslations
                    .AsEnumerable()
                    .Where(tt => dtos.Any(d =>
                        d.TagId == tt.TagId &&
                        d.LanguageId == tt.LanguageId))
                    .ToList();

                if (!tagTranslations.Any())
                    return (false, new[] { "No tagTranslation found" });

                _context.TagTranslations.RemoveRange(tagTranslations);
                var translationResult = await _context.SaveChangesAsync();
                if (translationResult == 0)
                    return (false, new[] { "Deletion failed" });


                // Tag deletion
                foreach (var dto in dtos)
                {
                    var lastTagTranslation = await _context.TagTranslations
                        .FirstOrDefaultAsync(tt => tt.TagId == dto.TagId);

                    if (lastTagTranslation == null)
                    {
                        var tagToRemove = await _context.Tags
                            .FirstOrDefaultAsync(t => t.TagId == dto.TagId);

                        if (tagToRemove != null)
                            _context.Tags.Remove(tagToRemove);
                    }
                }
                var tagResult = await _context.SaveChangesAsync();
                if (tagResult == 0)
                    return (false, new[] { "Deletion failed" }); await transaction.CommitAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, new[] { ex.Message });
            }
        }
    }
}
