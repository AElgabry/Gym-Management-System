using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.TrainerViewModel;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;

namespace GymManagement.BLL.Services.Classes
{
	public class TrainerService : ITrainerService
	{
		private readonly IUnitRepository _unitRepository;


		public TrainerService(IUnitRepository unitRepository )
		{
			_unitRepository = unitRepository;
		}

		public async Task<bool> AddTrainerAsync(AddTrainerViewModel trainer, CancellationToken ct = default)
		{
			bool EmailExist = await _unitRepository.GetRepository<Trainer>().AnyAsync(e => e.Email == trainer.Email);
			bool PhoneExist = await _unitRepository.GetRepository<Trainer>().AnyAsync(e => e.Phone == trainer.Phone);


			if(EmailExist || PhoneExist == true)
			{
				return false;
			}
			var newTrainer = new Trainer() 
			{
				Name = trainer.Name,
				Email = trainer.Email,
				Phone = trainer.Phone,
				DateOfBirth = trainer.DateOfBirth,
				Gender = trainer.Gender,
				Address = new Address()
				{
					BuildingNumber = trainer.BuildingNumber,
					City = trainer.City,
					Street = trainer.Street
				},
				Speciality = trainer.Speciality
			};
			 _unitRepository.GetRepository<Trainer>().AddAsync(newTrainer);
			var result = await _unitRepository.SaveChangesAsync(ct);
			return result > 0;
		}

		public async Task<IEnumerable<TrainerViewModel>> GetAllTrainerAsync(CancellationToken ct)
		{
			var trainers = await _unitRepository.GetRepository<Trainer>().GetAllAsync();
			if(!trainers.Any()) return [];

			var Alltrainers = trainers.Select(t => new TrainerViewModel
			{
				ID = t.ID,
				Name = t.Name,
				Email = t.Email,
				Phone = t.Phone,
				Speciality = t.Speciality.ToString()
			});

			return Alltrainers;

		}

		public async Task<TrainerDetailsViewModel> GetTrainerDetailsAsync(int id, CancellationToken ct = default)
		{
			var details =  await _unitRepository.GetRepository<Trainer>().GetByIDAsync(id,ct);
			if (details == null) 
				return null;
			else
			{
				return new TrainerDetailsViewModel()
				{
					Name = details.Name,
					Email = details.Email,
					Phone = details.Phone,
					DateOfBirth = details.DateOfBirth,
					Speciality = details.Speciality.ToString(),
					BuildingNumber = details.Address.BuildingNumber,
					Street = details.Address.Street,
					City = details.Address.City
				};
			}
		}

		public async Task<EditTrainerViewModel> GetTrainerToEdit(int id, CancellationToken ct = default)
		{
			var trainer = await _unitRepository.GetRepository<Trainer>().GetByIDAsync(id);
			var newTrainer = new EditTrainerViewModel()
			{
				Name = trainer!.Name,
				Gender = trainer.Gender.ToString(),
				DateOfBirth = trainer.DateOfBirth,
				Email = trainer!.Email,
				Phone = trainer.Phone,
				BuildingNumber = trainer.Address.BuildingNumber,
				City = trainer.Address.City,
				Street = trainer.Address.Street,
				Speciality = trainer.Speciality
			};
			return newTrainer;
		}
		public async Task<bool> EditTrainerDetailsAsync(int id,EditTrainerViewModel trainer , CancellationToken ct =default)
		{
			var oldVersion = await _unitRepository.GetRepository<Trainer>().GetByIDAsync(id,ct);
			if (oldVersion == null) return false;

			bool EmailExist = await _unitRepository.GetRepository<Trainer>().AnyAsync(e => e.Email == trainer.Email && e.ID!=id);
			bool PhoneExist = await _unitRepository.GetRepository<Trainer>().AnyAsync(e => e.Phone == trainer.Phone && e.ID != id);

			if (EmailExist || PhoneExist == true) return false;

			oldVersion!.Email = trainer.Email;
			oldVersion.Phone = trainer.Phone;
			oldVersion.Address.BuildingNumber = trainer.BuildingNumber;
			oldVersion.Address.City = trainer.City;
			oldVersion.Address.Street = trainer.Street;
			oldVersion.Speciality = trainer.Speciality;
			oldVersion.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);

			 _unitRepository.GetRepository<Trainer>().UpdateAsync(oldVersion);
			var result = await _unitRepository.SaveChangesAsync(ct);
			return result > 0;
		}

		public async Task<bool> DeleteTrainerAsync(int id, CancellationToken ct)
		{
			var trainer = await _unitRepository.GetRepository<Trainer>().GetByIDAsync(id);
			if (trainer == null) return false;

			var ActiveSessions = await _unitRepository.GetRepository<Session>().AnyAsync(s => s.TrainerID == trainer.ID && s.EndDate> DateTime.Now);
			if (ActiveSessions) return false;
			
			_unitRepository.GetRepository<Trainer>().DeleteAsync(trainer);
			var result = await _unitRepository.SaveChangesAsync(ct);
			return result > 0;
		}
	}
}
