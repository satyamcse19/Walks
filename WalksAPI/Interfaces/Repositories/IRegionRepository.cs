using WalksAPI.Models.Domain;

namespace WalksAPI.Interfaces.Repositories
{
    public interface IRegionRepository
    {
        Task< List<Region>> GetAllAsync();
        Task<Region> GetByIdAsync(Guid id);
        Task<Region> CreateAsync(Region regin);
        Task<Region>UpdateAsync (Guid id, Region regin);
        Task<Region> DeleteAsync (Guid id);

    }
}
