
using Domain;

namespace UseCases.RepoSpecifications.Filters;

public class CityFilterCriteriaSpec : BaseCriteriaSpec<City>
{
    public CityFilterCriteriaSpec(FilterCityParametr filter) : base(
        c => (string.IsNullOrEmpty(filter.City) || filter.City == c.CityTitle_en) &&
            (string.IsNullOrEmpty(filter.Country) || filter.Country == c.Country.CountryTitle_en)
        )
    {}
}