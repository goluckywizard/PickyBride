using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace pickyPride2;

public class Princess : IHostedService
{
    private IHall? _hall;
    private IFriend? _friend;
    private IResultSaver? _resultSaver;
    
    private IServiceScopeFactory _serviceScopeFactory;
    private IHostApplicationLifetime _hostApplicationLifetime;

    public Princess(IServiceScopeFactory serviceScopeFactory, IHostApplicationLifetime hostApplicationLifetime, IOptions<OutputFilePrefix> options)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _hostApplicationLifetime = hostApplicationLifetime;
        Console.WriteLine(options.Value);
    }

    public Princess(IHall? hall, IFriend? friend, IResultSaver? resultSaver)
    {
        _hall = hall;
        _friend = friend;
        _resultSaver = resultSaver;
    }

    private const int GroomsCount = 100;

    public Contender? ChooseContender()
    {
        Contender? best = null;
        int i;
        for (i = 0; i < GroomsCount / 2; i++)
        {
            var cur = _hall.GetNextContender();
            _friend.AddToViewed(cur);
            if (best is null || _friend.CompareContenders(cur, best))
            {
                best = cur;
            }
        }
        Contender? result = null;
        Contender curGroom = _hall.GetNextContender();
        _friend.AddToViewed(curGroom);
        ++i;
        while (!_friend.CompareContenders(curGroom, best) && i < 100)
        {
            curGroom = _hall.GetNextContender();
            _friend.AddToViewed(curGroom);
            ++i;
        }
        if (_friend.CompareContenders(curGroom, best))
        {
            result = curGroom;
        }

        if (result is null)
        {
            Console.WriteLine("Null");
        }
        else
        {
            Console.WriteLine(result.Name + " " + result.Rating);
        }
        if (_resultSaver is not null)
        {
            _resultSaver.SaveResult(result);
        }
        else
        {
            Console.WriteLine("Writer is null!");
        }

        return result;
    }
    public void RunAsync()
    {
        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
        {
            _hall = scope.ServiceProvider.GetService<IHall>();
            _friend = scope.ServiceProvider.GetService<IFriend>();
            _resultSaver = scope.ServiceProvider.GetService<IResultSaver>();
            if (_hall is null || _friend is null || _resultSaver is null)
            {
                throw new ArgumentNullException();
            }

            ChooseContender();
        }
        _hostApplicationLifetime.StopApplication();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(RunAsync);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}