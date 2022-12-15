using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            _nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsyncWalk(Walk walk)
        {
            walk.Id=Guid.NewGuid();
            await _nZWalksDbContext.AddAsync(walk);
            await _nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsyncWalk(Guid id)
        {
           var walk= await _nZWalksDbContext.Walks.FindAsync(id);
            if(walk==null)
            {
                return null;
            }
            _nZWalksDbContext.Walks.Remove(walk);
            await _nZWalksDbContext.SaveChangesAsync();
            return walk;


        }

        public async Task<IEnumerable<Walk>> GetAllAsyncWalks()
        {
           return await  _nZWalksDbContext.Walks.
                Include(x=>x.Region).
                Include(x=>x.WalkDifficulty)
                .ToListAsync(); 
        }

        public async Task<Walk> GetAsyncWalk(Guid id)
        {
           return await _nZWalksDbContext.Walks.
                Include(x=>x.Region).
                Include(x=>x.WalkDifficulty).
                FirstOrDefaultAsync(x => x.Id == id);
          
        }

        public async Task<Walk> UpdateAsyncWalk(Guid id, Walk walk)
        {
            var walkexisting=await _nZWalksDbContext.Walks.FindAsync(id);
            if(walk == null)
            {
                return null;
            }
            walkexisting.Name=walk.Name;
            walkexisting.Length=walk.Length;
            walkexisting.RegionId=walk.RegionId;
            walkexisting.WalkDifficultyId=walk.WalkDifficultyId;
           await  _nZWalksDbContext.SaveChangesAsync();
            return walkexisting;

        }
    }
}
