using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
	public class Book : Base
	{
		public int BookingDays { get; set; }
		public bool IsAttended { get; set; }
		public Member Member { get; set; } //nav
		public int MemberID { get; set; } //fk
		public Session Session { get; set; } //nav
		public int SeesionID { get; set; }//fk
	}
}
