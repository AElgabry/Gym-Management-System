using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagement.BLL.ViewModels.SessionViewModel;
using GymManagement.DAL.Models;

namespace GymManagement.BLL.Services.Interfaces
{
	public interface ISessionService
	{
		public Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
		public Task<Result> AddNewSession(AddSessionViewModel model, CancellationToken ct);
		public Task<IEnumerable<TrainerMenu>> GetTrainerMenu( CancellationToken ct = default);
		public Task<IEnumerable<CategoryMenu>> GetCategoryMenu( CancellationToken ct = default);
		public Task<Result<SessionViewModel>?> GetSessionByIDAsync(int id, CancellationToken ct);
		public Task<Result<EditSessionViewModel>> GetSessionToEditAsync(int id, CancellationToken ct = default);
		public Task<Result> UpdateSessionAsync(int id, EditSessionViewModel model, CancellationToken ct = default);
		public Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default);
	}
}
