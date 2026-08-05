using GymManagement.BLL;
using GymManagement.BLL.Services.AttachmentService;
using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gym_Management
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddScoped<IPlanRepository, PlanRepository>();
			builder.Services.AddScoped<IMemberService, MemberService>();
			builder.Services.AddScoped<ITrainerService, TrainerService>();
			builder.Services.AddScoped<IUnitRepository, UnitRepository>();
			builder.Services.AddScoped<ISessionService, SessionService>();
			builder.Services.AddScoped<ISessionRepository, SessionRepository>();
			builder.Services.AddScoped<IAttachmentService, AttachmentService>();
			builder.Services.AddScoped<IHomeService, HomeService>();




			builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
			confiq =>
			{
				confiq.Lockout.MaxFailedAccessAttempts = 5;
				confiq.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
			
				
			}).AddEntityFrameworkStores<GybDbContext>();


			builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfile()));


			builder.Services.AddDbContext<GybDbContext>(options=>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});
			builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			var app = builder.Build();

			var scope = app.Services.CreateScope();


			var roleManagement = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
			var userManagement = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
			var logger = scope.ServiceProvider.GetService<ILogger<Program>>();

			await ProgramExtention.MigrateAndSeedAsync(app);
			await IdentityDataSeed.SeedAsync(roleManagement,userManagement,logger);




			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapStaticAssets();
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}")
				.WithStaticAssets();

			app.Run();
		}
	}
}
