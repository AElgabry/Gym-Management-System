using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
	public class Address
	{
		public string BuildingNumber { get; set; } = default!;
		public string Street { get; set; } = default!;
		public string City { get; set; } = default!;
	}
}
