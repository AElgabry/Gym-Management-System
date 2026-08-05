using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Gym_Management.Models;
using GymManagement.DAL.Models;

namespace GymManagement.DAL.Repositories.Interfaces
{
	public interface IGenericRepository<TEntity> where TEntity : Base , new()
	{
		Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
		Task<TEntity?> GetByIDAsync(int ID, CancellationToken ct = default);
		void AddAsync(TEntity p);
		void UpdateAsync(TEntity p);
		void DeleteAsync(TEntity p);
		Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate , CancellationToken ct=default);
		Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct =default, bool tracking = false);
		Task<int> CountAsync(Expression<Func<TEntity,bool>>? predicate = null , CancellationToken ct = default);
	}
}
