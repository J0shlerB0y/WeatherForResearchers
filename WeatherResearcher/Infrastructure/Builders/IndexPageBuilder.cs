using Domain;
using Infrastructure.Models;
using Infrastructure.Sorters;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Drawing.Printing;
using UseCases.API;
using UseCases.Builders;
using UseCases.Mutations;
using UseCases.RepoSpecifications.Filters;

namespace Infrastructure.Builders
{
    internal class IndexPageBuilder : AWeatherShowerPageBuilder<City, FilterCityParametr, WeatherWithCityModel>
    {
        protected List<WeatherWithCityModel> weatherWithCityList = new List<WeatherWithCityModel> ();

        protected ViewModel<WeatherWithCityModel, FilterCityParametr> viewModel;

        internal IndexPageBuilder(IReadWithAdditionalToolsRepository<City> repo) : base(repo) { }

        internal override IndexPageBuilder FindWeather(IWeatherAPI weatherAPI)
        {
            foreach(City city in pageItemsList) 
            { 
                Weather weatherToCheck = weatherAPI.GetWeather(city);
                if (weatherToCheck != null)
                {
                    weatherWithCityList.Add(new WeatherWithCityModel { city = city, weather = weatherToCheck });
                }
            }
            return this;
        }
        internal override ViewModel<WeatherWithCityModel, FilterCityParametr> buildModel(int page = 0,
            FilterCityParametr filter = null,
            SortingEnum sortingState = SortingEnum.CityAsc)
        {
            return new ViewModel<WeatherWithCityModel, FilterCityParametr>(
                weatherWithCityList,
                new PageModel(count, page, pageSize),
                sortingState
            )
            {
                filter = filter
            };
        }

        internal override IndexPageBuilder Filter(FilterCityParametr filterParametr)
        {
            filterParametr.ClearBySpaces();
            repo.Filter(new CityFilterCriteriaSpec(filterParametr));
            return this;
        }

        internal override IndexPageBuilder Sort(SortingEnum sortingEnum)
        {
            repo.Sort(new CitySorter(sortingEnum));
            return this;
        }
    }
}
