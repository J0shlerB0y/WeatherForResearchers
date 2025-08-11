using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class WeatherWithTimeAndCity : WeatherWithTime
    {
        public int CityId { get; set; }
        public City City { get; set; }
    }
}
