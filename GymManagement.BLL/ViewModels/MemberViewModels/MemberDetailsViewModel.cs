using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.MemberViewModels
{
	public class MemberDetailsViewModel
	{
		public string? Photo { get; set; }
		public string Name { get; set; } = default!;
		public string Email { get; set; } = default!;
		public string Phone { get; set; } = default!;
		public string Gender { get; set; } = default!;
		public DateOnly DateOfBirth { get; set; }
		public string BuildingNumber { get; set; } = default!;
		public string Street { get; set; } = default!;
		public string City { get; set; } = default!;
		public DateOnly ?MembershipStartDate { get; set; }
		public DateOnly? MembershipEndDate { get; set; }
		public string? PlanName { get; set; } = default!;

	}
}
