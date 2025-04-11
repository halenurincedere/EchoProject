using Echo.Data.Contexts;
using Echo.Data.Repositories;

namespace Echo.Data.UnitOfWork
{
    // Coordinates repository operations and commits changes as a single unit
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly EchoDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(EchoDbContext context)
        {
            _context = context;
        }

        // Returns a cached or newly created repository instance for the given entity type
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);

            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<TEntity>(_context);
            }

            return (IRepository<TEntity>)_repositories[type];
        }

        // Commits all changes made in the context
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Releases the context resources
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}