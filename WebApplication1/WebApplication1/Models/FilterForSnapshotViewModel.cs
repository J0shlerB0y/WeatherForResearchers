namespace WeatherResearcher.Models
{
	public class FilterForSnapshotViewModel : FilterViewModel
	{
		public WeatherSnapshotModel? TopWeather { get; set; } = new WeatherSnapshotModel();
		public WeatherSnapshotModel? BottomWeather { get; set; } = new WeatherSnapshotModel();
	}
}
