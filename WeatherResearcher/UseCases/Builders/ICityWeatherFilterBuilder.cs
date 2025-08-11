using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Builders
{
    public interface ICityWeatherFilterBuilder<T> : ICityFilterBuilder<T>
    {
        public IFilterBuilder<T> ApplyAllFilters();

        public ICityWeatherFilterBuilder<T> FilterByWeatherCondition();

        public ICityWeatherFilterBuilder<T> FilterByTopTime();

        public ICityWeatherFilterBuilder<T> FilterByTopTemp();

        public ICityWeatherFilterBuilder<T> FilterByTopTempFeelsLike();

        public ICityWeatherFilterBuilder<T> FilterByTopTempMin();

        public ICityWeatherFilterBuilder<T> FilterByTopTempMax();

        public ICityWeatherFilterBuilder<T> FilterByTopPressure();

        public ICityWeatherFilterBuilder<T> FilterByTopHumidity();

        public ICityWeatherFilterBuilder<T> FilterByTopWindSpeed();

        public ICityWeatherFilterBuilder<T> FilterByBottomTime();

        public ICityWeatherFilterBuilder<T> FilterByBottomTemp();

        public ICityWeatherFilterBuilder<T> FilterByBottomTempFeelsLike();

        public ICityWeatherFilterBuilder<T> FilterByBottomTempMin();

        public ICityWeatherFilterBuilder<T> FilterByBottomTempMax();

        public ICityWeatherFilterBuilder<T> FilterByBottomPressure();

        public ICityWeatherFilterBuilder<T> FilterByBottomHumidity();

        public ICityWeatherFilterBuilder<Snapshot> FilterByBottomWindSpeed();
        
    }
}
