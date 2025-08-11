using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCases.Builders;

namespace Infrastructure.Filters
{
    public class UsersCityFilterBuilder
    : IFilterBuilder<UsersCity>
    {
        private IQueryable<UsersCity> cities;
        private FilterCityParametr filter;
        public UsersCityFilterBuilder(FilterCityParametr filter)
        {
            this.filter = filter ?? throw new ArgumentNullException(nameof(cities));
        }

        public void CreatBuilder(IQueryable<UsersCity> query)
        {
            this.cities = query ?? throw new ArgumentNullException(nameof(filter));
        }

        public IFilterBuilder<UsersCity> ApplyAllFilters()
        {
            return this
                .FilterByCity()
                .FilterByCountry();
        }

        public IQueryable<UsersCity> Build()
        {
            return cities;
        }

        public UsersCityFilterBuilder FilterByCity()
        {
            if (!string.IsNullOrEmpty(filter.City))
            {
                cities = cities.Where(c => filter.City == c.City.CityTitle_en);
            }
            return this;
        }

        public UsersCityFilterBuilder FilterByCountry()
        {
            if (!string.IsNullOrEmpty(filter.Country))
            {
                cities = cities.Where(c => filter.Country == c.City.Country.CountryTitle_en);
            }
            return this;
        }

    }
}
