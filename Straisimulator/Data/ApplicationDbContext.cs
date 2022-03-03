using Microsoft.EntityFrameworkCore;
using Straisimulator.Data.Entities;

namespace Straisimulator.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<PresseEnt> PresseEnt { get; set; }
    public DbSet<BremaEnt> BremaEnt { get; set; }
    public DbSet<Evert1Ent> Evert1Ent { get; set; }
    public DbSet<Evert2Ent> Evert2Ent { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}