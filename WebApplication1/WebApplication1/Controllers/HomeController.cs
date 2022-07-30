using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        public int count;
        public const int pageSize = 12;
        public List<CityAndCountry> pageCitiesAndCountries;
        public IQueryable<CityAndCountry> citiesAndCountries;
        public List<WeatherModel> WeatherForView;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationContext ContextDb)
        {
            _logger = logger;
            db = ContextDb;
        }

        public async Task<IActionResult> Index(int page = 0,
            FilterViewModel filter = null,
            SortingEnum sortingState = SortingEnum.CityAsc)
        {

            citiesAndCountries = db._cityandcountry;
            //filtration
            if (!String.IsNullOrEmpty(filter.City))
            {
                citiesAndCountries = citiesAndCountries.Where(c => filter.City == c.CityTitle_ru);
            }
            if (!String.IsNullOrEmpty(filter.Country))
            {
                citiesAndCountries = citiesAndCountries.Where(c => filter.Country == c.CountryTitle_ru || filter.Country == c.CountryTitle_en);
            }

            //sorting
            switch (sortingState)
            {
                case SortingEnum.CityAsc:
                    citiesAndCountries = citiesAndCountries.OrderBy(s => s.CityTitle_ru);
                    break;
                case SortingEnum.CountryAsc:
                    citiesAndCountries = citiesAndCountries.OrderBy(s => s.CountryTitle_ru);
                    break;
                case SortingEnum.CityDesc:
                    citiesAndCountries = citiesAndCountries.OrderByDescending(s => s.CityTitle_ru);
                    break;
                case SortingEnum.CountryDesc:
                    citiesAndCountries = citiesAndCountries.OrderByDescending(s => s.CountryTitle_ru);
                    break;
            }

            //building items for Page
            count = await citiesAndCountries.CountAsync();
            pageCitiesAndCountries = await citiesAndCountries.Skip(page * pageSize).Take(pageSize).ToListAsync();

            //finding weather
            WeatherForView = new List<WeatherModel>();
            foreach (CityAndCountry cityAndCountry in pageCitiesAndCountries)
            {
                WeatherForView.Add(GetWeather(cityAndCountry.CityTitle_ru));
            }

            //building view Model
            ForIndexViewModel forIndexViewModel = new ForIndexViewModel(
                pageCitiesAndCountries,
                new PageViewModel(count, page, pageSize),
                WeatherForView,
                sortingState
            )
            {
                filter = new FilterViewModel() { 
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