using Echo.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Echo.Data.Repositories
{
    // Generic repository implementation for basic CRUD operations.
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly EchoDbContext _context;

        public Repository(EchoDbContext context)
        {
            _context = context;
        }

        // ────── CREATE ──────

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // ────── READ ──────

        // Get by primary key (GUID)
        public async Task<T?> GetByIdAsync(Guid id) =>
            await _context.Set<T>().FindAsync(id);

        // Get by custom condition (first match)
        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate) =>
            await _context.Set<T>().FirstOrDefaultAsync(predicate);

        // Alias for conditional get (can be used for special logic like settings)
        public async Task<T?> GetByConditionAsync(Expression<Func<T, bool>> predicate) =>
            await _context.Set<T>().FirstOrDefaultAsync(predicate);

        // Get all with optional filter and include logic
        public async Task<List<T>> GetAll(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            return await query.ToListAsync();
        }

        // ────── UPDATE ──────

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        // ────── DELETE ──────

        public async Task RemoveAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}