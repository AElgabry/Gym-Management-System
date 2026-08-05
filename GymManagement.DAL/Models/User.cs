using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using GymManagement.DAL.Models.Enum;

namespace GymManagement.DAL.Models
{
	public abstract class User : Base
	{
		public string Name { get; set; } = default!;
		public string Phone { get; set; } = default!;
		public string Email { get; set; } = default!;
		public DateOnly DateOfBirth { get; set; }
		public Gender Gender { get; set; }
		public Address Address { get; set; }
	}
}
