using CelebRateApi.Data;
using CelebRateApi.DTOs;
using CelebRateApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CelebRateApi.Services
{
    /// <summary>
    /// Handles all Celeb and CelebTranslation related business logic.
    /// </summary>
    public class CelebService(CelebRateDbContext _context)
    {
        /// <summary>
        /// Creates a new Celeb including its first CelebTranslation.
        /// </summary>
        /// <returns> Tuple: indicates if succeeded and any error messages </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors)> CreateNewCelebAsync(CelebDTO dto)
        {
            if (!_context.Languages.Any(l => l.LanguageId == dto.LanguageId))
                return (false, new[] { "Language not found" });

            var celeb = new Celeb { 
                Stars = 0 ,
                CelebTranslations = new List<CelebTranslation>
                {
                    new CelebTranslation
                    {
                        LanguageId = dto.LanguageId,
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        Description = dto.Description
                    }
                }
            };

            await _context.Celebs.AddAsync(celeb);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return (false, new[] { "Creation failed" });


            return (true, null);
        }

        /// <summary>
        /// Deletes a list of Celebs.
        /// </summary>
        /// <returns> Tuple: indicates if succeeded and any error messages </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors)> DeleteCelebsAsync(int[] celebIds)
        {
            var celebs = await _context.Celebs
                .Where(c => celebIds.Contains(c.CelebId))
                .ToListAsync();

            if (!celebs.Any())
                return (false, new[] { "No celeb found" });

            _context.Celebs.RemoveRange(celebs);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return (false, new[] { "Deletion failed" });

            return (true, null);
        }

        /// <summary>
        /// Creates a new CelebTranslation for an existing Celeb.
        /// </summary>
        /// <returns> Tuple: indicates if succeeded and any error messages </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors)> CreateNewCelebTranslationAsync(CelebDTO dto)
        {
            var celebCheck = await _context.Celebs
                .FindAsync(dto.CelebId);
            if (celebCheck == null)
                return (false, new[] { "Celeb not found" });

            var languageCheck = await _context.Languages
                .FindAsync(dto.LanguageId);
            if (languageCheck == null)
                return (false, new[] { "Language not found" });

            var celebTranslationCheck = await _context.CelebTranslations
                .FindAsync(dto.CelebId, dto.LanguageId);
            if (celebTranslationCheck != null)
                return (false, new[] { "CelebTranslation already exists" });

            var celebTranslation = new CelebTranslation
            {
                CelebId = dto.CelebId,
                LanguageId = dto.LanguageId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Description = dto.Description
            };

            await _context.CelebTranslations.AddAsync(celebTranslation);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return (false, new[] { "Creation failed" });

            return (true, null);
        }

        /// <summary>
        /// Edits an existing CelebTranslation.
        /// </summary>
        /// <returns> Tuple: indicates if succeeded and any error messages </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors)> EditCelebTranslationAsync(CelebDTO dto)
        {
            var celebTranslation = await _context.CelebTranslations
                .FindAsync(dto.CelebId, dto.LanguageId);
            if (celebTranslation == null)
                return (false, new[] { "CelebTranslation not found" });

            celebTranslation.FirstName = dto.FirstName;
            celebTranslation.LastName = dto.LastName;
            celebTranslation.Description = dto.Description;

            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return (false, new[] { "Update failed" });

            return (true, null);
        }

        /// <summary>
        /// 1. Deletes a list of CelebTranslations.
        /// 2. Deletes a Celeb that remains without any CelebTranslations.
        /// Fails if CelebTranslation OR Celeb deletion doesn't succeed.
        /// </summary>
        /// <returns> Tuple: indicates if succeeded and any error messages </returns>
        public async Task<(bool Success, IEnumerable<string>? Errors)> DeleteCelebTranslationsAsync(CelebDTO[] dtos)
        {
            // Ensures That CelebTranslation-deletion is rolled back if Celeb-deletion fails. 
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // .AsEnumerable() is needed beause SQL can't handle dtos.Any().
                var celebTranslations = _context.CelebTranslations
                    .AsEnumerable()
                    .Where(ct => dtos.Any(d =>
                        d.CelebId == ct.CelebId &&
                        d.LanguageId == ct.LanguageId))
                    .ToList();

                if (!celebTranslations.Any())
                    return (false, new[] { "No celebTranslation found" });

                _context.CelebTranslations.RemoveRange(celebTranslations);
                var translationResult = await _context.SaveChangesAsync();
                if (translationResult == 0)
                    return (false, new[] { "Deletion failed" });


                // Celeb deletion
                foreach (var dto in dtos)
                {
                    var lastCelebTranslation = await _context.CelebTranslations
                        .FirstOrDefaultAsync(ct => ct.CelebId == dto.CelebId);

                    if (lastCelebTranslation == null)
                    {
                        var celebToRemove = await _context.Celebs
                            .FirstOrDefaultAsync(c => c.CelebId == dto.CelebId);

                        if (celebToRemove != null)
                            _context.Celebs.Remove(celebToRemove);
                    }
                }
                var celebResult = await _context.SaveChangesAsync();
                if (celebResult == 0)
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
