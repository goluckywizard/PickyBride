using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PickyBride.Data;
using PickyBride.Data.Entities;
using pickyPride2;

namespace TestBride;

[TestFixture]
public class InMemory_PrincessServiceTests
{
    private readonly EnvironmentContext _context;
    public InMemory_PrincessServiceTests()
    {
        var options = new DbContextOptionsBuilder<EnvironmentContext>()
            .UseInMemoryDatabase(databaseName: "EnvironmentDatabase")
            .Options;
        _context = new EnvironmentContext(options);
    }

    /*[SetUp]
    public void PrepareDatabase()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }*/

    [Test]
    public void Check_Reproducing_Attempt()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _context.ChangeTracker.Clear();
        var repository = new ContenderRepository(_context);
        for (int i = 0; i < 100; i++)
        {
            Contender c = new Contender("", i+1);
            c.Attempt = 0;
            c.Order = i;
            _context.Contenders.Add(c);
            _context.SaveChanges();
        }
        
        IServiceCollection services = new ServiceCollection();
        services.AddScoped<IContenderGenerator, SimpleContenderGenerator>();
        services.AddScoped<IHall, Hall>();
        services.AddScoped<IFriend, SimpleFriend>();
        services.AddScoped<IResultSaver, FileResultSaver>();
        services.AddScoped<ContenderRepository>((sp) => repository);
        services.AddHostedService<Princess>();
        var serviceProvider = services.BuildServiceProvider();

        var princess = serviceProvider.GetRequiredService<IHostedService>() as Princess;
        Assert.AreEqual(princess.ReproduceAttempt(0).Rating, 51);
    }

    [Test]
    public void Check_Generation_Attempts()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _context.ChangeTracker.Clear();
        var repository = new ContenderRepository(_context);
        IServiceCollection services = new ServiceCollection();
        services.AddScoped<IContenderGenerator, SimpleContenderGenerator>();
        services.AddScoped<IHall, Hall>();
        services.AddScoped<IFriend, SimpleFriend>();
        services.AddScoped<IResultSaver, FileResultSaver>();
        services.AddScoped<ContenderRepository>((sp) => repository);
        services.AddHostedService<Princess>();
        var serviceProvider = services.BuildServiceProvider();

        var princess = serviceProvider.GetRequiredService<IHostedService>() as Princess;
        princess.GenerateContenders();
        Assert.AreEqual(_context.Contenders.Count(), 100*100);
    }
}