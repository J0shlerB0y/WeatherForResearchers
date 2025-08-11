
using System.Linq.Expressions;
using System.Linq;
using System;
using Domain;
using UseCases.Services;

namespace UseCases.RepoSpecifications.Sorters;

public class UsersCitySorterSpec : ISorter<UsersCity>
{
    private SortingEnum sortingState;

    public UsersCitySorterSpec(SortingEnum sortingState)
    {
        this.sortingState = sortingState;
    }

    public IQueryable<UsersCity> ApplySorting(IQueryable<UsersCity> cities)
    {
        switch (sortingState)
        {
            case SortingEnum.CityAsc:
                cities = cities.OrderBy(c => c.City.CityTitle_en);
                break;
            case SortingEnum.CountryAsc:
                cities = cities.OrderBy(c => c.City.Country.CountryTitle_en);
                break;

            case SortingEnum.CityDesc:
                cities = cities.OrderByDescending(c => c.City.CityTitle_en);
                break;
            case SortingEnum.CountryDesc:
                cities = cities.OrderByDescending(c => c.City.Country.CountryTitle_en);
                break;
        }

        return cities;
    }
}