using Microsoft.AspNetCore.Mvc;
using WeatherResearcher.Models;
using WeatherResearcher.Services;

namespace WeatherResearcher.Controllers
{
	public class OwnCabinetController : AbstractWeatherShowerController
	{
		private readonly ILogger<HomeController> _logger;
		private PasswordHandler passwordHandler;

		public OwnCabinetController(ILogger<HomeController> logger, ApplicationContext ContextDb, PasswordHandler _passwordHandler)
		{
			passwordHandler = _passwordHandler;
			_logger = logger;
			db = ContextDb;
			this.passwordHandler = passwordHandler;
		}
		public async Task<IActionResult> OwnWeather(int page = 0,
			FilterViewModel filter = null,
			SortingEnum sortingState = SortingEnum.CityAsc)
		{
			citiesAndCountries = Enumerable.Empty<CityAndCountry>().AsQueryable();
			//fetching and recycling cookies

			var cookies = HttpContext.Request.Cookies;

			if (cookies["Login"] != null && cookies["Password"] != null)
			{
				citiesAndCountries = db.userscities.Join(
					db.users.Where(
						(x)=> (x.Login == cookies["Login"]) &&
						( x.Password == passwordHandler.DecryptString(cookies["Password"]) )
						),
					x => x.UserId,
					y => y.Id,
					(x, y) => x.CityId
					).Join(db.citiesAndCountries,
					x => x,
					y => y.Id,
					(x, y) => 
					new CityAndCountry { Id = x, CityTitle_en = y.CityTitle_en, CountryTitle_en = y.CountryTitle_en }
					);
			}
			else
			{
				foreach (var cooke in cookies.ToList())
				{
					if (cooke.Key != "Login")
					{
						citiesAndCountries = citiesAndCountries.Append<CityAndCountry>(db.citiesAndCountries.FirstOrDefault(x => x.Id.ToString() == cooke.Value));
					}
				}
			}

			return await PostWeather(page,filter,sortingState);
		}
	}
}
