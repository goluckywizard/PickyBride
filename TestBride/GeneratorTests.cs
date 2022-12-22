using pickyPride2;

namespace TestBride;

[TestFixture]
public class GeneratorTests
{
    [Test]
    public void Check_Unique_Contenders_Name()
    {
        IContenderGenerator generator = new SimpleContenderGenerator();
        var groomsArray = Array.ConvertAll(generator.GenerateGrooms(), input => input.Name);
        var groomsList = groomsArray.ToList();
        Assert.AreEqual(groomsList.Distinct().Count(), groomsList.Count());
    }
    
}