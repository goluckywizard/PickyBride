namespace pickyPride2;

public class SimpleFriend : IFriend
{
    private List<Contender> _viewedContenders = new List<Contender>();

    public void AddToViewed(Contender contender)
    {
        _viewedContenders.Add(contender);
    }
    public bool CompareContenders(Contender? op1, Contender? op2)
    {
        if (op1 is null)
            return false;
        if (op2 is null)
            return true;
        if (_viewedContenders.Contains(op1) && _viewedContenders.Contains(op2))
            return op1 > op2;
        throw new NotViewedContenderException();
    }
}