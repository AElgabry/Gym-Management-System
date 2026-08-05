using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Gym_Management;
using Gym_Management.Models;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DAL.Repositories.Classes
{
	public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Base, new()
	{
		private readonly GybDbContext db;
		private readonly DbSet<TEntity> set;
		
		public GenericRepository(GybDbContext db)
		{
			this.db = db;
			set = db.Set<TEntity>();
		}

		public async void AddAsync(TEntity e)
		{
			await set.AddAsync(e);
		}

		public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)=> await set.AsNoTracking().AnyAsync(predicate,ct);

		public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default) => predicate is null ? await set.AsNoTracking().CountAsync(ct) : await set.AsNoTracking().CountAsync(predicate,ct);  

		public async void DeleteAsync(TEntity e)
		{
			set.Remove(e);
		}

		public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct, bool tracking = false)
		{
			var query = tracking ? set : set.AsNoTracking();
			return await query.FirstOrDefaultAsync(predicate);
		}

		public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
		{
			IQueryable<TEntity>  query = tracking ? set: set.AsNoTracking();
			return await query.ToListAsync(ct);
		}

		public async Task<TEntity?> GetByIDAsync(int ID, CancellationToken ct = default) => await set.FindAsync(ID,ct);

		public void UpdateAsync(TEntity e)
		{
			set.Update(e);
		}
	}
}
