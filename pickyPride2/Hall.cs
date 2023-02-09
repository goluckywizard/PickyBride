using System.Collections;
using Microsoft.Extensions.Hosting;
using PickyBride.Data.Entities;

namespace pickyPride2;

public class Hall : IHall
{
    private readonly IContenderGenerator _contenderGenerator;
    public Queue<Contender> Contenders { get; set; }
    public Contender? LastContender { get; set; }
    private readonly IResultSaver _resultSaver;
    public int? Attempt { get; set; }

    /*public Hall(IContenderGenerator contenderGenerator)
    {
        _contenderGenerator = contenderGenerator;
        _contenders = new Queue<Contender>(_contenderGenerator.GenerateGrooms());
    }*/

    public Hall(IContenderGenerator contenderGenerator, IResultSaver resultSaver)
    {
        _contenderGenerator = contenderGenerator;
        _resultSaver = resultSaver;
        Contender[] contenders = _contenderGenerator.GenerateGrooms();
        resultSaver.Contenders = contenders;
        Contenders = new Queue<Contender>(contenders);
    }

    public Contender GetNextContender()
    {
        try
        {
            return Contenders.Dequeue();
        }
        catch (InvalidOperationException err)
        {
            throw new ContenderNotFoundException();
        }
    }
}