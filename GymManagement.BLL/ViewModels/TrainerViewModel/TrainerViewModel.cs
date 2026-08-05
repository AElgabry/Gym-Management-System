using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagement.DAL.Models.Enum;

namespace GymManagement.BLL.ViewModels.TrainerViewModel
{
	public class TrainerViewModel
	{
		public int ID { get; set; }

		public string Name { get; set; } = default!;
		public string Email { get; set; } = default!;
		public string Phone { get; set; } = default!; 
		public string Speciality { get; set; } = default!;
	}
}
