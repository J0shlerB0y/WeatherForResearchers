using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Text;
using WeatherResearcher.Models;
using WeatherResearcher.Services;

namespace WeatherResearcher.Controllers
{
	public abstract class AbstractWeatherShowerController : Controller
	{
		protected ForWeatherModel forIndexViewModel;
		protected ApplicationContext db;
		protected int count;
		protected int pageSize = 12;
		protected List<CityAndCountry> pageCitiesAndCountriesList;
		protected Queue<CityAndCountry> pageCitiesAndCountriesQueue;
		protected IQueryable<CityAndCountry> citiesAndCountries;
		protected List<WeatherModel> WeatherForView;
		protected bool isLogedIn = false;

		protected void filtration(FilterViewModel filter = null)
		{
			if (!String.IsNullOrEmpty(filter.City))
			{
				citiesAndCountries = citiesAndCountries.Where(c => filter.City == c.CityTitle_en);
			}
			if (!String.IsNullOrEmpty(filter.Country))
			{
				citiesAndCountries = citiesAndCountries.Where(c => filter.Country == c.CountryTitle_en || filter.Country == c.CountryTitle_en);
			}
		}

		protected void sorting(SortingEnum sortingState = SortingEnum.CityAsc)
		{
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
		}

		protected void buildingItems(int page = 0)
		{
			count = citiesAndCountries.Count();
			pageCitiesAndCountriesList = citiesAndCountries.Skip(page * pageSize).Take(pageSize).ToList();
			pageCitiesAndCountriesQueue = new Queue<CityAndCountry>(pageCitiesAndCountriesList);
		}

		protected void findingWeather(int page = 0)
		{
			WeatherForView = new List<WeatherModel>();
			while (WeatherForView.Count < pageSize)
			{
				WeatherModel tempWeatherModel = GetWeather(pageCitiesAndCountriesQueue.Dequeue(), page);
				if (tempWeatherModel != null)
				{
					WeatherForView.Add(tempWeatherModel);
				}
			}
		}

		protected void buildingModel(int page = 0,
			FilterViewModel filter = null,
			SortingEnum sortingState = SortingEnum.CityAsc)
		{
			forIndexViewModel = new ForWeatherModel(
				pageCitiesAndCountriesList.ToList(),
				new PageViewModel(count, page, pageSize),
				WeatherForView,
				sortingState,
				isLogedIn
			)
			{
				filter = new FilterViewModel()
				{
					City = filter.City,
					Country = filter.Country
				}
			};
		}

		protected async Task<IActionResult> PostWeather(int page = 0,
			FilterViewModel filter = null,
			SortingEnum sortingState = SortingEnum.CityAsc)
		{
			filtration(filter);

			sorting(sortingState);

			buildingItems(page);

			pageSize = pageCitiesAndCountriesList.Count();

			findingWeather(page);

			buildingModel(page, filter, sortingState);

			return View(forIndexViewModel);
		}
		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		protected WeatherModel GetWeather(CityAndCountry cityAndCountryToFindWeather, int page = 0)
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
				outputWeather.temp = (int)jsonputMain.GetValue("temp");

				outputWeather.temp_feels_like = (int)jsonputMain.GetValue("feels_like");

				outputWeather.temp_min = (int)jsonputMain.GetValue("temp_min");

				outputWeather.temp_max = (int)jsonputMain.GetValue("temp_max");

				outputWeather.pressure = (int)jsonputMain.GetValue("pressure");

				outputWeather.humidity = (int)jsonputMain.GetValue("humidity");

				JObject jsonWind = JObject.Parse(json.GetValue("wind").ToString());
				outputWeather.wind_speed = (int)jsonWind.GetValue("speed");
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
