using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WalksAPI.Data;
using WalksAPI.Interfaces.Repositories;
using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly WalkDbContext _dbContext;
        public SQLWalkRepository(WalkDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<List<Walk>> GetAllAsync()
        {
            return await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();

        }
        public async Task<Walk> GetByIdAsync(Guid id)
        {
            var walksDomainmodel = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walksDomainmodel == null)
                return null;
            return walksDomainmodel;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _dbContext.Walks.AddAsync(walk);
            await _dbContext.SaveChangesAsync();
            return walk;
        }
        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {



        var existingWalks = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalks == null)
                return null;
            existingWalks.Name = walk.Name;
            existingWalks.Description = walk.Description;
            existingWalks.LengthInKm=walk.LengthInKm;
            existingWalks.WalkImageUrl=walk.WalkImageUrl;
            existingWalks.RegionId=walk.RegionId;
            existingWalks.DifficultyId=walk.DifficultyId;
            await _dbContext.SaveChangesAsync();
            return existingWalks;

        }
        public async Task<Walk> DeleteAsync(Guid id)
        {
            var walksDomainmodel= await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walksDomainmodel == null)
                return null;
            _dbContext.Walks.Remove(walksDomainmodel);
            await _dbContext.SaveChangesAsync();
            return walksDomainmodel;
        }

    }
}
