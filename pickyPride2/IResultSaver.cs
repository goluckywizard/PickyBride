using PickyBride.Data.Entities;

namespace pickyPride2;

public interface IResultSaver
{
    public Contender[]? Contenders
    {
        set;
    }
    public void SaveResult(Contender? res);
}