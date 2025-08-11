using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UseCases.Builders;
using UseCases.RepoSpecifications;
using UseCases.Services;

namespace UseCases.Mutations
{
    public interface IReadWithAdditionalToolsRepository<T> : IReadRepository<T> where T : class, IEntity
    {
        void Sort(ISorter<T> sorter);
        void Filter(ICriteriaSpec<T> spec);
        void Paginate(int page, int pageSize);
    }
}
