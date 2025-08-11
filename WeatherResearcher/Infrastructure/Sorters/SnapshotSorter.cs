using Domain;
using UseCases.Services;

namespace Infrastructure.Sorters;

public class SnapshotSorter : ISorter<Snapshot>
{
    private SortingEnum sortingState;

    public SnapshotSorter( SortingEnum sortingState)
    {
        this.sortingState = sortingState;
    }   

    public IQueryable<Snapshot> ApplySorting(IQueryable<Snapshot> snapshots)
    {
        switch (sortingState)
        {
            case SortingEnum.CityAsc:
                snapshots = snapshots.OrderBy(s => s.City.CityTitle_en);
                break;
            case SortingEnum.CountryAsc:
                snapshots = snapshots.OrderBy(s => s.City.Country.CountryTitle_en);
                break;

            case SortingEnum.TempAsc:
                snapshots = snapshots.OrderBy(s => s.temp);
                break;
            case SortingEnum.TempFeelsLikeAsc:
                snapshots = snapshots.OrderBy(s => s.temp_feels_like);
                break;
            case SortingEnum.TempMinAsc:
                snapshots = snapshots.OrderBy(s => s.temp_min);
                break;
            case SortingEnum.TempMaxAsc:
                snapshots = snapshots.OrderBy(s => s.temp_max);
                break;
            case SortingEnum.PressureAsc:
                snapshots = snapshots.OrderBy(s => s.pressure);
                break;
            case SortingEnum.HumidityAsc:
                snapshots = snapshots.OrderBy(s => s.humidity);
                break;
            case SortingEnum.WindSpeedAsc:
                snapshots = snapshots.OrderBy(s => s.wind_speed);
                break;


            case SortingEnum.CityDesc:
                snapshots = snapshots.OrderByDescending(s => s.City.CityTitle_en);
                break;
            case SortingEnum.CountryDesc:
                snapshots = snapshots.OrderByDescending(s => s.City.Country.CountryTitle_en);
                break;

            case SortingEnum.TempDesc:
                snapshots = snapshots.OrderByDescending(s => s.temp);
                break;
            case SortingEnum.TempFeelsLikeDesc:
                snapshots = snapshots.OrderByDescending(s => s.temp_feels_like);
                break;
            case SortingEnum.TempMinDesc:
                snapshots = snapshots.OrderByDescending(s => s.temp_min);
                break;
            case SortingEnum.TempMaxDesc:
                snapshots = snapshots.OrderByDescending(s => s.temp_max);
                break;
            case SortingEnum.PressureDesc:
                snapshots = snapshots.OrderByDescending(s => s.pressure);
                break;
            case SortingEnum.HumidityDesc:
                snapshots = snapshots.OrderByDescending(s => s.humidity);
                break;
            case SortingEnum.WindSpeedDesc:
                snapshots = snapshots.OrderByDescending(s => s.wind_speed);
                break;
        }

        return snapshots;
    }
}