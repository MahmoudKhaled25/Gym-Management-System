using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagementSystem.Domain.Repositories;

public interface IRepository<T> where T : class
{
    IQueryable<T> Query();
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);

    Task<T?> FirstOrDefaultAsync(
    Expression<Func<T, bool>> predicate,
    CancellationToken cancellationToken = default);
}
