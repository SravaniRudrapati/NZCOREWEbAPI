using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;

        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            _nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<WalkDifficulty> AddWalkDifficultyAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id=Guid.NewGuid();
           await _nZWalksDbContext.WalkDifficulty.AddAsync(walkDifficulty);
            await _nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteWalkDifficultyAsync(Guid id)
        {
            var walkdifficulty = await _nZWalksDbContext.WalkDifficulty.FindAsync(id);
            if(walkdifficulty==null)
            {
                return null;
            }
            _nZWalksDbContext.WalkDifficulty.Remove(walkdifficulty);
            await _nZWalksDbContext.SaveChangesAsync();
            return walkdifficulty;

        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllWalkDifficultiesAsync()
        {
            return await _nZWalksDbContext.WalkDifficulty.ToListAsync();
           
        }

        public async Task<WalkDifficulty> GetWalkDifficultyAsync(Guid id)
        {
            var walkdifficulty=await _nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x=>x.Id==id);
            if(walkdifficulty==null)
            {
                return null;
            }
            return walkdifficulty;
        }

        public async Task<WalkDifficulty> UpdateWalkDifficultyAsync(Guid id, WalkDifficulty walkDifficulty)
        {
           var existingwalkdifficulty= await  _nZWalksDbContext.WalkDifficulty.FindAsync(id);
            if(existingwalkdifficulty==null)
            {
                return null;
            }
            existingwalkdifficulty.Code=walkDifficulty.Code;
            await _nZWalksDbContext.SaveChangesAsync();
            return existingwalkdifficulty;

        }
    }
}
