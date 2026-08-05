using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.TrainerViewModel
{
	public class TrainerDetailsViewModel
	{

		public string Name { get; set; } = default!;
		public string Email { get; set; } = default!;
		public string Phone { get; set; } = default!;
		public string Speciality { get; set; } = default!;
		public DateOnly DateOfBirth { get; set; } = default!;
		public string BuildingNumber { get; set; } = default!;
		public string Street { get; set; } = default!;
		public string City { get; set; } = default!;
	}
}
