using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickyBride.Data.Entities;

namespace PickyBride.Data;

public class EnvironmentContext : DbContext
{
    public DbSet<Contender> Contenders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contender>((tb) => ConfigureContender(tb));
    }

    private void ConfigureContender(EntityTypeBuilder<Contender> contender)
    {
        contender.Property(c => c.Name).IsRequired();
        contender.Property(c => c.Attempt).IsRequired();
        contender.Property(c => c.Order).IsRequired();
        contender.Property(c => c.Rating).IsRequired();
    }

    public EnvironmentContext()
    {
    }

    public EnvironmentContext(DbContextOptions<EnvironmentContext> options) : base(options)
    {
    }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        /*var connectionString = @"Server=localhost;Database=PickyBrideDB;
                                User Id=postgres;Password=admin";#1#
        optionsBuilder.UseNpgsql(connectionString);
    }*/
}