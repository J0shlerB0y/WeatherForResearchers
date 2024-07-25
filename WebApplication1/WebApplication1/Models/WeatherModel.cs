namespace WeatherResearcher.Models
{
    public class WeatherModel
    {
        public string weather { get; set; } = "";
        public string icon { get; set; } = "";
        public float? temp { get; set; } = null;
        public float? temp_feels_like { get; set; } = null;
        public float? temp_min { get; set; } = null;
        public float? temp_max { get; set; } = null;
        public float? pressure { get; set; } = null;
        public float? humidity { get; set; } = null;
        public float? wind_speed { get; set; } = null;
    }
}
