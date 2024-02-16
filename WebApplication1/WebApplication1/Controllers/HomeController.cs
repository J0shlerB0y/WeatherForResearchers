using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Text;
using WeatherResearcher.Models;
using WeatherResearcher.Services;

namespace WeatherResearcher.Controllers
{
    public class HomeController : Controller
	{
		private ApplicationContext db;
		private int count;
		private int pageSize = 12;
		private List<CityAndCountry> pageCitiesAndCountriesList;
		private Queue<CityAndCountry> pageCitiesAndCountriesQueue;
		private IQueryable<CityAndCountry> citiesAndCountries;
		private List<WeatherModel> WeatherForView;
		private readonly ILogger<HomeController> _logger;
		private bool isLogedIn = false;
		private UsersCity userCity;

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
			//filtration
			if (!String.IsNullOrEmpty(filter.City))
			{
				citiesAndCountries = citiesAndCountries.Where(c => filter.City == c.CityTitle_en);
			}
			if (!String.IsNullOrEmpty(filter.Country))
			{
				citiesAndCountries = citiesAndCountries.Where(c => filter.Country == c.CountryTitle_en || filter.Country == c.CountryTitle_en);
			}

			//sorting
			switch (sortingState)
			{
				case SortingEnum.CityAsc:
					citiesAndCountries = citiesAndCountries.OrderBy(s => s.CityTitle_en);
					break;
				case SortingEnum.CountryAsc:
					citiesAndCountries = citiesAndCountries.OrderBy(s => s.CountryTitle_en);
					break;
				case SortingEnum.CityDesc:
					citiesAndCountries = citiesAndCountries.OrderByDescending(s => s.CityTitle_en);
					break;
				case SortingEnum.CountryDesc:
					citiesAndCountries = citiesAndCountries.OrderByDescending(s => s.CountryTitle_en);
					break;
			}

			//building items for Page
			count = citiesAndCountries.Count();
			pageCitiesAndCountriesList = citiesAndCountries.Skip(page * pageSize).Take(pageSize).ToList();
			pageCitiesAndCountriesQueue = new Queue<CityAndCountry>(pageCitiesAndCountriesList);

			//limiting pageSize
			pageSize = pageCitiesAndCountriesList.Count();

			//finding weather
			WeatherForView = new List<WeatherModel>();
			while (WeatherForView.Count < pageSize)
			{
				WeatherModel tempWeatherModel = GetWeather(pageCitiesAndCountriesQueue.Dequeue(), page);
				if (tempWeatherModel != null)
				{
					WeatherForView.Add(tempWeatherModel);
				}
			}
			//building view Model
			ForIndexViewModel forIndexViewModel = new ForIndexViewModel(
				isLogedIn,
				pageCitiesAndCountriesList.ToList(),
				new PageViewModel(count, page, pageSize),
				WeatherForView,
				sortingState
			)
			{
				filter = new FilterViewModel()
				{
					City = filter.City,
					Country = filter.Country
				}
			};
			return View(forIndexViewModel);
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
		public WeatherModel GetWeather(CityAndCountry cityAndCountryToFindWeather, int page = 0)
		{
			string cityToFindWeather = cityAndCountryToFindWeather.CityTitle_en;
			HttpWebRequest request =
		(HttpWebRequest)WebRequest.Create($"https://api.openweathermap.org/data/2.5/weather?q={cityToFindWeather}&appid=fe4cbc13f433881a75daf2150465acd2&units=metric");

			request.Method = "GET";
			request.Accept = "application/json";
			request.UserAgent = "Mozilla/5.0 ....";
			WeatherModel outputWeather = new WeatherModel();
			try
			{
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				StreamReader reader = new StreamReader(response.GetResponseStream());
				StringBuilder output = new StringBuilder();
				output.Append(reader.ReadToEnd());
				response.Close();
				JObject json = JObject.Parse(output.ToString());

				//weather, icon, temp, temp feels like, temp min, temp max, pressure, humidity, wind speed 
				JObject jsonWeather = JObject.Parse(json.GetValue("weather").FirstOrDefault().ToString());
				outputWeather.weather = jsonWeather.GetValue("description").ToString();

				outputWeather.icon = $"https://openweathermap.org/img/wn/{jsonWeather.GetValue("icon").ToString()}@2x.png";

				JObject jsonputMain = JObject.Parse(json.GetValue("main").ToString());
				outputWeather.temp = jsonputMain.GetValue("temp").ToString();

				outputWeather.temp_feels_like = jsonputMain.GetValue("feels_like").ToString();

				outputWeather.temp_min = jsonputMain.GetValue("temp_min").ToString();

				outputWeather.temp_max = jsonputMain.GetValue("temp_max").ToString();

				outputWeather.pressure = jsonputMain.GetValue("pressure").ToString();

				outputWeather.humidity = jsonputMain.GetValue("humidity").ToString();

				JObject jsonWind = JObject.Parse(json.GetValue("wind").ToString());
				outputWeather.wind_speed = jsonWind.GetValue("speed").ToString();
			}
			catch
			{
				db.citiesAndCountries.Remove(cityAndCountryToFindWeather);
				pageCitiesAndCountriesQueue.Enqueue(citiesAndCountries.Skip(page * pageSize + pageSize).FirstOrDefault());
				db.SaveChanges();
				return null;
			}
			return outputWeather;
		}
	}
}