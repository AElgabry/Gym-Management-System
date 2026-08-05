using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gym_Management.Models;

namespace GymManagement.DAL.Models
{
	public class MemberPlan : Base
	{
		//CreatedAt is the Start Date
		public DateOnly EndDate { get; set; }
		public Member Member { get; set; } //nav
		public int MemberID { get; set; } //fk

		public Plan Plan { get; set; }//nav
		public int PlanID { get; set; }//fk
	}
}
