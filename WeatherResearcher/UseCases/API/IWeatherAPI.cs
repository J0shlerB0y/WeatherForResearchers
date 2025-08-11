using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.API
{
    public interface IWeatherAPI
    {
        public Weather GetWeather(City cityToFindWeather);
    }
}
