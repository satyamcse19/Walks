using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
        public async Task<List<Walk>> GetAllAsync(String? filterOn = null, [FromQuery] string? filterQuery = null, string? sortBy = null, bool  isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var walks = _dbContext.Walks.Include("Difficulty").Include("Region");
            //filtering 
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery)) 
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks= walks.Where(x => x.Name.Contains(filterQuery));
                }
                
            }
            //sorting
            if(!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                { 
                    walks = isAscending ? walks.OrderBy(x =>x.Name): walks.OrderByDescending(x => x.Name);
                }
            }

            //pagination

            // Calculate the number of results to skip
            var skipResult = (pageNumber - 1) * pageSize;

            // Return the paginated list of walks
            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();

            //return await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();

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
            existingWalks.LengthInKm = walk.LengthInKm;
            existingWalks.WalkImageUrl = walk.WalkImageUrl;
            existingWalks.RegionId = walk.RegionId;
            existingWalks.DifficultyId = walk.DifficultyId;
            await _dbContext.SaveChangesAsync();
            return existingWalks;

        }
        public async Task<Walk> DeleteAsync(Guid id)
        {
            var walksDomainmodel = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walksDomainmodel == null)
                return null;
            _dbContext.Walks.Remove(walksDomainmodel);
            await _dbContext.SaveChangesAsync();
            return walksDomainmodel;
        }

    }
}
