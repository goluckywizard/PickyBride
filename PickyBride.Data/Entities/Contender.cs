namespace PickyBride.Data.Entities;

public class Contender
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int Rating { get; set; }
    public int? Order { get; set; }
    public int? Attempt { get; set; }

    public Contender(string name, int rating)
    {
        Name = name;
        Rating = rating;
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