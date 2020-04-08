using System;

using System.IO;
using System.Linq;
using EFCoreConApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static System.Console;

namespace EFCoreConApp
{
  class Program
  {
    static IConfiguration BuildConfiguration(string[] args)
    {
      IConfiguration configuration = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json", false, true)
      .AddEnvironmentVariables()
      .AddCommandLine(args)
      .Build();

      return configuration;
    }

    static ServiceProvider ConfigureService(IConfiguration configuration)
    {
      var serviceCollection = new ServiceCollection();

      serviceCollection.AddDbContext<EFDbContext>(cfg =>
      {
        cfg.UseSqlServer(configuration.GetConnectionString("NorthConnection"), sqlServerOptionsAction: sqlOptions =>
        {
          sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
        });
      });

      serviceCollection.AddSingleton<EFDbContext>();

      return serviceCollection.BuildServiceProvider();
    }

    static void Main(string[] args)
    {
      IConfiguration configuration = BuildConfiguration(args);
      ServiceProvider serviceProvider = ConfigureService(configuration);

      EFDbContext context = serviceProvider.GetService<EFDbContext>();

      #region Get All Customers
      var result = context.Customers.ToList();
      int ctr = 1;
      foreach (var item in result)
      {
        WriteLine($"{ctr++} - {item}");
      }
      #endregion

    }
  }
}
