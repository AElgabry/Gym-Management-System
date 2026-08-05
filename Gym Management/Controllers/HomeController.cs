using System.Diagnostics;
using Gym_Management.Models;
using GymManagement.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym_Management.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IHomeService homeService;

		public HomeController(ILogger<HomeController> logger , IHomeService homeService)
		{
			_logger = logger;
			this.homeService = homeService;
		}

		public async Task<IActionResult> Index()
		{
			var home = await homeService.GetHomeDataAsync();
			return View(home);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
