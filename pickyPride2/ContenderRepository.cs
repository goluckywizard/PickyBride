using PickyBride.Data;
using PickyBride.Data.Entities;

namespace pickyPride2;

public class ContenderRepository
{
    private readonly EnvironmentContext _context;
    public ContenderRepository(EnvironmentContext context)
    {
        try
        {
            Console.WriteLine("constructor");
            _context = context;
            //_context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }
    }

    public void SaveContenders(Contender[] contenders, int attemptId)
    {
        _context.ChangeTracker.Clear();
        for (int i = 0; i < 100; ++i)
        {
            contenders[i].Order = i;
            contenders[i].Attempt = attemptId;
            _context.Add(contenders[i]);
            _context.SaveChanges();
        }
    }

    public Contender[] GetContendersByAttempt(int attempt)
    {
        return _context.Contenders.Where(c => c.Attempt == attempt).ToArray();
    }

    public int? getMaxAttempt()
    {
        return _context.Contenders.Max(c => c.Attempt).Value;
    }
    public void DeleteAll()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }
}