namespace Devly.Database.Basics.Context.ContextProvider;

public interface IDbContextProvider<TContext> where TContext : IDbContext
{
    Task<TContext> GetContextAsync();
}