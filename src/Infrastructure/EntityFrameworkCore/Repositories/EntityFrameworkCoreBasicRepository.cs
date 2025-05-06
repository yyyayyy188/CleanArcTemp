using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using CleanArchitectureTemp.Domain.DomainShared.Contracts.Entities;
using CleanArchitectureTemp.Domain.DomainShared.Contracts.Repositories;
using CleanArchitectureTemp.Domain.DomainShared.Linq;
using CleanArchitectureTemp.Infrastructure.EntityFrameworkCore.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemp.Infrastructure.EntityFrameworkCore.Repositories;

public class EntityFrameworkCoreBasicRepository<TDbContext, TEntity> : IBasicRepository<TEntity>
    where TDbContext : BaseDbContext
    where TEntity : class, IEntity
{
    private readonly IDbContextProvider<TDbContext> _dbContextProvider;
    private TDbContext? dbContext;
    /// <summary>
    ///     Initializes a new instance of the <see cref="EntityFrameworkCoreBasicRepository{TDbContext, TEntity}" /> class.
    ///     EntityFrameworkCore Basic Repository
    /// </summary>
    /// <param name="dbContextProvider">dbContextProvider</param>
    public EntityFrameworkCoreBasicRepository(IDbContextProvider<TDbContext> dbContextProvider)
    {
        _dbContextProvider = dbContextProvider;
    }

    public async Task InitDbContextAsync()
    {
        dbContext = await _dbContextProvider.GetOrCreateDbContextAsync();
    }   
    /// <summary>
    /// / Execute a function with the current DbContext or create a new one if it doesn't exist.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    protected async Task<TResult> ExecuteWithDbContextAsync<TResult>(Func<TDbContext, Task<TResult>> func)
    {
        if (dbContext != null)
        {
            return await func(dbContext);
        }

        await using var baseDbContext = await _dbContextProvider.CreateDbContextAsync();
        return await func(baseDbContext);
    }
    /// <summary>
    /// Execute a function with the current DbContext or create a new one if it doesn't exist.
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    protected async Task ExecuteWithDbContextAsync(Func<TDbContext, Task> func)
    {
        if (dbContext != null)
        {
            await func(dbContext);
            return;
        }

        await using var baseDbContext = await _dbContextProvider.CreateDbContextAsync();
        await func(baseDbContext);
    }

    public async Task<TEntity?> GetByIdAsync<TKey>(TKey id) where TKey : notnull
    {
        return await ExecuteWithDbContextAsync(async baseDbContext => await baseDbContext.Set<TEntity>().FindAsync(id));
    }

    public async Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        if(predicate == null)
        {
            return true;
        }

        return await ExecuteWithDbContextAsync(async baseDbContext => await baseDbContext.Set<TEntity>().AnyAsync(predicate));
    }

    public async Task<TEntity> AddAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithDbContextAsync(async baseDbContext =>
        {
            await baseDbContext.AddAsync(entity, cancellationToken);
            await baseDbContext.SaveChangesAsync(cancellationToken);
            return entity;
        });
    }

    public async Task<int> BulkAddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithDbContextAsync(async baseDbContext => {
            await baseDbContext.AddRangeAsync(entities, cancellationToken);
            return await baseDbContext.SaveChangesAsync(cancellationToken);
        });
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return ExecuteWithDbContextAsync(async baseDbContext =>
        {
            var queryable = baseDbContext.Set<TEntity>().AsQueryable().AsNoTracking();
            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }
            return await queryable.CountAsync();
        });
    }

    public async Task<bool> DeleteAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithDbContextAsync(async baseDbContext =>
        {
            baseDbContext.Remove(entity);
            return await baseDbContext.SaveChangesAsync(cancellationToken) > 0;
        });
    }

    public async Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithDbContextAsync(async baseDbContext =>
        {
            await baseDbContext.Set<TEntity>().Where(predicate).ExecuteDeleteAsync();
            return await baseDbContext.SaveChangesAsync(cancellationToken);
        });
    }

    public async Task<int> ExecuteSqlRawAsync(string sql, CancellationToken cancellationToken = default, params object[] parameters)
    {
        return await ExecuteWithDbContextAsync(async baseDbContext => {
            var raw =  await baseDbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken, parameters);
            return raw;
        });
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, IOrderBy<TEntity, dynamic>? orderBy = null, Expression<Func<TEntity, TEntity>>? selector = null)
    {
        return await ExecuteWithDbContextAsync(async baseDbContext => {
            var queryable = baseDbContext.Set<TEntity>().Where(predicate).AsNoTracking();
            if(orderBy != null)
            {
                queryable = orderBy.IsDescending
                    ? queryable.OrderByDescending(orderBy.Expression)
                    : queryable.OrderBy(orderBy.Expression);
            }
            if(selector != null)
            {
                queryable = queryable.Select(selector);
            }
            return await queryable.FirstOrDefaultAsync();
        });
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await ExecuteWithDbContextAsync(async baseDbContext => await baseDbContext.Set<TEntity>().AsNoTracking().ToListAsync());
    }


    public async Task<List<TEntity>> PageAsync(int pageSize, int skipCount, Expression<Func<TEntity, bool>>? predicate = null, IOrderBy<TEntity, dynamic>? orderBy = null, Expression<Func<TEntity, TEntity>>? selector = null)
    {
        return await ExecuteWithDbContextAsync(async baseDbContext => {
            var queryable = baseDbContext.Set<TEntity>().AsQueryable().AsNoTracking();
            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }
            if (orderBy != null)
            {
                queryable = orderBy.IsDescending
                    ? queryable.OrderByDescending(orderBy.Expression)
                    : queryable.OrderBy(orderBy.Expression);
            }
            if (selector != null)
            {
                queryable = queryable.Select(selector);
            }
            return await queryable.Skip(skipCount).Take(pageSize).ToListAsync();
        }); 
    }

    public async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, TEntity>>? selector = null)
    {
        return await ExecuteWithDbContextAsync(async baseDbContext => {
            var queryable = baseDbContext.Set<TEntity>().AsQueryable().AsNoTracking();
            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }
            if (selector != null)
            {
                queryable = queryable.Select(selector);
            }
            return await queryable.ToListAsync();
        });
    }

    public Task<bool> UpdateAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default)
    {
        return ExecuteWithDbContextAsync(async baseDbContext => {
            baseDbContext.Update(entity);
            return await baseDbContext.SaveChangesAsync(cancellationToken) > 0;
        });
    }

    public Task<bool> UpdateAsync([NotNull] TEntity entity, Action<TEntity> updateAction, CancellationToken cancellationToken = default)
    {
        return ExecuteWithDbContextAsync(async baseDbContext => {
            baseDbContext.Attach(entity);
            updateAction(entity);
            return await baseDbContext.SaveChangesAsync(cancellationToken) > 0;
        });
    }
}