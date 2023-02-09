using PickyBride.DataContracts;
using pickyPride2;

namespace PickyBrideWeb;

public class FriendService
{
    private readonly SimpleFriend _friend;

    public FriendService(SimpleFriend friend)
    {
        _friend = friend;
    }
    
    public ContenderDto Compare(int tryId, string? session, CompareDto names)
    {
        var betterContender = _friend.GetBetterContender(names.Name1, names.Name2);
        return new ContenderDto
        {
            Name = betterContender.Name
        };
    }
}