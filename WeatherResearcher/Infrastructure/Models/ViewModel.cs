using Domain;

namespace Infrastructure.Models
{
    public class ViewModel<Entity,Filter>
    {
        public List<Entity> items { get;  set; }
        public PageModel pageViewModel { get; set; }
        public Filter filter { get; set; }
        public SortingEnum sortingState { get; set; }
		public ViewModel(List<Entity> items, PageModel pageViewModel, SortingEnum sortingStatebool)
        {
			this.items = items;
            this.pageViewModel = pageViewModel;
            this.sortingState = sortingState;
        }
    }
}