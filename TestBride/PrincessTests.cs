using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PickyBride.Data.Entities;
using pickyPride2;

namespace TestBride;

[TestFixture]
public class PrincessTests
{
    [Test]
    public void Check_If_Highest_Contender_In_Second_Half()
    {
        Contender[] contenders = new Contender[100];
        var rand = new Random();
        var firstRatings = Enumerable.Range(1, 50).OrderBy(x => rand.Next()).ToArray();
        var secondRatings = Enumerable.Range(51, 50).OrderBy(x => rand.Next()).ToArray();
        for (int i = 0; i < 50; i++)
        {
            contenders[i] = new Contender("name", firstRatings[i]);
        }
        for (int i = 50; i < 100; i++)
        {
            contenders[i] = new Contender("name", secondRatings[i - 50]);
        }

        var stabGenerator = Mock.Of<IContenderGenerator>(fb =>
            fb.GenerateGrooms() == contenders);
        
        IServiceCollection services = new ServiceCollection();
        services.AddScoped<IContenderGenerator>(sp => stabGenerator);
        services.AddScoped<IHall, Hall>();
        services.AddScoped<IFriend, SimpleFriend>();
        services.AddScoped<IResultSaver, FileResultSaver>();
        services.AddHostedService<Princess>();
        var serviceProvider = services.BuildServiceProvider();
        var princess = serviceProvider.GetRequiredService<IHostedService>() as Princess;

        Assert.Greater(princess.ChooseContender().Rating, 10);
    }
    [Test]
    public void Check_If_Highest_Contender_In_First_Half()
    {
        Contender[] contenders = new Contender[100];
        var rand = new Random();
        var firstRatings = Enumerable.Range(1, 50).OrderBy(x => rand.Next()).ToArray();
        var secondRatings = Enumerable.Range(51, 50).OrderBy(x => rand.Next()).ToArray();
        for (int i = 0; i < 50; i++)
        {
            contenders[i] = new Contender("name", secondRatings[i]);
        }
        for (int i = 50; i < 100; i++)
        {
            contenders[i] = new Contender("name", firstRatings[i - 50]);
        }
    
        var stabGenerator = Mock.Of<IContenderGenerator>(fb =>
            fb.GenerateGrooms() == contenders);
        
        IServiceCollection services = new ServiceCollection();
        services.AddScoped<IContenderGenerator>(sp => stabGenerator);
        services.AddScoped<IHall, Hall>();
        services.AddScoped<IFriend, SimpleFriend>();
        services.AddScoped<IResultSaver, FileResultSaver>();
        services.AddHostedService<Princess>();
        var serviceProvider = services.BuildServiceProvider();
        var princess = serviceProvider.GetRequiredService<IHostedService>() as Princess;
        
        Assert.That(princess.ChooseContender() is null);
        //Assert.AreEqual( 10, princess.ChooseContender().Rating);
    }
    
    [Test]
    public void Check_Exception_When_Empty_Hall()
    {
        var stabGenerator = Mock.Of<IContenderGenerator>(fb => 
            fb.GenerateGrooms() == Array.Empty<Contender>());
        IServiceCollection services = new ServiceCollection();
        services.AddScoped<IContenderGenerator>(sp => stabGenerator);
        services.AddScoped<IHall, Hall>();
        services.AddScoped<IFriend, SimpleFriend>();
        services.AddScoped<IResultSaver, FileResultSaver>();
        services.AddHostedService<Princess>();
        var serviceProvider = services.BuildServiceProvider();
        var princess = serviceProvider.GetRequiredService<IHostedService>() as Princess;
        Assert.Throws<ContenderNotFoundException>(() => princess.ChooseContender());
    }
}