using Microsoft.IdentityModel.Tokens;

namespace Domain
{
	public class FilterSnapshotParametr : FilterCityParametr
	{
		public WeatherWithTime? TopWeather { get; set; } = new WeatherWithTime();
		public WeatherWithTime? BottomWeather { get; set; } = new WeatherWithTime();

		public override void ClearBySpaces()
		{
			base.ClearBySpaces();

			if (TopWeather.weather.IsNullOrEmpty())
			{
				TopWeather.weather.Trim();
			}
			if (BottomWeather.weather.IsNullOrEmpty())
			{
				BottomWeather.weather.Trim();
			}
		}
    }
}
