using Microsoft.AspNetCore.Mvc;
using PickyBride.DataContracts;

namespace PickyBrideWeb.Controllers;

[ApiController]
[Route("friend")]
public class FriendController
{
    private readonly FriendService _friendService;

    public FriendController(FriendService friendService)
    {
        _friendService = friendService;
    }
    
    [HttpPost("{tryId}/compare")]
    public ContenderDto Compare(int tryId, string? session, [FromBody] CompareDto names)
    {
        return _friendService.Compare(tryId, session, names);
    }
}