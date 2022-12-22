using pickyPride2;
using Moq;
namespace TestBride;

[TestFixture]
public class HallTests
{
    [Test]
    public void Check_Get_Next_Contender()
    {
        IContenderGenerator generator = new SimpleContenderGenerator();
        IResultSaver resultSaver = new FileResultSaver();
        IHall hall = new Hall(generator, resultSaver);
        
        Assert.That(hall.GetNextContender().GetType() == typeof(Contender));
    }

    [Test]
    public void Check_Exception_When_Empty_Hall()
    {
        var stabGenerator = Mock.Of<IContenderGenerator>(fb => 
            fb.GenerateGrooms() == Array.Empty<Contender>());
        IResultSaver resultSaver = new FileResultSaver();
        IHall hall = new Hall(stabGenerator, resultSaver);
        Assert.Throws<ContenderNotFoundException>(() => hall.GetNextContender());
    }
}