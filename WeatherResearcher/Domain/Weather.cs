namespace Domain
{
    public class Weather
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

        public Snapshot ParseToSnapshot()
        {
            return new Snapshot()
            {
                weather = weather,
                icon = icon,
                temp = temp,
                temp_feels_like = temp_feels_like,
                temp_min = temp_min,
                temp_max = temp_max,
                pressure = pressure,
                humidity = humidity,
                wind_speed = wind_speed
            };
        }
    }
}
