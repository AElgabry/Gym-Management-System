using GymManagement.DAL.Models;

namespace Gym_Management.Models
{
	public class Plan
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int DurationDays { get; set; }

		public decimal Price { get; set; }
		public bool  IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		public ICollection<MemberPlan>? PlanMember { get; set; }
	}
}
