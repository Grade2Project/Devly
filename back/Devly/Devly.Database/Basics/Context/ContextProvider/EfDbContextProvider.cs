using Microsoft.EntityFrameworkCore;

namespace Devly.Database.Basics.Context.ContextProvider;

internal class EfDbContextProvider<TContext> : IDbContextProvider<TContext> where TContext :EfDbContext
{
    private readonly IDbContextFactory<TContext> _factory;

    public EfDbContextProvider(IDbContextFactory<TContext> factory)
    {
        _factory = factory;
    }

    public Task<TContext> GetContextAsync()
    {
        return _factory.CreateDbContextAsync();
    }
}