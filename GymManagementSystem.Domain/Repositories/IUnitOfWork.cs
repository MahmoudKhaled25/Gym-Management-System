using GymManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Domain.Repositories;

public interface IUnitOfWork
{
    IRepository<T> Repository<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
