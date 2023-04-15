using System.Linq.Expressions;
using Devly.Database.Basics.Context;

namespace Devly.Database.Basics.Repository
{
    internal interface IDbRepository<TContext> where TContext : EfDbContext
    {
        Task<TEntity?> FindAsync<TEntity>(
            Expression<Func<TEntity, bool>> condition, 
            CancellationToken token = default,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class;
        
        Task<IReadOnlyList<TEntity>> FindAllAsync<TEntity>(
            Expression<Func<TEntity, bool>> condition, 
            CancellationToken token = default,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class;

        Task InsertAsync<TEntity>(TEntity entity, CancellationToken token = default) where TEntity : class;

        Task InsertAllAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken token = default) where TEntity : class;

        Task UpdateAsync<TEntity>(
            TEntity entity, 
            CancellationToken token = default, 
            params Expression<Func<TEntity, object>>[] propsToUpdate) where TEntity : class;
    }
}