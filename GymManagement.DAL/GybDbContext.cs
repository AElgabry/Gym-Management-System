using System.Reflection;
using Gym_Management.Models;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gym_Management
{
	public class GybDbContext : IdentityDbContext<ApplicationUser>
	{
		public GybDbContext(DbContextOptions<GybDbContext> options) : base(options) 
		{
			
		}

		public DbSet<Plan> Plans { get; set; }
		public DbSet<Session> Session{ get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(
				Assembly.GetExecutingAssembly());

			modelBuilder.Entity<ApplicationUser>(EB =>
			{
				EB.Property(X => X.FirstName)
				.HasColumnType("varchar")
				.HasMaxLength(50);

				EB.Property(X => X.LastName)
				.HasColumnType("varchar")
				.HasMaxLength(50);
			});
		}
	}
}
