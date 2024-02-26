namespace WeatherResearcher.Models
{
    public class ForWeatherModel
    {
        public List<CityAndCountry> cityAndCountry { get;  set; }
        public PageViewModel pageViewModel { get; set; }
        public List<WeatherModel> weather { get; set; }
        public FilterViewModel filter { get; set; }
        public SortingEnum sortingState { get; set; }
        public bool isLogedIn { get; set; }
		public ForWeatherModel(List<CityAndCountry> cityAndCountry, PageViewModel pageViewModel, List<WeatherModel> weather, SortingEnum sortingStatebool,bool isLogedIn = false)
        {
            this.isLogedIn = isLogedIn;
			this.cityAndCountry = cityAndCountry;
            this.pageViewModel = pageViewModel;
            this.weather = weather;
            this.sortingState = sortingState;
        }
    }
}