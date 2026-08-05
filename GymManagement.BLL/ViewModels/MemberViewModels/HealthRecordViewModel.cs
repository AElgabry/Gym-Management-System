using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.MemberViewModels
{
	public class HealthRecordViewModel
	{
		public decimal Height { get; set; } = default!;

		public decimal Weight { get; set; } = default!;
		public string BloodType { get; set; } = default!;
		public string? Note { get; set; } = default!;
	}
}
