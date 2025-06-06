﻿namespace AnyCodeHub.Domain.Abstractions;

public interface IUnitOfWork : IAsyncDisposable
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
