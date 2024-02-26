using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using WeatherResearcher.Models;
using WeatherResearcher.Services;

namespace WeatherResearcher.Controllers
{
	public class OwnCabinetController : AbstractWeatherShowerController
	{
		private readonly ILogger<HomeController> _logger;

		public OwnCabinetController(ILogger<HomeController> logger, ApplicationContext ContextDb)
		{
			_logger = logger;
			db = ContextDb;
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
						(x)=>
						(x.Password == cookies["Password"]) && 
						(x.Login == cookies["Login"])
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
					if (cooke.Key != "Login" && cooke.Key != "Password")
					{
						var tttt = cooke.Value;
						var yyyy = db.citiesAndCountries.Where(x => x.Id.ToString() == cooke.Value).FirstOrDefault();

						citiesAndCountries = citiesAndCountries.Append<CityAndCountry>(db.citiesAndCountries.FirstOrDefault(x => x.Id.ToString() == cooke.Value));
					}
				}
			}

			return await PostWeather(page,filter,sortingState);
		}
	}
}
