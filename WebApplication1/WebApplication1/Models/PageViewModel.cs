namespace WeatherResearcher.Models
{
    public class PageViewModel
    {
        public int citiesCount { get; private set;}
        public int page { get; private set; }
        public int pageSize { get; private set; }

        public PageViewModel(int count, int page, int pageSize)
        {
            this.pageSize = pageSize;
            citiesCount = count;
            this.page = page;
        }
        public bool HasPreviousPage()
        {
            if(page <= 0)
                return false;
            return true;
        }
        public bool HasNextPage()
        {
            if ((page + 1) * pageSize >= citiesCount)
                return false;
            return true;
        }
    }
}
