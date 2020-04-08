using System;

using System.IO;
using System.Linq;
using EFCoreConApp.Models;
using Microsoft.Data.SqlClient;
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

      #region Raw SQL
      #region Regular Query
      // var result = context.Customers.FromSqlRaw($"Select * from Customers where Country='UK'").ToList();
      // var country = "USA";
      // var result = context.Customers.FromSqlRaw($"Select * from Customers where Country='{country}'").ToList();
      #endregion

      #region SQL Parameter
      // var country = new SqlParameter("@country", "USA");
      // var result = context.Customers.FromSqlRaw($"Select * from Customers where Country=@country", country).ToList();
      #endregion

      #endregion

      #region Get All Customers
      //var result = context.Customers.ToList();
      #endregion

      #region Get all Customers from London
      // var result = context.Customers.Where(c => c.City == "London").ToList();

      // var result = context.Customers
      // .Where(c => c.City == "London")
      // .Select(c => new { c.CustomerID, c.ContactName, c.City })
      // .ToList();

      // Query Expression
      // var result = (from c in context.Customers
      //               where c.City == "London"
      //               select new { c.CustomerID, c.CompanyName, c.City }).ToList();
      #endregion

      #region Get all Orders for the Customer with the CustomerID as 'ALFKI'
      // var cust = context.Customers.FirstOrDefault(c => c.CustomerID == "ALFKI");
      // WriteLine($"Cust -> {cust}");
      // var result = context.Orders.Where(o => o.CustomerID == "ALFKI").ToList();
      #endregion

      #region Get all orders for customer with customerID as 'ALFKI' along with the companyname
      // var result = (from o in context.Orders
      //               join c in context.Customers
      //               on o.CustomerID equals c.CustomerID
      //               where o.CustomerID == "ALFKI"
      //               select new { o.CustomerID, o.ShipCity, c.City, c.CompanyName }).ToList();

      // var result = context.Orders
      // .Join(context.Customers, o => o.CustomerID, c => c.CustomerID, (o, c) => new { o.OrderID, o.OrderDate, c.CustomerID, c.CompanyName })
      // .Where(o => o.CustomerID == "ALFKI").ToList();

      #endregion

      #region Get all Customers from London along with their no of Orders { CustID='ID', NofoOrder=XX }

      // var result = (from c in context.Customers
      //               where c.City == "London"
      //               join o in context.Orders
      //               on c.CustomerID equals o.CustomerID
      //               group o by c.CustomerID into grp
      //               select new { CustID = grp.Key, NoOfOrders = grp.Count() }).ToList();

      // var result = context.Customers
      //               .Where(c => c.City == "London")
      //               .Join(context.Orders, c => c.CustomerID, o => o.CustomerID, (c, o) => new { c, o })
      //               .GroupBy(c => c.c.CustomerID, o => o.o)
      //               .Select(g => new { CustID = g.Key, NoOfOrders = g.Count() }).ToList();

      #endregion
      // int ctr = 1;
      // foreach (var item in result)
      // {
      //   WriteLine($"{ctr++} - {item}");
      // }

    }
  }
}
