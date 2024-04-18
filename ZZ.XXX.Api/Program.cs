using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using ZZ.XXX.Application.DI;
using ZZ.XXX.Config.Routing;
using ZZ.XXX.Config.Swagger;
using ZZ.XXX.Data.Config;
using ZZ.XXX.Infrastructure.DI;
using ZZ.XXX.Middleware;
using ZZ.XXX.Config;
using Serilog;

namespace ZZ.XXX
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);
      var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
      var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env}.json", optional: true
        ).Build();

      builder.Services.ConfigureLogging(config, env!);
      builder.Host.UseSerilog();

      builder.Services.AddCorsPolicy(builder.Configuration);

      // Add services to the container.
      builder.Services.AddApplicationServices();
      builder.Services.AddInfrastructureServices(builder.Configuration);
      builder.Services.AddPersistenceServices(builder.Configuration);

      builder.Services.AddControllersWithViews(o =>
      {
        o.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
      });

      builder.Services.AddEndpointsApiExplorer();

      builder.Services.AddSwagger();


      //******************************************************************************************//
      var app = builder.Build(); 

      app.UseCors(CorsConfig.Policy);

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseHttpsRedirection();

      app.UseAuthorization();

      app.MapControllers();

      app.UseCustomExceptionHandler();

      app.Run();
    }
  }
}
