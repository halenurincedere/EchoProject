using Echo.Business.Operations.Match.Dtos;
using Echo.Business.Shared;

namespace Echo.Business.Operations.Match
{
    public interface IMatchService
    {
        // Matches two users manually, based on their IDs
        Task<ServiceMessage> CreateMatchAsync(CreateMatchDto dto);

        // Returns a list of all previously created matches
        Task<List<GetMatchDto>> GetAllMatchesAsync();

        // Automatically selects two users at random and matches them
        Task<ServiceMessage> CreateRandomMatchAsync();
    }
}