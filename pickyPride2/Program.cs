// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PickyBride.Data;
using pickyPride2;

class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    } 
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                var root = context.Configuration;
                services.Configure<OutputFilePrefix>(root.GetSection(nameof(OutputFilePrefix)));
                services.AddOptions<OutputFilePrefix>().Bind(root.GetSection(nameof(OutputFilePrefix)));
                services.AddHostedService<Princess>();
                services.AddDbContextFactory<EnvironmentContext>(
                    options => options.UseNpgsql(@"Server=localhost;Database=PickyBrideDB;
                                User Id=postgres;Password=admin"));
                services.AddScoped<ContenderRepository>();
                services.AddScoped<IContenderGenerator, SimpleContenderGenerator>();
                services.AddScoped<IHall, Hall>();
                services.AddScoped<IFriend, SimpleFriend>();
                services.AddScoped<IResultSaver, FileResultSaver>();

            });
    }
}
