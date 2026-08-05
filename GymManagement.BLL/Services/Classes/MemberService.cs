using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Gym_Management.Models;
using GymManagement.BLL.Services.AttachmentService;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;

namespace GymManagement.BLL.Services.Classes
{
	public class MemberService : IMemberService
	{
		private readonly IUnitRepository _unitRepository;
		private readonly IPlanRepository _plan;
		private readonly IAttachmentService attachmentService;

		public MemberService(IUnitRepository unitRepository,IPlanRepository plan, IAttachmentService attachmentService)
		{
			_unitRepository = unitRepository;
			_plan = plan;
			this.attachmentService = attachmentService;
		}


		public async Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(CancellationToken ct)
		{
			var member = await _unitRepository.GetRepository<Member>().GetAllAsync();
			if (!member.Any()) return [];

			var members = member.Select(m => new MemberViewModel
			{
				Id = m.ID,
				Name = m.Name,
				Email = m.Email,
				Phone = m.Phone,
				Photo = m.Photo,
				Gender = m.Gender.ToString()
			});
			return members;

		}
		public async Task<bool> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default)
		{
			var emailExist = await _unitRepository.GetRepository<Member>().AnyAsync(e => e.Email == member.Email);
			var phoneExist = await _unitRepository.GetRepository<Member>().AnyAsync(e => e.Phone == member.Phone);

			if(emailExist || phoneExist)
			{
				return false;
			}
			var photo = await attachmentService.UploadAsync(member.PhotoFile.OpenReadStream(), "MembersPictures", member.PhotoFile.FileName);
			if (string.IsNullOrEmpty(photo?.model)) return false;
			
			var newMember = new Member()
			{
				Name = member.Name,
				Email = member.Email,
				Phone = member.Phone,
				DateOfBirth = member.DateOfBirth,
				Gender = member.Gender,
				Photo = photo.model,
				Address = new Address()
				{
					BuildingNumber = member.BuildingNumber,
					City = member.City,
					Street = member.Street
				},
				HealthRecord = new HealthRecord()
				{
					Height = member.HealthRecordViewModel.Height,
					Weight = member.HealthRecordViewModel.Weight,
					BloodType = member.HealthRecordViewModel.BloodType,
					Note = member.HealthRecordViewModel.Note
				},
			};



			 _unitRepository.GetRepository<Member>().AddAsync(newMember);
			var result = await _unitRepository.SaveChangesAsync(ct);

			if (result > 0) return true;
			else
			{
			 	await attachmentService.DeleteAsync("MembersPictures",photo.model);
				return false;
			}

		}

		public async Task<MemberDetailsViewModel> GetMemberDetailsAsync(int id, CancellationToken ct = default)
		{
			var member = await _unitRepository.GetRepository<Member>().GetByIDAsync(id);
			if (member == null) return null;

			var details = new MemberDetailsViewModel()
			{
				Photo = member.Photo,
				Name = member.Name,
				Email = member.Email,
				Phone = member.Phone,
				Gender = member.Gender.ToString(),
				DateOfBirth = member.DateOfBirth,
				Street = member.Address.Street,
				BuildingNumber = member.Address.BuildingNumber,
				City = member.Address.City,
			};

			var membership = await _unitRepository.GetRepository<MemberPlan>().FirstOrDefaultAsync(m => m.MemberID == id && m.EndDate > DateOnly.FromDateTime(DateTime.Now), ct);
			if (membership != null)
			{

				var activePlan = await _plan.GetByIDAsync(membership.PlanID);
				details.PlanName = activePlan.Name;
				details.MembershipStartDate = membership.CreatedAt;
				details.MembershipEndDate = membership.EndDate;
			}
			return details;

		}

		public async Task<HealthRecordViewModel> GetMemberHealthRecord(int id, CancellationToken ct)
		{
			var record = await _unitRepository.GetRepository<HealthRecord>().FirstOrDefaultAsync(m => m.MemberID == id,ct);
			if (record == null) return null;

			var detailedRecord = new HealthRecordViewModel()
			{
				Height = record.Height,
				Weight = record.Weight,
				BloodType = record.BloodType,
				Note = record.Note
			};
			return detailedRecord;

		}

		public async Task<EditMemberViewModel> EditMemberAsync(int id, CancellationToken ct = default)
		{
			var member = await _unitRepository.GetRepository<Member>().GetByIDAsync(id);

			if (member == null) return null;
			var memberForm = new EditMemberViewModel()
			{
				Name = member.Name,
				Photo = member.Photo,
				Email = member.Email,
				Phone = member.Phone,
				BuildingNumber = member.Address.BuildingNumber,
				City = member.Address.City,
				Street = member.Address.Street
			};
			return memberForm;
		}

		public async Task<bool> EditConfirmationAsync(EditMemberViewModel model, int id, CancellationToken ct)
		{
			var member = await _unitRepository.GetRepository<Member>().GetByIDAsync(id);

			if (member == null) return false;

			bool EmailExist = await _unitRepository.GetRepository<Member>().AnyAsync(e => e.Email == model.Email && e.ID != member.ID);
			bool PhoneExist = await _unitRepository.GetRepository<Member>().AnyAsync(e => e.Phone == model.Phone && e.ID != member.ID);

			if (EmailExist ==true || PhoneExist == true) return false;

			member.Email = model.Email;
			member.Phone = model.Phone;
			member.Address.BuildingNumber = model.BuildingNumber;
			member.Address.Street = model.Street;
			member.Address.City = model.City;

			_unitRepository.GetRepository<Member>().UpdateAsync(member);
			var result =  await _unitRepository.SaveChangesAsync(ct);
			return result > 0;
		}

		public async Task<bool> DeleteMemberAsync(int id, CancellationToken ct)
		{
			var member = await _unitRepository.GetRepository<Member>().GetByIDAsync(id);
			if (member == null) return false;

			 _unitRepository.GetRepository<Member>().DeleteAsync(member);
			var result = await _unitRepository.SaveChangesAsync(ct);
			return result > 0;
		}
	}
}
