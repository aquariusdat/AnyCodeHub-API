namespace AnyCodeHub.Domain.Abstractions;
public interface IUnitOfWorkDbContext<TContext> : IAsyncDisposable
{
    /// <summary>
    /// Call save change from dbcontext
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
