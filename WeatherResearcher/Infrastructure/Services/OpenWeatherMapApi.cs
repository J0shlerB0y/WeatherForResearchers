using Domain;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Net;
using System.Text;
using UseCases.API;

namespace Infrastructure.Services
{
    public class OpenWeatherMapApi : IWeatherAPI
    {
        string openWeatherMapKey;
        public OpenWeatherMapApi(IConfiguration configuration)
        {
            openWeatherMapKey = configuration["OpenWeatherMapKey"];
            if (openWeatherMapKey == null) 
                throw new ArgumentNullException(nameof(openWeatherMapKey));
        }
        public Weather GetWeather(City cityToFindWeather)
        {
            string cityToFindWeatherTitle = cityToFindWeather.CityTitle_en;
            HttpWebRequest request =
        (HttpWebRequest)WebRequest.Create($"https://api.openweathermap.org/data/2.5/weather?q={cityToFindWeatherTitle}&appid={openWeatherMapKey}&units=metric");

            request.Method = "GET";
            request.Accept = "application/json";
            request.UserAgent = "Mozilla/5.0 ....";
            Weather outputWeather = new Weather();
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
                //db.Cities.Remove(cityAndCountryToFindWeather);
                //pageCitiesQueue.Enqueue(Cities.Skip(page * pageSize + pageSize).FirstOrDefault());
                //db.SaveChanges();
                return null;
            }
            return outputWeather;
        }
    }
}
