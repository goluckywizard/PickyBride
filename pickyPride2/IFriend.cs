using PickyBride.Data.Entities;

namespace pickyPride2;

public interface IFriend
{
    public void AddToViewed(Contender contender);
    public bool CompareContenders(Contender? op1, Contender? op2);
}
