using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Builders
{
    public interface ICityFilterBuilder<T> : IFilterBuilder<T>
    {
        public ICityFilterBuilder<T> FilterByCity();
        public ICityFilterBuilder<T> FilterByCountry();
    }
}
