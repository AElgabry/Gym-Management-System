using Gym_Management.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gym_Management.Controllers
{
	[Authorize]
	public class PlansController : Controller
	{

		private readonly IPlanRepository db;
		
		public PlansController(IPlanRepository db)
		{
			this.db = db;
		}
		public async Task<IActionResult> Index()
		{
			var plans = await db.GetAllAsync();
			return View(plans);
		}
		public async Task<IActionResult> Details(int ID)
		{
			var plan = await db.GetByIDAsync(ID);
			if (plan==null)
			{
				return RedirectToAction(nameof(Index));
			}
			return View(plan);
		}
		[HttpGet]
		public async Task<IActionResult> Edit(int ID,CancellationToken ct = default)
		{
			var plan = await db.GetToUpdate(ID);
			return View(plan);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(int ID, Plan plan, CancellationToken ct = default)
		{
			if (!ModelState.IsValid) return View(plan);

			var result =  await db.UpdatePlanAsync(ID, plan);

			if(result)
			{
				TempData["Success"] = "Plan has been updated successfully";
			}
			else
			{
				TempData["Failed"] = "Failed to update the plan";
			}
			return RedirectToAction(nameof(Index));
		}
		[HttpPost]
		public async Task<IActionResult> Index(int id)
		{
			var result = await db.ToogleActiveAsync(id);
			if (result)
			{
				TempData["Success"] = "Plan status changed";
			}
			else
			{
				TempData["Failed"] = "Failed to update the plan";
			}
			return RedirectToAction(nameof(Index));
		}

	}
}
