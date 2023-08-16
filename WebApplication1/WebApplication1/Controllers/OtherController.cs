using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{

    public class OtherController : Controller
	{
		private ApplicationContext db;
		private int count;
        private const int pageSize = 12;
        private List<CityAndCountry> pageCitiesAndCountries;
        private IQueryable<CityAndCountry> citiesAndCountries;
        private List<WeatherModel> WeatherForView;
        private readonly ILogger<HomeController> _logger;
        public OtherController(ILogger<HomeController> logger, ApplicationContext ContextDb)
		{
			_logger = logger;
			db = ContextDb;
		}
		public async Task<IActionResult> OwnCabinet(int page = 0,
            FilterViewModel filter = null,
            SortingEnum sortingState = SortingEnum.CityAsc)
        {
            citiesAndCountries = Enumerable.Empty<CityAndCountry>().AsQueryable();
            //fetching and recycling cookies

            var cookies = HttpContext.Request.Cookies;

            if (cookies["Login"] != null && cookies["Password"] != null)
            {
				citiesAndCountries = db.userscities.Join(db.users,
                    x => x.UserId,
                    y => y.Id,
                    (x, y) => x.CityId
                    ).Join(db.citiesAndCountries,
                    x => x,
                    y => y.Id,
                    (x,y)=>new CityAndCountry {Id = x, CityTitle_en = y.CityTitle_en , CountryTitle_en = y.CountryTitle_en}
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

                        citiesAndCountries = citiesAndCountries.Append<CityAndCountry>(db.citiesAndCountries.Where(x => x.Id.ToString() == cooke.Value).FirstOrDefault());
                    }
                }
            }
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
            pageCitiesAndCountries = citiesAndCountries.Skip(page * pageSize).Take(pageSize).ToList();

            //finding weather
            WeatherForView = new List<WeatherModel>();
            foreach (CityAndCountry cityAndCountry in pageCitiesAndCountries)
            {
                WeatherForView.Add(GetWeather(cityAndCountry.CityTitle_en));
            }

            //building view Model
            ForOwnCabinetViewModel forOwnCabinetViewModel = new ForOwnCabinetViewModel(
                pageCitiesAndCountries,
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
            return View(forOwnCabinetViewModel);
        }
        public WeatherModel GetWeather(string cityToFindWeather)
        {
            HttpWebRequest request =
            (HttpWebRequest)WebRequest.Create($"https://api.openweathermap.org/data/2.5/weather?q={cityToFindWeather}&appid=fe4cbc13f433881a75daf2150465acd2&units=metric");

            request.Method = "GET";
            request.Accept = "application/json";
            request.UserAgent = "Mozilla/5.0 ....";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            StringBuilder output = new StringBuilder();
            output.Append(reader.ReadToEnd());
            response.Close();
            JObject json = JObject.Parse(output.ToString());


            WeatherModel outputWeather = new WeatherModel();
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

            return outputWeather;
        }
    }
}
