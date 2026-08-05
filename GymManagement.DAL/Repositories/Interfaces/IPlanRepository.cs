using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Gym_Management.Models;

namespace GymManagement.DAL.Repositories.Interfaces
{
	public interface IPlanRepository

	{
		Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false,CancellationToken ct =default);
		Task<Plan?> GetByIDAsync(int ID, CancellationToken ct = default);
		Task<int> AddAsync(Plan p, CancellationToken ct = default);
		Task<int> UpdateAsync(Plan p, CancellationToken ct = default);
		Task<Plan> GetToUpdate(int id, CancellationToken ct = default);

		Task<bool> UpdatePlanAsync(int id, Plan plan, CancellationToken ct =default);
		Task<int> DeleteAsync(Plan p, CancellationToken ct = default);
		Task<bool> ToogleActiveAsync(int id, CancellationToken ct = default);
		Task<Plan?> FirstOrDefaultAsync(Expression<Func<Plan, bool>> predicate, CancellationToken ct, bool tracking = false);

	}
}
