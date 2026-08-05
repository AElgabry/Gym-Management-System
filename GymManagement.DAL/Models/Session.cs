using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
	public class Session :Base
	{
		public string Description { get; set; }
		public int Capacity { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public ICollection<Book> SessionMembers { get; set; }
		public Trainer Trainer { get; set; } //nav
		public int TrainerID { get; set; }//fk
		public Category Category { get; set; }//nav
		public int CategoryID { get; set; }

	}
}
