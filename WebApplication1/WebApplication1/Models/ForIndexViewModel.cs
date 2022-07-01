namespace WebApplication1.Models
{
    public class ForIndexViewModel
    {
        public IQueryable<CityAndCountry> cityAndCountry { get; set; }
        public PageViewModel pageViewModel { get; set; }
        public ForIndexViewModel(IQueryable<CityAndCountry> cityAndCountry, PageViewModel pageViewModel)
        {
            this.cityAndCountry = cityAndCountry;
            this.pageViewModel = pageViewModel;
        }
    }
}
