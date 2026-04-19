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

            var categories = new[]
            {
                new { CategoryName = "category1", Order = 1 },
                new { CategoryName = "category2", Order = 2 },
                new { CategoryName = "category3", Order = 3 },
                new { CategoryName = "category4", Order = 4 },
                new { CategoryName = "category5", Order = 5 },
            };

            foreach (var category in categories)
            {
                bool exists = context.Categories
                    .Any(c => c.DefaultName == category.CategoryName);

                if (!exists)
                {
                    await context.Categories
                        .AddAsync(new Category { DefaultName = category.CategoryName, Order = category.Order});
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
