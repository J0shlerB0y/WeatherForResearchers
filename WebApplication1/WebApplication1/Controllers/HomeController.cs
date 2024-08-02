using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WeatherResearcher.Models;
using WeatherResearcher.Services;

namespace WeatherResearcher.Controllers
{
	public class HomeController : AbstractWeatherShowerController
	{
		private readonly ILogger<HomeController> _logger;
		private PasswordHandler passwordHandler;

		public HomeController(ILogger<HomeController> logger, ApplicationContext ContextDb, PasswordHandler passwordHandler)
		{
			_logger = logger;
			db = ContextDb;
			this.passwordHandler = passwordHandler;
		}


		public async Task<IActionResult> Index(int page = 0,
			FilterViewModel filter = null,
			SortingEnum sortingState = SortingEnum.CityAsc)
		{
			//Logining
			var cookies = HttpContext.Request.Cookies;
			if (cookies["Login"] != null && cookies["Password"] != null && !db.users.Where(x => x.Login
							== cookies["Login"] && passwordHandler.DecryptString(cookies["Password"]) == x.Password).IsNullOrEmpty())
			{
				isLogedIn = true;
			}
			citiesAndCountries = db.citiesAndCountries;

			return await PostWeather(page, filter, sortingState);
		}
	}
}