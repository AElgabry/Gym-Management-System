using GymManagement.DAL;
using Microsoft.EntityFrameworkCore;

namespace Gym_Management
{
	public static class ProgramExtention
	{
		public static async Task MigrateAndSeedAsync(this WebApplication app)
		{
			var scope = app.Services.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<GybDbContext>();
			var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

			var pendingMigration = await db.Database.GetPendingMigrationsAsync();
			if (pendingMigration.Any())
			{
				logger.LogInformation($"Applying {pendingMigration.Count()} migrations");
				await db.Database.MigrateAsync();
			}
			var seedPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
			await DataSeeding.SeedDate(db, seedPath, logger);

		}
	}
}
