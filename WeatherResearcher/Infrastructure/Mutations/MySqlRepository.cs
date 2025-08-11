using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using UseCases.Builders;
using UseCases.Mutations;
using UseCases.Services;
using UseCases.RepoSpecifications;
using Domain;
using System.Linq.Expressions;

namespace Infrastructure.Mutations
{
    public class MySqlRepository<T> : IReadWithAdditionalToolsRepository<T>, IWriteRepository<T>, IDisposable
        where T : class, IEntity
    {
        private IQueryable<T> items;
        private MySqlDbContext db;
        public MySqlRepository(IIncludesSpec<T> spec, MySqlDbContext db)
        {
            this.db = db;
            items = ApplyIncludesSpec(spec);
        }

        public async Task AddAsync(T item)
        {
            await db.Set<T>().AddAsync(item);
        }

        public void Delete(ICriteriaSpec<T> spec)
        {
            db.Set<T>().Remove(ApplyCriteriaSpec(spec).First());
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await items.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public T GetByName(ICriteriaSpec<T> spec)
        {
            return ApplyCriteriaSpec(spec).FirstOrDefault();
        }

        public void Filter(ICriteriaSpec<T> spec)
        {
            items = ApplyCriteriaSpec(spec);
        }

        public void Paginate(int page, int pageSize)
        {
            items = items.Skip(page * pageSize).Take(pageSize).AsNoTracking();
        }

        public void Sort(ISorter<T> sorter)
        {
            items = sorter.ApplySorting(items);
        }

        public IEnumerable<T> GetTheRest()
        {
            return items.AsEnumerable();
        }

        public void Update(ICriteriaSpec<T> spec)
        {
            db.Set<T>().Update(ApplyCriteriaSpec(spec).First());
        }

        public int Count()
        {
            return items.Count();
        }

        private IQueryable<T> ApplyCriteriaSpec(ICriteriaSpec<T> spec)
        {
            if (spec.Criteria != null)
            {
                items = items.Where(spec.Criteria);
            }

            return items.AsNoTracking();
        }

        private IQueryable<T> ApplyIncludesSpec(IIncludesSpec<T> spec)
        {
            IQueryable<T> query = db.Set<T>();

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query.AsNoTracking();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Update(T item)
        {
            db.Set<T>().Update(item);
        }

        public void Delete(T item)
        {
            db.Set<T>().Remove(item);
        }
    }
}
