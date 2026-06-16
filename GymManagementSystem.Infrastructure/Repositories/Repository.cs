using GymManagementSystem.Domain.Repositories;
using GymManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Infrastructure.Repositories;

public class Repository<T>(ApplicationDbContext context)
    : IRepository<T> where T : class
{
    private readonly DbSet<T> _context = context.Set<T>();

    public IQueryable<T> Query()
       => _context.AsNoTracking();
    public async Task<T?> GetByIdAsync(int id) =>
        await _context.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _context.AsNoTracking().ToListAsync();

    public async Task AddAsync(T entity) =>
        await _context.AddAsync(entity);

    public void Update(T entity) =>
        _context.Update(entity);

    public void Delete(T entity) =>
        _context.Remove(entity);
}