using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagement.BLL.ViewModels.MemberViewModels;

namespace GymManagement.BLL.Services.Interfaces
{
	public interface IMemberService
	{
		Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(CancellationToken ct);
		Task<bool> CreateMemberAsync(CreateMemberViewModel member ,CancellationToken ct=default);
		Task<MemberDetailsViewModel> GetMemberDetailsAsync(int id, CancellationToken ct = default);
		Task<HealthRecordViewModel> GetMemberHealthRecord(int id , CancellationToken ct = default);
		Task<EditMemberViewModel> EditMemberAsync(int id, CancellationToken ct =default);
		Task<bool> EditConfirmationAsync(EditMemberViewModel model, int id , CancellationToken ct);
		Task<bool> DeleteMemberAsync(int id, CancellationToken ct);


	}
}
