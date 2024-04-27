﻿using Bogus;
using Microsoft.EntityFrameworkCore;
using CCA.Core.Application.Interfaces.Persistence;
using CCA.Core.Domain.Models.Cruds.Repo;
using CCA.Data.Persistence.Repositories.Common;
using CCA.Core.Infra.Models.Search;
using CCA.Data.Persistence.Config.DbContexts;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;

namespace CCA.Data.Persistence.Repositories
{
  public class CrudRepository : EFCoreRepository<CrudEntity>, ICrudRepository
  {
    readonly ILogger<CrudRepository> _logger;
    public CrudRepository(ILogger<CrudRepository> logger, CrudContext dbContext) : base(logger, dbContext)
    {
      _logger = logger;
    }


    public async Task<IReadOnlyList<CrudEntity>> Read(Paging paging = default, DateRange dateRange = default)
    {
      var results = await _dbContext.Cruds
        .Take(paging.CountPer)
        .Where(c =>
          (c.CreatedDate >= dateRange.From || c.LastModifiedDate >= dateRange.From)
          && (c.CreatedDate <= dateRange.Until || c.LastModifiedDate <= dateRange.Until)
        ).ToListAsync();

      return results;
    }

    public async Task<int> Delete(int id)
    {
      var entity = await _dbContext.Cruds.FindAsync(id);
      if (entity == null)
      {
        return 0;
      }

      _dbContext.Cruds.Remove(entity);
      return await _dbContext.SaveChangesAsync();
    }



  }
}
