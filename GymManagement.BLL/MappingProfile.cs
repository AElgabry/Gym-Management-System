using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagement.BLL.ViewModels.SessionViewModel;
using GymManagement.DAL.Models;

namespace GymManagement.BLL
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Session, SessionViewModel>()
			.ForMember(d => d.TrainerName, s => s.MapFrom(s => s.Trainer.Name))
			.ForMember(d => d.CategoryName, s => s.MapFrom(s => s.Category.CategoryName))
			.ForMember(d => d.StartTime, s => s.MapFrom(s => s.StartDate))
			.ForMember(d => d.EndTime, s => s.MapFrom(s => s.EndDate))
			.ForMember(d => d.AvailableSlots, s => s.MapFrom(s => s.Capacity - s.SessionMembers.Count));

			CreateMap<AddSessionViewModel, Session>();

			CreateMap<Category, CategoryMenu>();

			CreateMap<Trainer, TrainerMenu>();

			CreateMap<Session, EditSessionViewModel>().ReverseMap();



		}
	}
}
