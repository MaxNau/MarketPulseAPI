using MarketPriceAPI.Data.Entities;

namespace MarketPulseAPI.Interfaces.Data.Repositories
{
    public interface IAssetsRepository
    {
        Task<IEnumerable<Asset>> GetAllAsync();
        Task<IEnumerable<Guid>> GetAllIdsAsync();
        Task<Asset?> GetByIdAsync(Guid id);
        Task AddAsync(Asset asset);
        Task AddManyAsync(IEnumerable<Asset> assets);
        Task UpdateManyAsync(IEnumerable<Asset> assets);
    }
}
