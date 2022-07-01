namespace WebApplication1.Models
{
    public class PageViewModel
    {
        public int pageCount { get; private set;}
        public int pageNumber { get; private set; }

        public PageViewModel(int count, int page, int pageSize)
        {
            pageCount = count/ pageSize;
            pageNumber = page;
        }
        public bool HasPreviousPage()
        {
            if(pageNumber-1 <= 0)
                return false;
            return true;
        }
        public bool HasNextPage()
        {
            if (pageNumber+1 >= pageCount)
                return false;
            return true;
        }
    }
}
