using CelebRateApi.Data;
using CelebRateApi.DTOs;
using CelebRateApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CelebRateApi.Services
{
    public class CelebService(CelebRateDbContext _context)
    {
        public async Task<(bool Success, IEnumerable<string>? Errors)> CreateNewCelebAsync(CelebDTO dto)
        {
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
            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool Success, IEnumerable<string>? Errors)> DeleteCelebsAsync(int[] celebIds)
        {
            var celebs = await _context.Celebs
                .Where(c => celebIds.Contains(c.CelebId))
                .ToListAsync();

            if (!celebs.Any())
                return (false, new[] { "No celeb found" });

            _context.Celebs.RemoveRange(celebs);
            await _context.SaveChangesAsync();

            return (true, null);
        }

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
                return (false, new[] { "CelebTranslatien already exists" });

            var celebTranslation = new CelebTranslation
            {
                CelebId = dto.CelebId,
                LanguageId = dto.LanguageId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Description = dto.Description
            };

            await _context.CelebTranslations.AddAsync(celebTranslation);
            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool Success, IEnumerable<string>? Errors)> EditCelebTranslationAsync(CelebDTO dto)
        {
            var celebTranslation = await _context.CelebTranslations
                .FindAsync(dto.CelebId, dto.LanguageId);
            if (celebTranslation == null)
                return (false, new[] { "CelebTranslatien not found" });

            celebTranslation.FirstName = dto.FirstName;
            celebTranslation.LastName = dto.LastName;
            celebTranslation.Description = dto.Description;

            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool Success, IEnumerable<string>? Errors)> DeleteCelebTranslationsAsync(CelebDTO[] dtos)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var celebTranslations = _context.CelebTranslations
                    .AsEnumerable()
                    .Where(ct => dtos.Any(d =>
                        d.CelebId == ct.CelebId &&
                        d.LanguageId == ct.LanguageId))
                    .ToList();

                if (!celebTranslations.Any())
                    return (false, new[] { "No celebTranslation found" });

                _context.CelebTranslations.RemoveRange(celebTranslations);
                await _context.SaveChangesAsync();

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
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

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
