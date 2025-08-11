using Domain;
using Infrastructure.Models;
using Infrastructure.Sorters;
using UseCases.API;
using UseCases.Mutations;
using UseCases.RepoSpecifications;
using UseCases.RepoSpecifications.Filters;

namespace Infrastructure.Builders
{
    internal class SubscriptionPageBuilder : AWeatherShowerPageBuilder<UsersCity, FilterCityParametr, WeatherWithCityModel>
    {
        protected List<WeatherWithCityModel> weatherWithCityList = new List<WeatherWithCityModel>();

        protected ViewModel<WeatherWithCityModel, FilterCityParametr> viewModel;

        internal SubscriptionPageBuilder(IReadWithAdditionalToolsRepository<UsersCity> repo, User user) 
            : base(repo) 
        { repo.Filter(new UsersCityFilterCriteriaSpec(user)); }

        internal override ViewModel<WeatherWithCityModel, FilterCityParametr> buildModel(int page = 0, FilterCityParametr filter = null, SortingEnum sortingState = SortingEnum.CityAsc)
        {
            return viewModel = new ViewModel<WeatherWithCityModel, FilterCityParametr>(
                weatherWithCityList,
                new PageModel(count, page, pageSize),
                sortingState
            )
            {
                filter = filter
            };
        }

        internal override SubscriptionPageBuilder Filter(FilterCityParametr filterParametr)
        {
            filterParametr.ClearBySpaces();
            repo.Filter(new UsersCityFilterCriteriaSpec(filterParametr));
            return this;
        }

        internal override SubscriptionPageBuilder FindWeather(IWeatherAPI weatherAPI)
        {
            foreach (UsersCity usersCity in pageItemsList)
            {
                Weather weatherToCheck = weatherAPI.GetWeather(usersCity.City);
                if (weatherToCheck != null)
                {
                    weatherWithCityList.Add(new WeatherWithCityModel { city = usersCity.City, weather = weatherToCheck });
                }
            }
            return this;
        }

        internal override SubscriptionPageBuilder Sort(SortingEnum sortingEnum)
        {
            repo.Sort(new UsersCitySorter(sortingEnum));
            return this;
        }
    }
}
