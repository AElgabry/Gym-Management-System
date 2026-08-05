using GymManagement.BLL.ViewModels;
using GymManagement.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Gym_Management.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> user;
		private readonly SignInManager<ApplicationUser> signCheck;

		public AccountController(UserManager<ApplicationUser> user, SignInManager<ApplicationUser> signCheck)
		{
			this.user = user;
			this.signCheck = signCheck;
		}


	
		[HttpGet]
		public async Task<IActionResult> Login() =>  View();
	
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model, CancellationToken ct)
		{
			if (!ModelState.IsValid) return View(model);

			var client = await user.FindByEmailAsync(model.Email);
			
			if(client==null)
			{ 
				ModelState.AddModelError("InvalidLogin", "Invalid email or password.");
				return View(model);
			}

			var result = await signCheck.PasswordSignInAsync(client, model.Password, model.RememberMe, false);

			if(result.Succeeded)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ModelState.AddModelError("InvalidLogin", "Invalid email or password.");
				return View(model);
			}


		}
		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await signCheck.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}

		public async Task<IActionResult> AccessDenied()
		{
			return View();
		}

	}
}
