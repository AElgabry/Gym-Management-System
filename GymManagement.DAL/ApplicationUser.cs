using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GymManagement.DAL
{
	public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; } = default!;
		public string LastName { get; set; } = default!;

	}
}
