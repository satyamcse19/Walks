using Microsoft.EntityFrameworkCore;
using WalksAPI.Data;
using WalksAPI.Interfaces.Repositories;
using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories
{
    public  class SQLRegionRepository : IRegionRepository
    {
        private readonly WalkDbContext _dbcontext;
        public SQLRegionRepository(WalkDbContext dbContext)
        {
            this._dbcontext = dbContext;
        }


        public async Task<List<Region>> GetAllAsync()
        {
            return await _dbcontext.Regions.ToListAsync(); ;
        }

        public async Task<Region> GetByIdAsync(Guid id)
        {
            return await _dbcontext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> CreateAsync(Region regin)
        {
            await _dbcontext.Regions.AddAsync(regin);
            await _dbcontext.SaveChangesAsync();
            return regin;
        }

        public async Task<Region> UpdateAsync(Guid id, Region regin)
        {
            var existingRegion = await _dbcontext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null)
                return null;
            existingRegion.Code = regin.Code;
            existingRegion.Name = regin.Name;
            existingRegion.RegionImageUrl = regin.RegionImageUrl;
            await _dbcontext.SaveChangesAsync();
            return existingRegion;
        }
        public async Task<Region> DeleteAsync(Guid id)
        {
            var existingRegion = await _dbcontext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null)
                return null;
             _dbcontext.Remove(existingRegion);
            await _dbcontext.SaveChangesAsync();
            return existingRegion;
        }
      
    }
}
