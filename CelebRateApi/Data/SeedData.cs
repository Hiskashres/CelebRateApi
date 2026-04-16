using Microsoft.AspNetCore.Identity;
using CelebRateApi.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace CelebRateApi.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = serviceProvider.GetRequiredService<CelebRateDbContext>();

            string[] roles = ["User", "Moderator", "Admin"];

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            //var defoultLanguage = await context.Languages
            //    .FirstOrDefaultAsync(l => l.ShortName == _config["DefaultLanguage"])
            //    ?? throw new Exception("Default language not found");

            //string[] categories = ["Category1", "Category2", "Category3", "Category4", "Category5"];

            //foreach (var category in categories)
            //{
            //    bool exists = await context.CategoryTranslations
            //        .Any(ct => ct.LanguageId == defoultLanguage.LanguageId && ct.CategoryName == category);

            //    if (!exists)
            //    {
            //        await context.CategoryTranslations
            //            .AddAsync(new CategoryTranslation { CategoryName = category });
            //    }
            //}

            //await context.SaveChangesAsync();
        }
    }
}
