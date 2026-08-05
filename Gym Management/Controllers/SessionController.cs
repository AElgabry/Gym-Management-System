using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SessionViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Gym_Management.Controllers
{
	[Authorize]
	public class SessionController : Controller
	{
		private readonly ISessionService session;

		public SessionController(ISessionService session)
		{
			this.session = session;
		}


		public async Task<IActionResult> Index( CancellationToken ct)
		{
			var result = await session.GetAllSessionsAsync();
			return View(result);
		}
		[HttpGet]
		public async Task<IActionResult> Add()
		{
			ViewBag.Trainer = new SelectList(await session.GetTrainerMenu(), "ID", "Name");
			ViewBag.Category = new SelectList(await session.GetCategoryMenu(), "ID", "CategoryName");
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddSessionViewModel model,CancellationToken ct)
		{
			ViewBag.Trainer = new SelectList(await session.GetTrainerMenu(), "ID", "Name");
			ViewBag.Category = new SelectList(await session.GetCategoryMenu(), "ID", "CategoryName");
			if (!ModelState.IsValid) return View(model);

			var newSession = await session.AddNewSession(model, ct);
			if(newSession.result)
			{
				TempData["Success"] = "Session Added Successfully";
				return RedirectToAction(nameof(Index));
			}
			else
			{
				TempData["Failed"] = newSession.error;
				return View(model);
			}
			
		}


		public async Task<IActionResult> Details(int id, CancellationToken ct)
		{

			var details = await session.GetSessionByIDAsync(id,ct);
			if(details.result)
			{
				return View(details.model);
			} 
			else
			{
				TempData["Failed"] = details.message;
				return RedirectToAction(nameof(Index));
			}
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id, CancellationToken ct)
		{
			var model = await session.GetSessionToEditAsync(id,ct);
			if(model.result)
			{
				ViewBag.Trainers = new SelectList(await session.GetTrainerMenu(), "ID", "Name");
				return View(model.model);
			}
			else
			{
				TempData["Failed"] = model.message;
				return RedirectToAction(nameof(Index));
			}

		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, EditSessionViewModel model ,CancellationToken ct)
		{
			var newModel = await session.UpdateSessionAsync(id, model, ct);
			if(newModel.result)
			{
				TempData["Success"] = "Session updated successully";
				return RedirectToAction(nameof(Index));
			}
			else
			{
				ViewBag.Trainers = new SelectList(await session.GetTrainerMenu(), "ID", "Name");
				TempData["Failed"] = newModel.error;
				return View(model);
			}
		}

		[HttpGet]
		public async Task<IActionResult> Delete(CancellationToken ct) =>  View(ct);

		[HttpPost]
		public async Task<IActionResult> Delete(int id , CancellationToken ct)
		{
			var result = await session.DeleteSessionAsync(id, ct);

			if(result.result)
			{
				TempData["Success"] = "Session deleted successfully";
			}
			else
			{
				TempData["Failed"] = result.error;
			}
			return RedirectToAction(nameof(Index));
		}


	}
}
