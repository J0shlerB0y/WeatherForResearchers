using Domain;
using System.Linq.Expressions;
using UseCases.Services;

namespace UseCases.RepoSpecifications.Sorters;

public class SnapshotSorterSpec : BaseSorter<Snapshot>
{
    private static Dictionary<SortingEnum, Expression<Func<Snapshot, object>>> sortingExpression =
        new()
        {
            {SortingEnum.CityAsc, s => s.City.CityTitle_en },
            { SortingEnum.CountryAsc, s => s.City.Country.CountryTitle_en },
            { SortingEnum.TempAsc, s => s.temp },
            { SortingEnum.TempFeelsLikeAsc, s => s.temp_feels_like },
            { SortingEnum.TempMinAsc, s => s.temp_min },
            { SortingEnum.TempMaxAsc, s => s.temp_max },
            { SortingEnum.PressureAsc, s => s.pressure },
            { SortingEnum.HumidityAsc, s => s.humidity },
            { SortingEnum.WindSpeedAsc, s => s.wind_speed },
            {SortingEnum.WeatherAsc, s=>s.weather },
            {SortingEnum.TimeAsc, s=>s.Time }
        };

    public SnapshotSorterSpec(SortingEnum sortingState) : base(sortingState, sortingExpression) {}
}