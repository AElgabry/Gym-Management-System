using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.TrainerViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Gym_Management.Controllers
{
	[Authorize(Roles = "SuperAdmin")]
	public class TrainerController : Controller
	{
		private readonly ITrainerService trainerService;
		public TrainerController(ITrainerService trainerService)
		{
			this.trainerService = trainerService;
		}

		public async Task<IActionResult> Index(CancellationToken ct)
		{
			var trainer = await trainerService.GetAllTrainerAsync(ct);
			return View(trainer);
		}
		[HttpGet]
		public async Task<IActionResult> Add(CancellationToken ct) => View();
		[HttpPost]
		public async Task<IActionResult> Add(AddTrainerViewModel trainer, CancellationToken ct)
		{
			if (!ModelState.IsValid)
			{
				return View(trainer);
			}
			var result = await trainerService.AddTrainerAsync(trainer, ct);
			if (result)
			{
				TempData["Success"] = "Trainer added successfully";
			}
			else
			{
				TempData["Failed"] = "Failed to add the trainer";
			}
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> Details(int id, CancellationToken ct)
		{
			var result = await trainerService.GetTrainerDetailsAsync(id);
			if (result == null)
			{
				return View(nameof(Index));
			}
			return View(result);
		}
		[HttpGet]
		public async Task<IActionResult> Edit(int id) 
		{
			var result = await trainerService.GetTrainerToEdit(id);
			return View(result);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(int id, EditTrainerViewModel model, CancellationToken ct)
		{

			if (!ModelState.IsValid) return View(model);
			var result = await trainerService.EditTrainerDetailsAsync(id, model, ct);

			if(result)
				TempData["Success"] = "Trainer has been updated successfully";
			else
				TempData["Failed"] = "Failed to update the trainer";
			return RedirectToAction(nameof(Index));


		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id,CancellationToken ct)
		{
			var isValid = await trainerService.GetTrainerDetailsAsync(id);	
			if(isValid ==null)
			{
				TempData["Failed"] = "Failed to delete the trainer";
				return RedirectToAction(nameof(Index));
			}
			return View();
		}
		
		[HttpPost]
		public async Task<IActionResult> DeleteConfirm([FromRoute]int id,CancellationToken ct)
		{
			var result = await trainerService.DeleteTrainerAsync(id,ct);
			if (result)
				TempData["Success"] = "Trainer has been deleted successfully";
			else
				TempData["Failed"] = "Failed to delete the trainer";
			return RedirectToAction(nameof(Index));
		}
	}
}
