using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using CleanArchitectureTemp.Domain.DomainShared.Contracts.Entities;
using CleanArchitectureTemp.Domain.DomainShared.Linq;

namespace CleanArchitectureTemp.Domain.DomainShared.Contracts.Repositories;


public interface IBasicRepository<TEntity> : IRepository where TEntity : class, IEntity
{
    /// <summary>
    /// Get an entity by its identifier.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity?> GetByIdAsync<TKey>(TKey id) where TKey : notnull;
    
    /// <summary>
    /// get an entity is exists by its identifier.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>>? predicate = null);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity> AddAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default);
    Task<int> BulkAddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default);
    /// <summary>
    /// Update an entity with a custom update action. pre edit the entity before saving. u => u.Name = "new name" for e.g.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="updateAction"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> UpdateAsync([NotNull] TEntity entity, Action<TEntity> updateAction, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default);
    Task<int> ExecuteSqlRawAsync(string sql, CancellationToken cancellationToken = default, params object[] parameters);
    Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        IOrderBy<TEntity, dynamic>? orderBy = null,
        Expression<Func<TEntity, TEntity>>? selector = null); 

    Task<IEnumerable<TEntity>> QueryAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, TEntity>>? selector = null
    );

    Task<List<TEntity>> PageAsync(
        int pageSize,
        int skipCount,
        Expression<Func<TEntity, bool>>? predicate = null,
        IOrderBy<TEntity, dynamic>? orderBy = null,
        Expression<Func<TEntity, TEntity>>? selector = null
    );

    Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null
    );
}