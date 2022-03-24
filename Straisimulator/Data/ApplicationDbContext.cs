using Microsoft.EntityFrameworkCore;
using Straisimulator.Data.Entities;

namespace Straisimulator.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    //public DbSet<Production> Production { get; set; }
    //public DbSet<ProductionEventLog> ProductionEventLog { get; set; }
    //public DbSet<ProductionEventTypes> ProductionEventTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}