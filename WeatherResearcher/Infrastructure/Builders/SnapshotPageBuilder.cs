using Domain;
using Infrastructure.Models;
using Infrastructure.Sorters;
using UseCases.API;
using UseCases.Mutations;
using UseCases.RepoSpecifications.Filters;

namespace Infrastructure.Builders
{
    internal class SnapshotPageBuilder : AWeatherShowerPageBuilder<Snapshot, FilterSnapshotParametr, Snapshot>
    {
        private ViewModel<Snapshot, FilterSnapshotParametr> viewModel;
        internal SnapshotPageBuilder(IReadWithAdditionalToolsRepository<Snapshot> repo, User user)
            : base(repo)
        { repo.Filter(new SnapshotFilterCriteriaSpec(user)); }


        internal override ViewModel<Snapshot, FilterSnapshotParametr> buildModel(int page = 0, FilterSnapshotParametr filter = null, SortingEnum sortingState = SortingEnum.CityAsc)
        {
            return new ViewModel<Snapshot, FilterSnapshotParametr>(
                pageItemsList.Cast<Snapshot>().ToList(),
                new PageModel(count, page, pageSize),
                sortingState
            )
            {
                filter = filter
            };
        }

        internal override SnapshotPageBuilder Filter(FilterSnapshotParametr filterParametr)
        {
            filterParametr.ClearBySpaces();
            repo.Filter(new SnapshotFilterCriteriaSpec(filterParametr));
            return this;
        }

        internal override SnapshotPageBuilder FindWeather(IWeatherAPI weatherAPI)
        {
            throw new NotImplementedException();
        }

        internal override SnapshotPageBuilder Sort(SortingEnum sortingEnum)
        {
            repo.Sort(new SnapshotSorter(sortingEnum));
            return this;
        }
    }
}
