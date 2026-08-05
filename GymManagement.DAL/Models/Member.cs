using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
	public class Member : User
	{
		//createdAt is the Join Date
		public string? Photo { get; set; }
		public HealthRecord HealthRecord { get; set; }
		public ICollection<Book> Sessions { get; set; }
		public ICollection<MemberPlan> MemberPlan { get; set; }

	}
}
