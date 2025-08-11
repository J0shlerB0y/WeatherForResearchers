using Domain;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using UseCases.API;
using UseCases.Builders;
using UseCases.Mutations;

namespace Infrastructure.Builders
{
    internal abstract class AWeatherShowerPageBuilder<Entity, FilterParams, ItemModel> 
        where FilterParams : class 
        where Entity : class,IEntity
    {
        protected IReadWithAdditionalToolsRepository<Entity> repo;
        protected int count;
        protected int pageSize = 12;
        protected List<Entity> pageItemsList;

        internal AWeatherShowerPageBuilder(IReadWithAdditionalToolsRepository<Entity> repo)
        {
            this.repo = repo;
        }

        internal AWeatherShowerPageBuilder<Entity, FilterParams, ItemModel> Paginate(int page = 0)
        {
            count = repo.Count();
            repo.Paginate(page, pageSize);
            pageItemsList = repo.GetTheRest()?.ToList();
            return this;
        }

        internal abstract AWeatherShowerPageBuilder<Entity, FilterParams, ItemModel> Filter(FilterParams filterParametr);

        internal abstract AWeatherShowerPageBuilder<Entity, FilterParams, ItemModel> Sort(SortingEnum sortingEnum);
        internal abstract AWeatherShowerPageBuilder<Entity, FilterParams, ItemModel> FindWeather(IWeatherAPI weatherAPI);

        internal abstract ViewModel<ItemModel, FilterParams> buildModel(int page = 0,
            FilterParams? filter = null,
            SortingEnum sortingState = SortingEnum.CityAsc);

        internal ErrorViewModel Error(int statusCode = 200)
        {
            string errorMessange = "";
            if (statusCode == 401)
            {
                errorMessange = "Unauthorized";
            }
            else if (statusCode == 404)
            {
                errorMessange = "Not Found";
            }
            else if (statusCode == 408)
            {
                errorMessange = "Request Timeout";
            }
            else if (statusCode == 500)
            {
                errorMessange = "Internal Server Error";
            }
            else if (statusCode == 310)
            {
                errorMessange = "Too Many Redirects";
            }
            return new ErrorViewModel { StatusCode = statusCode, ErrorMessage = errorMessange };
        }
    }
}
