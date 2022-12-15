using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<IEnumerable<Walk>> GetAllAsyncWalks();

         Task<Walk> GetAsyncWalk(Guid id);

        Task<Walk> AddAsyncWalk(Walk walk);

        Task<Walk> UpdateAsyncWalk(Guid id, Walk walk);

        Task<Walk> DeleteAsyncWalk(Guid id);
    }
}