﻿using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using CCA.Core.Domain.Models.Cruds.Repo;
using CCA.Data.Persistence.Repositories;
using CCA.Core.Infra.Models.Common;
using CCA.Data.Persistence.Repositories.DbContexts;

namespace CCA.Data.Persistence.Repositories.DbContexts
{
  public class GeneralDbContext : DbContext
  {
    public GeneralDbContext(DbContextOptions<GeneralDbContext> options) : base(options)
    {
      try
      {
        var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
        if (databaseCreator != null)
        {
          if (!databaseCreator.CanConnect())
          {
            databaseCreator.Create();
          }
          if (!databaseCreator.HasTables())
          {
            databaseCreator.CreateTables();
          }
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public DbSet<CrudEntity> Cruds { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.EnableSensitiveDataLogging();
    }


    protected override void OnModelCreating(ModelBuilder model)
    {
      model.ApplyConfigurationsFromAssembly(typeof(GeneralDbContext).Assembly);

      model.Entity<CrudEntity>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.LastModifiedBy).IsRequired(false);
        entity.Property(e => e.LastModifiedDate).IsRequired(false);
      });

      var faker = new Faker<CrudEntity>()
        .RuleFor(s => s.Name, f => f.Commerce.ProductName())
        .RuleFor(s => s.Department, f => f.Commerce.Department());
      var fakes = faker.Generate(10);

      var i = 0;
      foreach (var fake in fakes)
      {
        fake.Id = ++i;
      }

      model.Entity<CrudEntity>().HasData(fakes);

      base.OnModelCreating(model);
    }



    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
#if DEBUG
      // This was helpful when diagnosing the "create new if no entity exists to update" issue.
      var count = ChangeTracker.Entries<Auditable>().Count();
#endif

      foreach (var entry in ChangeTracker.Entries<Auditable>())
      {
        switch (entry.State)
        {

          case EntityState.Added:
            entry.Entity.CreatedDate = DateTime.Now;
            entry.Entity.CreatedBy = nameof(CCA);
            // Initialize LastModifiedDate with CreatedDate to make the search more efficient.
            entry.Entity.LastModifiedDate = DateTime.Now;
            // Leave LastModifiedBy null to indicate that the entity has not been modified.
            //entry.Entity.LastModifiedBy = nameof(CCA);
            break;

          case EntityState.Modified:
            entry.Entity.LastModifiedDate = DateTime.Now;
            entry.Entity.LastModifiedBy = nameof(CCA);
            break;

        }
      }
      return base.SaveChangesAsync(cancellationToken);
    }



  }
}
