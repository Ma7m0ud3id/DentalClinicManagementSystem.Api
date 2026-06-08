using DentalClinicManagementSystem.DAL.Data;
using DentalClinicManagementSystem.DAL.Seed;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicManagementSystem.Apis.Extensions
{
    public static class DatabaseInitializerExtensions
    {
        public static async Task InitializeDatabaseAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                    logger.LogInformation("Applying migrations...");
                    await context.Database.MigrateAsync();
                    logger.LogInformation("Migrations applied successfully");

                    logger.LogInformation("Seeding database...");
                    await DatabaseSeeder.SeedAsync(context);
                    logger.LogInformation("Database seeded successfully");
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while initializing the database");
                    throw;
                }
            }
        }
    }
}