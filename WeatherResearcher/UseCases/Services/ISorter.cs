using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Services
{
    public interface ISorter<T>
    {
        public IQueryable<T> ApplySorting(IQueryable<T> items);
    }
}
