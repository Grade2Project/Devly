using Devly.Database.Basics.Context;
using Devly.Database.Basics.Context.ContextProvider;

namespace Devly.Database.Basics.Repository;

internal class DbRepositoryBase<TContext> where TContext : IDbContext
{
    private readonly IDbContextProvider<TContext> _contextProvider;

    public DbRepositoryBase(IDbContextProvider<TContext> contextProvider)
    {
        _contextProvider = contextProvider;
    }

    protected Task<TContext> GetContextAsync(CancellationToken token = default)
    {
        return _contextProvider.GetContextAsync();
    }
}