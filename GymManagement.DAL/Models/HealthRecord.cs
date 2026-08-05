using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
	public class HealthRecord : Base
	{
		//updatedAt is the last update field
		public decimal Height { get; set; }
		public decimal Weight { get; set; }
		public string BloodType { get; set; }
		public string Note { get; set; }
		public int MemberID { get; set; } //fk
		public Member Member { get; set; }
	}
}
