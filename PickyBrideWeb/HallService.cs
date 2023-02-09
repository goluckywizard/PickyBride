using PickyBride.Data.Entities;
using PickyBride.DataContracts;
using pickyPride2;

namespace PickyBrideWeb;

public class HallService
{
    private readonly Hall _hall;
    private readonly ContenderRepository _repository;
    private readonly SimpleFriend _friend;
    private IContenderGenerator _contenderGenerator;

    public HallService(Hall hall, ContenderRepository repository, SimpleFriend friend, IContenderGenerator contenderGenerator)
    {
        _hall = hall;
        _repository = repository;
        _friend = friend;
        _contenderGenerator = contenderGenerator;
    }

    public void Reset(string? sessionId)
    {
        _repository.DeleteAll();
        for (int i = 0; i < 100; i++)
        {
            _repository.SaveContenders(_contenderGenerator.GenerateGrooms(), i);
        }
    }
    public ContenderDto GetNextContender(int attempt, string? sessionId)
    {
        try
        {
            Console.WriteLine("hall : " + _hall.Attempt + " " + attempt);
            if (_hall.Attempt != attempt)
            {
                _hall.LastContender = null;
                _hall.Attempt = attempt;
                _hall.Contenders = new Queue<Contender>(_repository.GetContendersByAttempt(attempt));
            }

            var cur = _hall.GetNextContender();
            _hall.LastContender = cur;
            _friend.AddToViewed(cur);
            return new ContenderDto
            {
                Name = cur.Name
            };
        }
        catch (ContenderNotFoundException e)
        {
            return new ContenderDto
            {
                Name = null
            };
        }
    }

    public ContenderRankDto GetRank(int attempt, string? sessionId)
    {
        if (_hall.LastContender is null)
        {
            return new ContenderRankDto
            {
                Rank = null
            };
        }

        return new ContenderRankDto
        {
            Rank = _hall.LastContender.Rating
        };
    }
}