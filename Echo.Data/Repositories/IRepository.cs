using System.Linq.Expressions;

namespace Echo.Data.Repositories
{
    // Generic repository interface – provides basic CRUD operations and custom queries.
    public interface IRepository<T> where T : class
    {
        // ────── CREATE ──────
        Task AddAsync(T entity);

        // ────── READ ──────

        // Returns a single record matching the predicate (FirstOrDefault).
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);

        // Gets a single record by primary key (GUID).
        Task<T?> GetByIdAsync(Guid id);

        // Returns a list of records with optional filter and include logic.
        // Example: repo.GetAll(f => f.IsActive, q => q.Include(x => x.Related))
        Task<List<T>> GetAll(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null);

        // Returns a single record matching a custom condition.
        Task<T?> GetByConditionAsync(Expression<Func<T, bool>> predicate);

        // ────── UPDATE ──────
        Task UpdateAsync(T entity);

        // ────── DELETE ──────
        Task RemoveAsync(T entity);
    }
}