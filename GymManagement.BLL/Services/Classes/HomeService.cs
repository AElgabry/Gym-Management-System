using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;

namespace GymManagement.BLL.Services.Classes
{
	public class HomeService : IHomeService
	{
		private readonly IUnitRepository unitOfWork;

		public HomeService(IUnitRepository unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public async Task<HomeViewModel> GetHomeDataAsync(CancellationToken ct = default)
		{
			var totalMembers = await unitOfWork.GetRepository<Member>().CountAsync();
			var activeMembers = await unitOfWork.GetRepository<MemberPlan>().CountAsync(a => a.EndDate > DateOnly.FromDateTime(DateTime.Now));
			var totalTrainers = await  unitOfWork.GetRepository<Trainer>().CountAsync();
			var upSessions =await unitOfWork.GetRepository<Session>().CountAsync(s => s.StartDate > DateTime.Now);
			var ongoing = await unitOfWork.GetRepository<Session>().CountAsync(s => s.StartDate < DateTime.Now && s.EndDate > DateTime.Now);
			var compSessions = await unitOfWork.GetRepository<Session>().CountAsync(s => s.EndDate < DateTime.Now);

			var home = new HomeViewModel()
			{
				TotalMembers = totalMembers,
				ActiveMembers = activeMembers,
				TotalTrainers = totalTrainers,
				UpcomingSessions = upSessions,
				OngoingSessions = ongoing,
				CompletedSessions = compSessions
			};

			return home;
		}
	}
}
