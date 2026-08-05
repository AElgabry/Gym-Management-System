using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Gym_Management;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DAL.Repositories.Classes
{
	public class SessionRepository : GenericRepository<Session>, ISessionRepository
	{
		private readonly GybDbContext db;
		public SessionRepository(GybDbContext db) : base(db)
		{
			this.db = db;
		}

		public async Task<Session?> GetSessionIDWithTrainerAndCategory(int id, CancellationToken ct)
		{
			var session = db.Session.AsNoTracking().Include(t => t.Trainer).Include(c => c.Category).FirstOrDefault(i => i.ID == id);
			return session;
		}

		public async Task<IEnumerable<Session>> GetSessionsWithTrainerAndCategory(CancellationToken ct)
		{
			return await db.Session.AsNoTracking().Include(t => t.Trainer).Include(c => c.Category).ToListAsync(ct);
		}
	}
}
