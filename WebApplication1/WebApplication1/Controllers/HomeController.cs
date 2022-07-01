using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        public int count;
        public const int pageSize = 18;
        public IQueryable<City> cities;
        public List<City> citiesList;
        public IQueryable<Country> countries;
        public IQueryable<City> pageCities;
        public IQueryable<CityAndCountry> citiesAndCountries;
        public IQueryable<CityAndCountry> pageCitiesAndCountries;
        private readonly ILogger<HomeController> _logger;
        private Country tempCountryMatchCity;

        public HomeController(ILogger<HomeController> logger, ApplicationContext ContextDb)
        {
            _logger = logger;
            db = ContextDb;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            cities = db._cities;
            countries = db._countries;
            citiesAndCountries = Enumerable.Empty<CityAndCountry>().AsQueryable();

            count = await cities.CountAsync();
            pageCities = cities.Skip((page-1)* pageSize).Take(pageSize);

            citiesList = await pageCities.ToListAsync();
            foreach (City city in citiesList)
            {
                tempCountryMatchCity = await countries.SingleOrDefaultAsync(x => x.Country_id == city.Country_id);
                citiesAndCountries = citiesAndCountries.Append<CityAndCountry>(
                    new CityAndCountry(city.Title_ru,
                    tempCountryMatchCity.Title_ru,
                    tempCountryMatchCity.Title_en)
                    );
            }

            ForIndexViewModel forIndexViewModel = new ForIndexViewModel(
                citiesAndCountries,
                new PageViewModel(count,page,pageSize)
                );
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
        public List<string> GetWeather(string cityToFindWeather)
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

            //weather, icon, temp, temp feels like, temp min, temp max, pressure, humidity, wind speed, 
            List<string> outputWeather = new List<string>() {
                JObject.Parse(json.GetValue("weather").FirstOrDefault().ToString()).GetValue("description").ToString(),
                JObject.Parse(json.GetValue("weather").FirstOrDefault().ToString()).GetValue("icon").ToString(),

                JObject.Parse(json.GetValue("main").FirstOrDefault().ToString()).GetValue("temp").ToString(),
                JObject.Parse(json.GetValue("main").FirstOrDefault().ToString()).GetValue("feels_like").ToString(),
                JObject.Parse(json.GetValue("main").FirstOrDefault().ToString()).GetValue("temp_min").ToString(),
                JObject.Parse(json.GetValue("main").FirstOrDefault().ToString()).GetValue("temp_max").ToString(),
                JObject.Parse(json.GetValue("main").FirstOrDefault().ToString()).GetValue("pressure").ToString(),
                JObject.Parse(json.GetValue("main").FirstOrDefault().ToString()).GetValue("humidity").ToString(),

                JObject.Parse(json.GetValue("wind").FirstOrDefault().ToString()).GetValue("speed").ToString(),
        };
            json = JObject.Parse(json.GetValue("weather").FirstOrDefault().ToString());
            outputWeather.Add(json.GetValue("main").ToString());
            return outputWeather;
        }
    }
}