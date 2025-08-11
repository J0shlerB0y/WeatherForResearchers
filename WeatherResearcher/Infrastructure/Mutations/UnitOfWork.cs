using Domain;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using UseCases.Mutations;
using UseCases.RepoSpecifications;
namespace Infrastructure.Mutations
{
    public class UnitOfWork<T> : IDisposable where T : class, IEntity
    {
        private readonly MySqlDbContext context;
        public IReadWithAdditionalToolsRepository<T> readRepository { get; protected set; }
        public IWriteRepository<T> writeRepository { get; protected set; }

        public UnitOfWork(MySqlDbContext context, IIncludesSpec<T> spec)
        {
            this.context = context;

            readRepository = new MySqlRepository<T>(spec, context);
            writeRepository = new MySqlRepository<T>(spec, context);
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
