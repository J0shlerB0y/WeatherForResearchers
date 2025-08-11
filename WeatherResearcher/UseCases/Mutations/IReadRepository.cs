using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UseCases.RepoSpecifications;

namespace UseCases.Mutations
{
    public interface IReadRepository<T> : IDisposable where T : class, IEntity
    {
        IEnumerable<T> GetTheRest();
        Task<T> GetByIdAsync(int id);
        T GetByName(ICriteriaSpec<T> spec);
        int Count();
    }
}
