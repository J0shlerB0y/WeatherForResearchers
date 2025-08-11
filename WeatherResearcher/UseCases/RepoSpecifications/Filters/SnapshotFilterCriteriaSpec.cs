
using Domain;

namespace UseCases.RepoSpecifications.Filters;

public class SnapshotFilterCriteriaSpec : BaseCriteriaSpec<Snapshot>
{
    public SnapshotFilterCriteriaSpec(User user) : base(
        c => c.User.Id == user.Id
    ){ }

    public SnapshotFilterCriteriaSpec(FilterSnapshotParametr filter) : base(c =>
                (String.IsNullOrEmpty(filter.City) || filter.City == c.City.CityTitle_en) &&
                (String.IsNullOrEmpty(filter.Country) || filter.Country == c.City.Country.CountryTitle_en) &&

                (filter.TopWeather.weather == "" || filter.TopWeather.weather == c.weather) &&

                (filter.TopWeather.Time == null || filter.TopWeather.Time <= c.Time) &&
                (filter.TopWeather.temp == null || filter.TopWeather.temp <= c.temp) &&
                (filter.TopWeather.temp_feels_like == null || filter.TopWeather.temp_feels_like <= c.temp_feels_like) &&
                (filter.TopWeather.temp_min == null || filter.TopWeather.temp_min <= c.temp_min) &&
                (filter.TopWeather.temp_max == null || filter.TopWeather.temp_max <= c.temp_max) &&
                (filter.TopWeather.pressure == null || filter.TopWeather.pressure <= c.pressure) &&
                (filter.TopWeather.humidity == null || filter.TopWeather.humidity <= c.humidity) &&
                (filter.TopWeather.wind_speed == null || filter.TopWeather.wind_speed <= c.wind_speed) &&

                (filter.BottomWeather.Time == null || filter.BottomWeather.Time >= c.Time) &&
                (filter.BottomWeather.temp == null || filter.BottomWeather.temp >= c.temp) &&
                (filter.BottomWeather.temp_feels_like == null || filter.BottomWeather.temp_feels_like >= c.temp_feels_like) &&
                (filter.BottomWeather.temp_min == null || filter.BottomWeather.temp_min >= c.temp_min) &&
                (filter.BottomWeather.temp_max == null || filter.BottomWeather.temp_max >= c.temp_max) &&
                (filter.BottomWeather.pressure == null || filter.BottomWeather.pressure >= c.pressure) &&
                (filter.BottomWeather.humidity == null || filter.BottomWeather.humidity >= c.humidity) &&
                (filter.BottomWeather.wind_speed == null || filter.BottomWeather.wind_speed >= c.wind_speed)
            )
    { }
}