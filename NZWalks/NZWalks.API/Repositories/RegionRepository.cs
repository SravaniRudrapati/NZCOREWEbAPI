﻿using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;
        public RegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Region> AddRegionAsync(Region region)
        {
            region.Id=Guid.NewGuid();
            await nZWalksDbContext.Regions.AddAsync(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteRegionAsync(Guid id)
        {
            var region=await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(region == null)
            {
                return null;
            }
            nZWalksDbContext.Regions.Remove(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;

        }

        public async Task<IEnumerable<Region>> GetAllRegionsAsync()
        {
            return await nZWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetRegionAsync(Guid id)
        {
            return await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);


        }

        public async Task<Region> UpdateRegionAsync(Guid id, Region region)
        {
            var existingregion = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingregion == null)
            {
                return null;
            }
            existingregion.Code = region.Code;
            existingregion.Name = region.Name;
            existingregion.Area = region.Area;
            existingregion.Lat = region.Lat;
            existingregion.Long = region.Long;
            existingregion.Population = region.Population;

            await nZWalksDbContext.SaveChangesAsync();
            return existingregion;

        }
            

    }
}
