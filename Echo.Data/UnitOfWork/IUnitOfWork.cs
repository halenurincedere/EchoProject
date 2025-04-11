using Echo.Data.Repositories;

namespace Echo.Data.UnitOfWork
{
    // Represents a unit of work abstraction to group multiple repository operations together
    public interface IUnitOfWork
    {
        // Provides a generic repository for a specific entity type
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        // Persists all changes made in the current unit of work
        Task<int> SaveChangesAsync();
    }
}