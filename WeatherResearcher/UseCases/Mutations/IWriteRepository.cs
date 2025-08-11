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
    public interface IWriteRepository<T> : IDisposable where T : class, IEntity
    {
        Task AddAsync(T entity);
        void Update(ICriteriaSpec<T> spec);
        void Update(T item);
        void Delete(ICriteriaSpec<T> spec);
        void Delete(T item);
        Task SaveAsync();
    }
}
