using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GymManagement.DAL.Models;

namespace GymManagement.DAL.Repositories.Interfaces
{
	public interface IUnitRepository
	{
		IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : Base, new();
		Task<int> SaveChangesAsync( CancellationToken ct);
	}
}
