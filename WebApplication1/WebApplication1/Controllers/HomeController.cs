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
			SortingEnum sortingState = SortingEnum.CityAsc,
			int cityIdToAdd = -1)
		{
			//Logining
			var cookies = HttpContext.Request.Cookies;
			if (cookies["Login"] != null && cookies["Password"] != null)
			{
				isLogedIn = true;

				//Adding City To Acc
				if (cityIdToAdd > 0)
				{
					userCity = new UsersCity()
					{
						CityId = cityIdToAdd,
						UserId =
							db.users.Where(x => x.Login == cookies["Login"])
						.Where(x => x.Password == cookies["Password"]).FirstOrDefault().Id
					};
					db.userscities.Add(userCity);
					db.SaveChanges();
				}
			}
			citiesAndCountries = db.citiesAndCountries;
			
			return await PostWeather(page, filter, sortingState);
		}
	}
}