﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZZ.Core.Application.Interfaces.Persistence;
using ZZ.Infra.Persistence.Sql;
using ZZ.Infra.Persistence.Sql.Common;
using ZZ.Infra.Persistence.Sql.DbContexts;

namespace ZZ.Infra.Persistence.Config
{
  public static class DbContextConfigs
  {
    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<XXXDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("XXXDb")));
           
      services.AddScoped(typeof(IAsyncRepository<>), typeof(BasicRepository<>));     
      services.AddScoped<IXXXRepository, XXXRepository>();

      return services;
    }
  }
}
