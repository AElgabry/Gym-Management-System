using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagement.BLL.ViewModels;

namespace GymManagement.BLL.Services.Interfaces
{
	public interface IHomeService
	{
		Task<HomeViewModel> GetHomeDataAsync(CancellationToken ct=default);
	}
}
