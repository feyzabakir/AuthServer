using System.Linq.Expressions;

namespace AuthServer.Core.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    void Remove(TEntity entity);

    TEntity Update(TEntity entity);
}
