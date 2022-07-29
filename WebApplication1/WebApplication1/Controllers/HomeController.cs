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
        public const int pageSize = 18;
        public List<CityAndCountry> pageCitiesAndCountries;
        public IQueryable<CityAndCountry> citiesAndCountries;
        public List<WeatherModel> WeatherForView;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationContext ContextDb)
        {
            _logger = logger;
            db = ContextDb;
        }

        public async Task<IActionResult> Index(int page = 0, FilterViewModel filter = null)
        {
            citiesAndCountries = db._cityandcountry;
            if (filter.City != null)
            {
                citiesAndCountries = citiesAndCountries.Where(c => filter.City == c.CityTitle_ru);
            }
            if (filter.Country != null)
            {
                citiesAndCountries = citiesAndCountries.Where(c => filter.Country == c.CountryTitle_ru || filter.Country == c.CountryTitle_en);
            }
            WeatherForView = new List<WeatherModel>();

            count = await citiesAndCountries.CountAsync();
            pageCitiesAndCountries = await citiesAndCountries.Skip(page * pageSize).Take(pageSize).ToListAsync();
            //нужно связать погоду с названиями для вьюшки
            foreach (CityAndCountry cityAndCountry in pageCitiesAndCountries)
            {
                WeatherForView.Add(GetWeather(cityAndCountry.CityTitle_ru));
            }
            ForIndexViewModel forIndexViewModel = new ForIndexViewModel(
                pageCitiesAndCountries,
                new PageViewModel(count, page, pageSize),
                WeatherForView
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

            outputWeather.icon = jsonWeather.GetValue("icon").ToString();

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