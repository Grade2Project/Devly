using System.Linq.Expressions;
using Devly.Database.Basics.Context;
using Devly.Database.Basics.Context.ContextProvider;
using Devly.Database.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Devly.Database.Basics.Repository;

internal class EfDbRepository<TContext> : DbRepositoryBase<TContext>, IDbRepository<TContext> where TContext : EfDbContext
{
    public EfDbRepository(IDbContextProvider<TContext> contextProvider) : base(contextProvider)
    {
    }
    
    public async Task<TEntity?> FindAsync<TEntity>(Expression<Func<TEntity, bool>> condition,
        CancellationToken token = default, params Expression<Func<TEntity, object>>[] includes) where TEntity : class
    {
        await using var context = await GetContextAsync(token).ConfigureAwait(false);

        return await context.GetQuery<TEntity>()
            .IncludeMultiple(includes)
            .SingleOrDefaultAsync(condition, token)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<TEntity>> FindAllAsync<TEntity>(Expression<Func<TEntity, bool>> condition,
        CancellationToken token = default, params Expression<Func<TEntity, object>>[] includes) where TEntity : class
    {
        await using var context = await GetContextAsync(token).ConfigureAwait(false);

        return await context.GetQuery<TEntity>()
            .IncludeMultiple(includes)
            .Where(condition)
            .ToListAsync(token)
            .ConfigureAwait(false);
    }

    public async Task InsertAsync<TEntity>(TEntity entity, CancellationToken token = default) where TEntity : class
    {
        await using var context = await GetContextAsync(token).ConfigureAwait(false);

        context.Set<TEntity>().Add(entity);
        await context.SaveChangesAsync(token).ConfigureAwait(false);
    }

    public async Task InsertAllAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken token = default)
        where TEntity : class
    {
        await using var context = await GetContextAsync(token).ConfigureAwait(false);
        
        context.Set<TEntity>().AddRange(entities);
        await context.SaveChangesAsync(token).ConfigureAwait(false);
    }

    public async Task DeleteAsync<TEntity>(TEntity entity, CancellationToken token = default) where TEntity : class
    {
        await using var context = await GetContextAsync(token).ConfigureAwait(false);

        context.Set<TEntity>().Remove(entity);
        await context.SaveChangesAsync(token).ConfigureAwait(false);
    }

    public async Task DeleteAllAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken token = default) where TEntity : class
    {
        await using var context = await GetContextAsync(token).ConfigureAwait(false);

        context.Set<TEntity>().RemoveRange(entities);
        await context.SaveChangesAsync(token).ConfigureAwait(false);
    }

    public async Task UpdateAsync<TEntity>(TEntity entity, CancellationToken token = default,
        params Expression<Func<TEntity, object>>[] propsToUpdate) where TEntity : class
    {
        await using var context = await GetContextAsync(token).ConfigureAwait(false);

        context.UpdateInternal(entity, propsToUpdate);
        await context.SaveChangesAsync(token).ConfigureAwait(false);
    }
}