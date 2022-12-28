using PickyBride.Data.Entities;

namespace pickyPride2;

public interface IHall
{
    public Queue<Contender> Contenders { get; set; }
    Contender GetNextContender();
}
