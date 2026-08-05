using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagement.DAL.Models;
using GymManagement.DAL.Models.Enum;

namespace GymManagement.BLL.ViewModels.SessionViewModel
{
	public class SessionViewModel
	{
		public int ID { get; set; }
		public string Description { get; set; } = default!;
		public string TrainerName { get; set; } = default!;
		public string CategoryName { get; set; } = default!;
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public int Capacity { get; set; }
		public int AvailableSlots { get; set; }

		public string DisplayDate => $"{StartTime:MMM dd , yyyy}";
		public string DisplayTime => $"{StartTime:hh:mm} - {EndTime:hh:mm}";
		public TimeSpan Duration => EndTime - StartTime ;


		public string Status 
		{
			get
			{
				if (StartTime > DateTime.Now) return "Upcomming";
				else if (StartTime <= DateTime.Now && EndTime >= DateTime.Now) return "Ongoing";
				else return "Completed";
			}
		}




	}
}
