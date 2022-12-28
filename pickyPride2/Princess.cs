using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PickyBride.Data.Entities;

namespace pickyPride2;

public class Princess : IPrincess
{
    private IHall _hall;
    private IFriend _friend;
    private IResultSaver _resultSaver;
    private ContenderRepository _repository;
    private IContenderGenerator _contenderGenerator;
    
    private IHostApplicationLifetime? _hostApplicationLifetime;

    /*public Princess(IServiceScopeFactory serviceScopeFactory, IHostApplicationLifetime hostApplicationLifetime, IOptions<OutputFilePrefix> options)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _hostApplicationLifetime = hostApplicationLifetime;
        Console.WriteLine(options.Value);
    }*/
    public Princess(IHall hall, IFriend friend, IResultSaver resultSaver, ContenderRepository repository, IContenderGenerator contenderGenerator, IHostApplicationLifetime hostApplicationLifetime)
    {
        _hall = hall;
        _friend = friend;
        _resultSaver = resultSaver;
        _repository = repository;
        _contenderGenerator = contenderGenerator;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    public Princess(IHall hall, IFriend friend, IResultSaver resultSaver, ContenderRepository repository, IContenderGenerator contenderGenerator)
    {
        _hall = hall;
        _friend = friend;
        _resultSaver = resultSaver;
        _repository = repository;
        _contenderGenerator = contenderGenerator;
    }
    public Princess(IHall hall, IFriend friend, IResultSaver resultSaver, IContenderGenerator contenderGenerator)
    {
        _hall = hall;
        _friend = friend;
        _resultSaver = resultSaver;
        _contenderGenerator = contenderGenerator;
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

    public void GenerateContenders()
    {
        _repository.DeleteAll();
        for (int i = 0; i < 100; i++)
        {
            _repository.SaveContenders(_contenderGenerator.GenerateGrooms(), i);
        }
    }
    public Contender? ReproduceAttempt(int attempt)
    {
        _hall.Contenders = new Queue<Contender>(_repository.GetContendersByAttempt(attempt));
        return ChooseContender();
    }

    public double GetAverageResult()
    {
        int attemptsCount = _repository.getMaxAttempt() ?? -1;
        if (attemptsCount == -1)
        {
            return -1;
        }

        int totalResults = 0;
        for (int i = 0; i < attemptsCount; i++)
        {
            totalResults += ReproduceAttempt(i)?.Rating ?? 0;
        }

        return (double)totalResults / attemptsCount;
    }
    private void RunAsync()
    {
        string input = Console.ReadLine() ?? "avg";
        if (input.Equals("generate"))
        {
            Console.WriteLine("generate");
            GenerateContenders();
        }

        if (input.Equals("avg"))
        {
            var avg = GetAverageResult();
            Console.WriteLine($"average is {avg}");
        }
        if (int.TryParse(input, out int attempt))
        {
            Console.WriteLine("reproduce");
            ReproduceAttempt(attempt);
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