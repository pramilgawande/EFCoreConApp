using System;

using System.IO;
using Microsoft.Extensions.Configuration;

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
    static void Main(string[] args)
    {
      IConfiguration configuration = BuildConfiguration(args);
      WriteLine(configuration["MOD"]);
      WriteLine(configuration.GetConnectionString("NorthConnection"));
    }
  }
}
