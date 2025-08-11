using Domain;
using System.Linq.Expressions;
using UseCases.Services;

namespace UseCases.RepoSpecifications.Sorters;

public class CitySorterSpec : BaseSorter<City>
{
    private static Dictionary<SortingEnum, Expression<Func<City, object>>> sortingAscExpression =
        new()
        {
            {SortingEnum.CityAsc, s => s.CityTitle_en },
            { SortingEnum.CountryAsc, s => s.Country.CountryTitle_en }
        };

    public CitySorterSpec(SortingEnum sortingState) : base(sortingState, sortingAscExpression) {}
}