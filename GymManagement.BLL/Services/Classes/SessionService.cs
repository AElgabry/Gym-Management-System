using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SessionViewModel;
using GymManagement.DAL.Models;
using GymManagement.DAL.Models.Enum;
using GymManagement.DAL.Repositories.Interfaces;

namespace GymManagement.BLL.Services.Classes
{
	public class SessionService : ISessionService
	{
		private readonly IUnitRepository unitRepository;
		private readonly IMapper mapper;
		private readonly ISessionRepository session;

		public SessionService(IUnitRepository unitRepository,IMapper mapper, ISessionRepository session)
		{
			this.unitRepository = unitRepository;
			this.mapper = mapper;
			this.session = session;
		}

		public async Task<Result> AddNewSession(AddSessionViewModel model, CancellationToken ct)
		{
			if (model.StartDate >= model.EndDate) return Result.Validation("Start date must be before the end date");
			if (model.StartDate <= DateTime.Now) return Result.Validation("Start date must be in the future");
			if (model.Capacity <= 1) return Result.Validation("Capacity must be greater than 1");


			var trainer = await unitRepository.GetRepository<Trainer>().GetByIDAsync(model.TrainerID);
			if (trainer == null) return Result.NotFound("There is no trainer with such an ID");

			var category = await unitRepository.GetRepository<Category>().GetByIDAsync(model.CategoryID);
			if (category == null) return Result.NotFound("There is no category with such an ID");

			var speciality = Enum.TryParse<Speciality>(category.CategoryName, out var categoryName);
			if (speciality == false || trainer.Speciality != categoryName) return Result.Validation("Trainer and speciality doesn't match");

			var map = mapper.Map<AddSessionViewModel,Session>(model);

			unitRepository.GetRepository<Session>().AddAsync(map);
			var result =  await unitRepository.SaveChangesAsync(ct);
			return result > 0 ? Result.Ok() : Result.Faild("Failed to create the session") ;
	
		}

		public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
		{
			var allSessions = await session.GetSessionsWithTrainerAndCategory(ct);
			var map = mapper.Map<IEnumerable<SessionViewModel>>(allSessions);
			return map;
			
		}

		public async Task<IEnumerable<CategoryMenu>> GetCategoryMenu(CancellationToken ct = default)
		{
			var menu = await unitRepository.GetRepository<Category>().GetAllAsync();
			var map = mapper.Map<IEnumerable<CategoryMenu>>(menu);
			return map;
		}

		public async Task<Result<SessionViewModel>?> GetSessionByIDAsync(int id, CancellationToken ct)
		{
			var deatils = await session.GetSessionIDWithTrainerAndCategory(id,ct);
			if (deatils == null) return Result<SessionViewModel>.NotFound("Session not found");

			var map = mapper.Map<SessionViewModel>(deatils);
			return Result<SessionViewModel>.Ok(map);
		}

		public async Task<IEnumerable<TrainerMenu>> GetTrainerMenu(CancellationToken ct = default)
		{
			var menu = await unitRepository.GetRepository<Trainer>().GetAllAsync();
			var map = mapper.Map<IEnumerable<TrainerMenu>>(menu);
			return map;
		}

		public async Task<Result<EditSessionViewModel>> GetSessionToEditAsync(int id, CancellationToken ct = default)
		{
			var model = await session.GetByIDAsync(id);
			if (model == null) return  Result<EditSessionViewModel>.NotFound("Session was not found");
			if (model.StartDate <= DateTime.Now) return Result<EditSessionViewModel>.Faild("Session must be in the furure to edit");

			var map = mapper.Map<EditSessionViewModel>(model);
			return Result<EditSessionViewModel>.Ok(map); 
		}

		public async Task<Result> UpdateSessionAsync(int id, EditSessionViewModel newVersion, CancellationToken ct)
		{
			var oldVersion = await session.GetByIDAsync(id);

			if (oldVersion == null) return Result.NotFound("Session was not found");
			if (newVersion.StartDate <= DateTime.Now) return Result.Validation("Session date must be in the future");
			if (newVersion.StartDate >= newVersion.EndDate) return Result.Validation("End date must be after the start date");

			var trainer = await unitRepository.GetRepository<Trainer>().GetByIDAsync(newVersion.TrainerID);
			if (trainer == null) return Result.NotFound("Trainer was not found");
			var category = await unitRepository.GetRepository<Category>().GetByIDAsync(oldVersion.CategoryID);
			if (category == null) return Result.NotFound("Category was not found");
			if (trainer.Speciality.ToString() != category.CategoryName) return Result.Validation("This trainer does not belong to this category");

			var map = mapper.Map(newVersion, oldVersion);
			unitRepository.GetRepository<Session>().UpdateAsync(map);
			var result = await unitRepository.SaveChangesAsync(ct);
			return result > 0 ? Result.Ok() : Result.Faild("Failed to update the session");

		}

		public async Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default)
		{
			var model = await session.GetByIDAsync(id, ct);
			if (model == null) return Result.NotFound("Session cannot be found");
			if(model.StartDate <= DateTime.Now && DateTime.Now <= model.EndDate) return Result.NotFound("Cannot delete an ongoing session");


			session.DeleteAsync(model);
			var result = await unitRepository.SaveChangesAsync(ct);
			return result > 0 ? Result.Ok() : Result.Faild("Failed to delete the session");
		}
	}
}
