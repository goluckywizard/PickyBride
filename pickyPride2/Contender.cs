namespace pickyPride2;
public class Contender : IComparable<Contender>
{
    public string Name
    {
        get;
        set;
    }

    public Contender(string name, int rating)
    {
        Name = name;
        Rating = rating;
    }

    public int Rating
    {
        get;
        set;
    }

    public int CompareTo(Contender? obj)
    {
        if(obj is null) throw new ArgumentException("Некорректное значение параметра");
        return Rating - obj.Rating;
    }

    public static bool operator >(Contender op1, Contender op2)
    {
        return op1.Rating > op2.Rating;
    }
    public static bool operator <(Contender op1, Contender op2)
    {
        return op1.Rating < op2.Rating;
    }
}