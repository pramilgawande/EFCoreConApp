using EFCoreConApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCoreConApp.Models
{
  public class EFDbContext : DbContext
  {

    static readonly ILoggerFactory _logger = LoggerFactory.Create(builder => { builder.AddConsole(); });
    public EFDbContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseLoggerFactory(_logger).EnableSensitiveDataLogging();
    }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
  }
}
