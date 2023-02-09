using Microsoft.AspNetCore.Mvc;
using PickyBride.DataContracts;

namespace PickyBrideWeb.Controllers;

[ApiController]
[Route("hall")]
public class HallController
{
    private HallService _hallService;

    public HallController(HallService hallService)
    {
        _hallService = hallService;
    }
    [HttpPost("reset")]
    public void Reset(string? session)
    {
        _hallService.Reset(session);
    }

    [HttpPost("{tryId}/next")]
    public ContenderDto NextContender(int tryId, string? session)
    {
        return  _hallService.GetNextContender(tryId, session);
    }

    [HttpPost("{tryId}/select")]
    public ContenderRankDto GetRating(int tryId, string? session)
    {
        return _hallService.GetRank(tryId, session);
    }
}