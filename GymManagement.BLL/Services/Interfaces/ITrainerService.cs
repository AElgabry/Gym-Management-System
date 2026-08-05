using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagement.BLL.ViewModels.TrainerViewModel;

namespace GymManagement.BLL.Services.Interfaces
{
	public interface ITrainerService
	{
		Task<IEnumerable<TrainerViewModel>> GetAllTrainerAsync(CancellationToken ct=default);
		Task<bool> AddTrainerAsync(AddTrainerViewModel trainer ,CancellationToken ct=default);
		Task<TrainerDetailsViewModel> GetTrainerDetailsAsync(int id, CancellationToken ct = default);
		Task<EditTrainerViewModel> GetTrainerToEdit(int id, CancellationToken ct = default);
		Task<bool> EditTrainerDetailsAsync(int id, EditTrainerViewModel trainer ,CancellationToken ct);
		Task<bool> DeleteTrainerAsync (int id, CancellationToken ct);
	}
}
