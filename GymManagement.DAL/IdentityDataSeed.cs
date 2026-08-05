using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymManagement.DAL
{
	public static class IdentityDataSeed
	{
		public static async Task SeedAsync( RoleManager<IdentityRole> role, UserManager<ApplicationUser> user ,ILogger logger, CancellationToken ct = default )
		{

			try
			{
				var hasRole = await role.Roles.AnyAsync(ct);
				var hasUser = await user.Users.AnyAsync(ct);

				if (hasRole && hasUser) return ;

				if (!hasRole)
				{
					var roles = new List<IdentityRole>()
				{
					new IdentityRole("SuperAdmin"),
					new IdentityRole("Admin")
				};

					foreach (var item in roles)
					{
						if (!await role.RoleExistsAsync(item.Name))
						{
							await role.CreateAsync(item);
						}
						else
						{
							return;
						}
					}

				}
				if (!hasUser)
				{
					var MainAdmin = new ApplicationUser()
					{
						FirstName = "Abdelrahman",
						LastName = "ElGabry",
						UserName = "Horus",
						Email = "horus@gmail.com",
						PhoneNumber = "01550118204"
					};
					await user.CreateAsync(MainAdmin, "P@ssw0rd");
					await user.AddToRoleAsync(MainAdmin, "SuperAdmin");

					var Admin = new ApplicationUser()
					{
						FirstName = "Osama",
						LastName = "ElGabry",
						UserName = "Baba",
						Email = "baba@gmail.com",
						PhoneNumber = "01093655890"
					};
					await user.CreateAsync(Admin, "P@ssw0rd");
					await user.AddToRoleAsync(Admin, "Admin");
				}
			}
			catch (Exception ex)
			{
				logger.LogInformation($"Failed to seed the roles or user : {ex}");
				return;	
			}



		}

	}
}
