using GymManagement.BLL.Services.AttachmentService;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Gym_Management.Controllers
{
	[Authorize(Roles ="SuperAdmin")]
	public class MemberController : Controller
	{
		private readonly IMemberService memberService;
		private readonly IAttachmentService attachmentService;

		public MemberController(IMemberService ms, IAttachmentService attachmentService)
		{
			memberService = ms;
			this.attachmentService = attachmentService;
		}
		public async Task<IActionResult> Index(CancellationToken ct)
		{
			var members = await memberService.GetAllMemberAsync(ct);
			return View(members);
		}


		[HttpGet]
		public async Task<IActionResult> Picture(int id)
		{
			var member = await memberService.GetMemberDetailsAsync(id);
			if (member == null || string.IsNullOrEmpty(member.Photo)) return null;

			var result = attachmentService.GetFile(member.Photo, "MembersPictures");
			if (result == null) return null;

			return File(result.Value.Stream, result.Value.ContentType);

		}



		[HttpGet]
		public async Task<IActionResult> Create() => View();
		
		[HttpPost]
		public async Task<IActionResult> Create(CreateMemberViewModel newMember, CancellationToken ct) 
		{
			if (!ModelState.IsValid) return View(newMember);
			var addMember = await memberService.CreateMemberAsync(newMember, ct);
			if(addMember)
			{
				TempData["Success"] = "Member added successfully";
			}
			else
			{
				TempData["failed"] = "Failed to add the member";
			}
			return RedirectToAction(nameof(Index));

		}

		public async Task<IActionResult> Details(int id, CancellationToken ct)
		{
			var member = await memberService.GetMemberDetailsAsync(id);
			if(member==null)
			{
				return View(nameof(Index));
			}
			return View(member);
		}
		public async Task<IActionResult> HealthDetails(int id, CancellationToken ct)
		{
			var member = await memberService.GetMemberHealthRecord(id, ct);
			if (member == null)
			{
				return View(nameof(Index));
			}
			return View(member);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id, CancellationToken ct)
		{
			var member = await memberService.EditMemberAsync(id);
			if (member == null) return View(nameof(Index));
			return View(member);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditMemberViewModel model , int id, CancellationToken ct)
		{
			var result = await memberService.EditConfirmationAsync(model, id, ct);
			if (result)
				TempData["Success"] = "Trainer has been updated successfully";
			else
				TempData["Failed"] = "Failed to update the trainer";
			return RedirectToAction(nameof(Index));

		}
		[HttpGet]
		public async Task<IActionResult> Delete(int id) 
		{
			var result = await memberService.GetMemberDetailsAsync(id);
			if (result == null) return null;
			return View(result);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id, CancellationToken ct) 
		{
			var result = await memberService.DeleteMemberAsync(id,ct);

			if(result)
			{
				TempData["Success"] = "Member has been deleted successfully";
			}
			else
			{
				TempData["Failed"] = "Failed to delete the member";
			}
			return RedirectToAction(nameof(Index));

		}



	}
}
