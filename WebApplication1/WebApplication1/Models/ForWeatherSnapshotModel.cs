namespace WeatherResearcher.Models
{
	public class ForWeatherSnapshotModel
	{
		public List<SnapshotWithCityModel> snapshotsWithCitiesAndCountries { get; set; }
		public PageViewModel pageViewModel { get; set; }
		public FilterForSnapshotViewModel filter { get; set; }
		public SortingEnum sortingState { get; set; }
		public bool isLogedIn { get; set; }
		public ForWeatherSnapshotModel(List<SnapshotWithCityModel> snapshotsWithCitiesAndCountries, PageViewModel pageViewModel, SortingEnum sortingStatebool, bool isLogedIn = false)
		{
			this.isLogedIn = isLogedIn;
			this.snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries;
			this.pageViewModel = pageViewModel;
			this.sortingState = sortingState;
		}
	}
}
