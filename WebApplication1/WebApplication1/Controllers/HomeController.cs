using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Text;
using WeatherResearcher.Models;
using WeatherResearcher.Services;

namespace WeatherResearcher.Controllers
{
    public class HomeController : AbstractWeatherShowerController
	{
		private UsersCity userCity;
		protected readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, ApplicationContext ContextDb)
		{
			_logger = logger;
			db = ContextDb;
		}


		public async Task<IActionResult> Index(int page = 0,
			FilterViewModel filter = null,
			SortingEnum sortingState = SortingEnum.CityAsc)
		{
			//Logining
			var cookies = HttpContext.Request.Cookies;
			if (cookies["Login"] != null)
			{
				isLogedIn = true;
			}
			citiesAndCountries = db.citiesAndCountries;
			
			return await PostWeather(page, filter, sortingState);
		}
	}
}