using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
	public class Base
	{
		public int ID { get; set; }
		public DateOnly CreatedAt { get; set; }
		public DateOnly? UpdatedAt { get; set; }
	}
}
