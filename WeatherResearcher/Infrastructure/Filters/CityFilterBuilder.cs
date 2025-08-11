
using Domain;
using UseCases.Builders;

namespace Infrastructure.Filters
{
    public class CityFilterBuilder : ICityFilterBuilder<City>
    {
        private IQueryable<City> cities;
        private FilterCityParametr filter;
        public CityFilterBuilder(FilterCityParametr filter)
        {
            this.filter = filter ?? throw new ArgumentNullException(nameof(filter));
        }

        public void CreatBuilder(IQueryable<City> query)
        {
            this.cities = query ?? throw new ArgumentNullException(nameof(cities));
        }

        public IFilterBuilder<City> ApplyAllFilters()
        {
            return this
                .FilterByCity()
                .FilterByCountry();
        }

        public IQueryable<City> Build()
        {
            return cities;
        }

        public ICityFilterBuilder<City> FilterByCity()
        {
            if (!string.IsNullOrEmpty(filter.City))
            {
                cities = cities.Where(c => filter.City == c.CityTitle_en);
            }
            return this;
        }

        public ICityFilterBuilder<City> FilterByCountry()
        {
            if (!string.IsNullOrEmpty(filter.Country))
            {
                cities = cities.Where(c => filter.Country == c.Country.CountryTitle_en);
            }
            return this;
        }

    }
}