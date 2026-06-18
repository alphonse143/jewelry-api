using Bogus;
using JewelryAPI.Core.Entities;
using JewelryAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JewelryAPI.Web.Extensions;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");
        
        try
        {
            // Apply pending migrations (creates tables) before attempting to query/seed them.
            if (context.Database.IsRelational())
            {
                await context.Database.MigrateAsync();
            }

            if (!await context.Users.AnyAsync())
            {
                var adminPasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123");
                context.Users.Add(new User
                {
                    Username = "admin",
                    PasswordHash = adminPasswordHash,
                    Role = "Admin"
                });
                await context.SaveChangesAsync();
            }

            if (!await context.Customers.AnyAsync())
            {
                var customerFaker = new Faker<Customer>()
                    .RuleFor(c => c.CustomerName, f => f.Person.FullName)
                    .RuleFor(c => c.MobileNumber, f => f.Phone.PhoneNumber("##########"))
                    .RuleFor(c => c.Address, f => f.Address.FullAddress())
                    .RuleFor(c => c.CreatedDate, f => f.Date.Past(2).ToUniversalTime());

                var customers = customerFaker.Generate(55);
                context.Customers.AddRange(customers);
                await context.SaveChangesAsync();

                var purchaseFaker = new Faker<Purchase>()
                    .RuleFor(p => p.CustomerId, f => f.PickRandom(customers).CustomerId)
                    .RuleFor(p => p.ItemName, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Category, f => f.PickRandom("Gold", "Silver", "Diamond"))
                    .RuleFor(p => p.Weight, f => Math.Round(f.Random.Decimal(1, 50), 2))
                    .RuleFor(p => p.Amount, f => Math.Round(f.Random.Decimal(100, 5000), 2))
                    .RuleFor(p => p.PurchaseDate, f => f.Date.Past(1).ToUniversalTime());

                var purchases = purchaseFaker.Generate(120);
                context.Purchases.AddRange(purchases);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating or seeding the database.");
            // We catch the error so the API process doesn't completely crash if the DB is momentarily down.
        }
    }
}
