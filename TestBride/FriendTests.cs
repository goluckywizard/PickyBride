using pickyPride2;

namespace TestBride;

[TestFixture]
public class FriendTests
{
    [Test]
    public void Check_Comparing_Contenders()
    {
        IFriend friend = new SimpleFriend();
        Contender c1 = new Contender("name", 2);
        Contender c2 = new Contender("name2", 1);
        friend.AddToViewed(c1);
        friend.AddToViewed(c2);
        Assert.True(friend.CompareContenders(c1, c2));
    }

    [Test]
    public void Check_Not_Viewed_Contender_Comparing()
    {
        IFriend friend = new SimpleFriend();
        Contender c1 = new Contender("name", 2);
        Contender c2 = new Contender("name2", 1);
        friend.AddToViewed(c1);
        
        Assert.Throws<NotViewedContenderException>(() => friend.CompareContenders(c1, c2));
    }
}