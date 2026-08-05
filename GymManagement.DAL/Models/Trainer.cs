using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagement.DAL.Models.Enum;

namespace GymManagement.DAL.Models
{
	public class Trainer : User
	{
		//createdAt is the hire date
		public Speciality Speciality { get; set; }
		public ICollection<Session> TrainerSessions { get; set; }

	}
}
