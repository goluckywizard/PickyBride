using System.Collections;
using Microsoft.Extensions.Hosting;

namespace pickyPride2;

public class Hall : IHall
{
    private readonly IContenderGenerator _contenderGenerator;
    private readonly Queue<Contender> _contenders;
    private readonly IResultSaver _resultSaver;

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
        _contenders = new Queue<Contender>(contenders);
    }

    public Contender GetNextContender()
    {
        try
        {
            return _contenders.Dequeue();
        }
        catch (InvalidOperationException err)
        {
            throw new ContenderNotFoundException();
        }
    }
}