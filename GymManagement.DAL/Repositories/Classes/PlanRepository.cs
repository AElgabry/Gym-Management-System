using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Gym_Management;
using Gym_Management.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DAL.Repositories.Classes
{
	public class PlanRepository : IPlanRepository
	{
		private readonly GybDbContext db;
		public PlanRepository(GybDbContext db)
		{
			this.db = db;
		}
		public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
		=> tracking ? await db.Plans.ToListAsync(ct) : await db.Plans.AsNoTracking().ToListAsync(ct);

		public async Task<Plan?> GetByIDAsync(int ID, CancellationToken ct = default)
		=> await db.Plans.FirstOrDefaultAsync(id => id.ID == ID , ct);
		
		public async Task<int> AddAsync(Plan p, CancellationToken ct = default)
		{
			await db.Plans.AddAsync(p,ct);
			return await db.SaveChangesAsync(ct);
		}
		
		public  async Task<int> DeleteAsync(Plan p, CancellationToken ct = default)
		{
			db.Plans.Remove(p);
			return await db.SaveChangesAsync(ct);
		}
		public async Task<Plan> GetToUpdate(int id, CancellationToken ct = default)
		{
			var result = await db.Plans.FindAsync(id);
			if (result == null) return null;
			return result;
		}
		public Task<int> UpdateAsync(Plan p, CancellationToken ct = default)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> UpdatePlanAsync(int id, Plan plan, CancellationToken ct = default)
		{
			var oldVersion = await db.Plans.FindAsync(id);
			if (oldVersion == null) return false;
			oldVersion!.DurationDays = plan.DurationDays;
			oldVersion.Price = plan.Price;
			oldVersion.Description = plan.Description;
			oldVersion.UpdatedAt = DateTime.Now;
			db.Plans.Update(oldVersion);
			var result = await db.SaveChangesAsync(ct);
			return result > 0;
		}

		public async Task<bool> ToogleActiveAsync(int id, CancellationToken ct=default)
		{
			var plan = await db.Plans.FindAsync(id,ct);
			if (plan == null) return false;

			plan.IsActive = !plan.IsActive;
			plan.UpdatedAt = DateTime.Now;
			db.Plans.Update(plan);
			return db.SaveChanges() > 0;
		}
		public async Task<Plan?> FirstOrDefaultAsync(Expression<Func<Plan, bool>> predicate, CancellationToken ct, bool tracking = false)
		{
			var query = tracking ? db.Plans : db.Plans.AsNoTracking();
			return await query.FirstOrDefaultAsync(predicate);
		}
	}
}
