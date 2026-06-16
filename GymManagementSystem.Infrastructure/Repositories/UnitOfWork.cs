using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Repositories;
using GymManagementSystem.Infrastructure.Persistence;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ConcurrentDictionary<string, object> _repositories = new();

    public IRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T).Name;
        return (IRepository<T>)_repositories.GetOrAdd(type, _ =>
            new Repository<T>(context));
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await context.SaveChangesAsync(cancellationToken);
}
