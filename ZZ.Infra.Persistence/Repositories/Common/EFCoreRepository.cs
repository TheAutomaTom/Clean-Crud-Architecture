﻿using Microsoft.EntityFrameworkCore;
using ZZ.Core.Application.Interfaces.Persistence;
using ZZ.Core.Domain.Common;
using ZZ.Infra.Persistence.Repositories.DbContexts;

namespace ZZ.Infra.Persistence.Repositories.Common
{
  public class EFCoreRepository<T> : IAsyncRepository<T> where T : AuditableEntity
  {
    protected readonly CrudContext _dbContext;

    public EFCoreRepository(CrudContext dbContext)
    {
      _dbContext = dbContext;
    }
    
    public virtual async Task<int> Create(T item)
    {
      _dbContext.Entry(item).State = EntityState.Added;
      await _dbContext.SaveChangesAsync();

      return item.Id;
    }

    public virtual async Task<T> ReadById(int id)
    {
      return await _dbContext.Set<T>().FindAsync(id);
    }

    public virtual async Task<IReadOnlyList<T>> ReadAll()
    {
      return await _dbContext.Set<T>().ToListAsync();
    }

    public virtual async Task<int> Update(T item)
    {
      _dbContext.Entry(item).State = EntityState.Modified;
      return await _dbContext.SaveChangesAsync();
    }

    public virtual async Task<int> Delete(T item)
    {
      _dbContext.Set<T>().Remove(item);
      return await _dbContext.SaveChangesAsync();

    }




  }
}
