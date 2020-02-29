using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PieShop.Models;
using PieShop.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PieShop.Controllers
{
	public class HomeController : Controller
	{
		private readonly IPieRepository _repository;

		public HomeController(IPieRepository repository)
		{
			_repository = repository;
		}
		
		public IActionResult Index()
		{
			var homeViewModel = new HomeViewModel
			{
				PiesOfTheWeek = _repository.PiesOfTheWeek
			};

			return View(homeViewModel);
		}
	}
}
