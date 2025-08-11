using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Builders
{
    public interface IFilterBuilder<T>
    {
        public void CreatBuilder(IQueryable<T> query);
        public IFilterBuilder<T> ApplyAllFilters();
        public IQueryable<T> Build();
    }
}