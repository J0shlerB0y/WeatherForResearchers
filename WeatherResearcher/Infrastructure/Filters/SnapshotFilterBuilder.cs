
using Domain;
using UseCases.Builders;

namespace Infrastructure.Filters
{

    public class SnapshotFilterBuilder : IFilterBuilder<Snapshot>
    {
        private IQueryable<Snapshot> snapshots;
        private readonly FilterSnapshotParametr filter;

        public SnapshotFilterBuilder(FilterSnapshotParametr filter)
        {
            this.filter = filter ?? throw new ArgumentNullException(nameof(filter));
        }


        public void CreatBuilder(IQueryable<Snapshot> query)
        {
            this.snapshots = query ?? throw new ArgumentNullException(nameof(snapshots));
        }

        public IQueryable<Snapshot> Build()
        {
            return snapshots;
        }

        public IFilterBuilder<Snapshot> ApplyAllFilters()
        {
            return this
                .FilterByCity()
                .FilterByCountry()
                .FilterByWeatherCondition()
                .FilterByTopTime()
                .FilterByTopTemp()
                .FilterByTopTempFeelsLike()
                .FilterByTopTempMin()
                .FilterByTopTempMax()
                .FilterByTopPressure()
                .FilterByTopHumidity()
                .FilterByTopWindSpeed()
                .FilterByBottomTime()
                .FilterByBottomTemp()
                .FilterByBottomTempFeelsLike()
                .FilterByBottomTempMin()
                .FilterByBottomTempMax()
                .FilterByBottomPressure()
                .FilterByBottomHumidity()
                .FilterByBottomWindSpeed();
        }

        public SnapshotFilterBuilder FilterByCity()
        {
            if (!string.IsNullOrEmpty(filter.City))
            {
                snapshots = snapshots.Where(s => filter.City == s.City.CityTitle_en);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByCountry()
        {
            if (!string.IsNullOrEmpty(filter.Country))
            {
                snapshots = snapshots.Where(s => filter.Country == s.City.Country.CountryTitle_en);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByWeatherCondition()
        {
            if (!string.IsNullOrEmpty(filter.TopWeather.weather))
            {
                snapshots = snapshots.Where(s => filter.TopWeather.weather == s.weather);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByTopTime()
        {
            if (filter.TopWeather.Time.HasValue)
            {
                snapshots = snapshots.Where(s => s.Time <= filter.TopWeather.Time.Value);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByTopTemp()
        {
            if (filter.TopWeather.temp.HasValue)
            {
                snapshots = snapshots.Where(s => s.temp <= filter.TopWeather.temp.Value);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByTopTempFeelsLike()
        {
            if (filter.TopWeather.temp_feels_like.HasValue)
            {
                snapshots = snapshots.Where(s => s.temp_feels_like <= filter.TopWeather.temp_feels_like.Value);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByTopTempMin()
        {
            if (filter.TopWeather.temp_min.HasValue)
            {
                snapshots = snapshots.Where(s => s.temp_min <= filter.TopWeather.temp_min.Value);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByTopTempMax()
        {
            if (filter.TopWeather.temp_max.HasValue)
            {
                snapshots = snapshots.Where(s => s.temp_max <= filter.TopWeather.temp_max.Value);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByTopPressure()
        {
            if (filter.TopWeather.pressure.HasValue)
            {
                snapshots = snapshots.Where(s => s.pressure <= filter.TopWeather.pressure.Value);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByTopHumidity()
        {
            if (filter.TopWeather.humidity.HasValue)
            {
                snapshots = snapshots.Where(s => s.humidity <= filter.TopWeather.humidity.Value);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByTopWindSpeed()
        {
            if (filter.TopWeather.wind_speed.HasValue)
            {
                snapshots = snapshots.Where(s => s.wind_speed <= filter.TopWeather.wind_speed.Value);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByBottomTime()
        {
            if (filter.BottomWeather.Time.HasValue)
            {
                snapshots = snapshots.Where(s => s.Time >= filter.BottomWeather.Time.Value);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByBottomTemp()
        {
            if (filter.BottomWeather.temp.HasValue)
            {
                snapshots = snapshots.Where(s => s.temp >= filter.BottomWeather.temp.Value);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByBottomTempFeelsLike()
        {
            if (filter.BottomWeather.temp_feels_like.HasValue)
            {
                snapshots = snapshots.Where(s => s.temp_feels_like >= filter.BottomWeather.temp_feels_like.Value);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByBottomTempMin()
        {
            if (filter.BottomWeather.temp_min.HasValue)
            {
                snapshots = snapshots.Where(s => s.temp_min >= filter.BottomWeather.temp_min.Value);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByBottomTempMax()
        {
            if (filter.BottomWeather.temp_max.HasValue)
            {
                snapshots = snapshots.Where(s => s.temp_max >= filter.BottomWeather.temp_max.Value);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByBottomPressure()
        {
            if (filter.BottomWeather.pressure.HasValue)
            {
                snapshots = snapshots.Where(s => s.pressure >= filter.BottomWeather.pressure.Value);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByBottomHumidity()
        {
            if (filter.BottomWeather.humidity.HasValue)
            {
                snapshots = snapshots.Where(s => s.humidity >= filter.BottomWeather.humidity.Value);
            }
            return this;
        }

        public SnapshotFilterBuilder FilterByBottomWindSpeed()
        {
            if (filter.BottomWeather.wind_speed.HasValue)
            {
                snapshots = snapshots.Where(s => s.wind_speed >= filter.BottomWeather.wind_speed.Value);
            }
            return this;
        }
    }
}