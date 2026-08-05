using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GymManagement.DAL.Models;

namespace GymManagement.DAL.Repositories.Interfaces
{
	public interface ISessionRepository:IGenericRepository<Session>
	{
		public Task<IEnumerable<Session>> GetSessionsWithTrainerAndCategory (CancellationToken ct);


		public Task<Session?> GetSessionIDWithTrainerAndCategory(int id , CancellationToken ct);
	}
}
