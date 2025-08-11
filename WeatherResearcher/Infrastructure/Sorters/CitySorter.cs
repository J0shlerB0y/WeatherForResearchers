
using System.Linq.Expressions;
using System.Linq;
using System;
using Domain;
using UseCases.Services;

namespace Infrastructure.Sorters;

public class CitySorter : ISorter<City>
{
    private SortingEnum sortingState;

    public CitySorter(SortingEnum sortingState)
    {
        this.sortingState = sortingState;
    }

    public IQueryable<City> ApplySorting(IQueryable<City> cities)
    {
        switch (sortingState)
        {
            case SortingEnum.CityAsc:
                cities = cities.OrderBy(c => c.CityTitle_en);
                break;
            case SortingEnum.CountryAsc:
                cities = cities.OrderBy(c => c.Country.CountryTitle_en);
                break;

            case SortingEnum.CityDesc:
                cities = cities.OrderByDescending(c => c.CityTitle_en);
                break;
            case SortingEnum.CountryDesc:
                cities = cities.OrderByDescending(c => c.Country.CountryTitle_en);
                break;
        }

        return cities;
    }
}