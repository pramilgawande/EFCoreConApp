using EFCoreConApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreConApp.Models
{
  public class EFDbContext : DbContext
  {
    public EFDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
  }
}
