using CelebRateApi.Data;
using CelebRateApi.DTOs;
using CelebRateApi.Models;

namespace CelebRateApi.Services
{
    /// <summary>
    /// Handles all Category and CategoryTranslation related business logic.
    /// </summary>
    public class CategoryService(CelebRateDbContext _context)
    {
        //    public async Task<(bool Success, IEnumerable<string>? Errors)> CreateNewCategoryAsync(CelebDTO dto)
        //    {
        //        var celeb = new Celeb
        //        {
        //            Stars = 0,
        //            CelebTranslations = new List<CelebTranslation>
        //            {
        //                new CelebTranslation
        //                {
        //                    LanguageId = dto.LanguageId,
        //                    FirstName = dto.FirstName,
        //                    LastName = dto.LastName,
        //                    Description = dto.Description
        //                }
        //            }
        //        };

        //        await _context.Celebs.AddAsync(celeb);
        //        await _context.SaveChangesAsync();

        //        return (true, null);
        //    }
    }
}
