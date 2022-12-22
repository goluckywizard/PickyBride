using Moq;
using pickyPride2;

namespace TestBride;

[TestFixture]
public class PrincessTests
{
    [Test]
    public void Check_If_Highest_Contender_In_Second_Half()
    {
        Contender[] contenders = new Contender[100];
        var rand = new Random();
        var firstRatings = Enumerable.Range(1, 50).OrderBy(x => rand.Next()).ToArray();
        var secondRatings = Enumerable.Range(51, 50).OrderBy(x => rand.Next()).ToArray();
        for (int i = 0; i < 50; i++)
        {
            contenders[i] = new Contender("name", firstRatings[i]);
        }
        for (int i = 50; i < 100; i++)
        {
            contenders[i] = new Contender("name", secondRatings[i - 50]);
        }

        IResultSaver resultSaver = new FileResultSaver();
        IFriend friend = new SimpleFriend();
        var stabGenerator = Mock.Of<IContenderGenerator>(fb => 
            fb.GenerateGrooms() == contenders);
        IHall hall = new Hall(stabGenerator, resultSaver);
        Princess princess = new Princess(hall, friend, resultSaver);
        Assert.Greater(princess.ChooseContender().Rating, 10);
    }
    [Test]
    public void Check_If_Highest_Contender_In_First_Half()
    {
        Contender[] contenders = new Contender[100];
        var rand = new Random();
        var firstRatings = Enumerable.Range(1, 50).OrderBy(x => rand.Next()).ToArray();
        var secondRatings = Enumerable.Range(51, 50).OrderBy(x => rand.Next()).ToArray();
        for (int i = 0; i < 50; i++)
        {
            contenders[i] = new Contender("name", secondRatings[i]);
        }
        for (int i = 50; i < 100; i++)
        {
            contenders[i] = new Contender("name", firstRatings[i - 50]);
        }

        IResultSaver resultSaver = new FileResultSaver();
        IFriend friend = new SimpleFriend();
        var stabGenerator = Mock.Of<IContenderGenerator>(fb => 
            fb.GenerateGrooms() == contenders);
        IHall hall = new Hall(stabGenerator, resultSaver);
        Princess princess = new Princess(hall, friend, resultSaver);
        Assert.That(princess.ChooseContender() is null);
        //Assert.AreEqual( 10, princess.ChooseContender().Rating);
    }

    [Test]
    public void Check_Exception_When_Empty_Hall()
    {
        IResultSaver resultSaver = new FileResultSaver();
        IFriend friend = new SimpleFriend();
        var stabGenerator = Mock.Of<IContenderGenerator>(fb => 
            fb.GenerateGrooms() == Array.Empty<Contender>());
        IHall hall = new Hall(stabGenerator, resultSaver);
        Princess princess = new Princess(hall, friend, resultSaver);
        Assert.Throws<ContenderNotFoundException>(() => princess.ChooseContender());
    }
}