using System.Linq.Expressions;
using Devly.Database.Context;
using Devly.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Devly.Database.Basics.Context
{
    public class EfDbContext : DbContext, IDbContext
    {
        protected EfDbContext(DbContextOptions<DevlyDbContext> options) : base(options)
        {
        }

        public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class
        {
            return Set<TEntity>().AsNoTracking();
        }

        internal void UpdateInternal<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[]? propsToUpdate)
            where TEntity : class
        {
            if (propsToUpdate == null || propsToUpdate.Length == 0)
            {
                Set<TEntity>().Update(entity);
                return;
            }

            Entry(entity).State = EntityState.Unchanged;
            foreach (var prop in propsToUpdate)
            {
                Entry(entity).Property(prop).IsModified = true;
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersFavoriteLanguage>()
                .HasKey(nameof(UsersFavoriteLanguage.UserLogin),
                    nameof(UsersFavoriteLanguage.ProgrammingLanguageId));
        }
    }
}