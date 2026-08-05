using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gym_Management;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;

namespace GymManagement.DAL.Repositories.Classes
{
	public class UnitRepository : IUnitRepository
	{
		private readonly GybDbContext _db;
		private readonly Dictionary<string, object> repositories=[];
		public UnitRepository(GybDbContext db)
		{
			_db = db;
		}
		public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : Base, new()
		{
			var typeName = typeof(TEntity).Name;

			if (repositories.TryGetValue(typeName, out object? repo))
				return (IGenericRepository<TEntity>)repo;
			else
			{
				var newRepo = new GenericRepository<TEntity>(_db);
				repositories[typeName] = newRepo;
				return newRepo;
			}
		}

		public async Task<int> SaveChangesAsync(CancellationToken ct)
		{
			return await _db.SaveChangesAsync(ct);
		}
	}
}
