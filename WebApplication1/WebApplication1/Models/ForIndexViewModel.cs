namespace WebApplication1.Models
{

    public class ForIndexViewModel
    {
        public List<CityAndCountry> cityAndCountry { get;  set; }
        public PageViewModel pageViewModel { get; set; }
        public List<WeatherModel> weather { get; set; }
        public FilterViewModel filter { get; set; }
        public ForIndexViewModel(List<CityAndCountry> cityAndCountry, PageViewModel pageViewModel, List<WeatherModel> weather)
        {
            this.cityAndCountry = cityAndCountry;
            this.pageViewModel = pageViewModel;
            this.weather = weather;
        }
    }
}