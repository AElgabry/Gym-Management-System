using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Gym_Management;
using Gym_Management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymManagement.DAL
{
	public class DataSeeding
	{
		public static async Task SeedDate(GybDbContext db, string folderPath, ILogger logger, CancellationToken ct = default)
		{
			try
			{
				if (!await db.Plans.AnyAsync(ct))
				{
					var plans = LoadDate<Plan>(folderPath, "plans.json");

					if (plans.Count > 0)
					{
						db.Plans.AddRange(plans);
						logger.LogInformation($"Plans data seeded successfully {plans.Count}");
					}
					if (db.ChangeTracker.HasChanges())
						await db.SaveChangesAsync(ct);
					else
						logger.LogInformation("Data already seeded");
				}
			 }
			 catch (Exception ex)
			 {
				logger.LogError(ex, "Gym data seeding failed.");
				throw;
			}
		}
		public static List<T> LoadDate<T>(string folderpath , string fileName)
		{
			var filePath = Path.Combine(folderpath, fileName);
			if(!File.Exists(filePath))
			{
				throw new FileNotFoundException("Couldn't seed the data");
			}

			var data = File.ReadAllText(filePath);
			return JsonSerializer.Deserialize<List<T>>(data) ?? [];
		}
	}
}
