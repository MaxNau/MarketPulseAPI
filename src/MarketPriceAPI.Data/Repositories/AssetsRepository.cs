using MarketPriceAPI.Data.Contexts;
using MarketPriceAPI.Data.Entities;
using MarketPulseAPI.Interfaces.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarketPriceAPI.Data.Repositories
{
    public class AssetsRepository : IAssetsRepository
    {
        private readonly MarketPulseAPIDbContext _context;

        public AssetsRepository(MarketPulseAPIDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Asset>> GetAllAsync()
        {
            return await _context.Assets.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Guid>> GetAllIdsAsync()
        {
            return await _context.Assets.AsNoTracking().Select(a => a.Id).ToListAsync();
        }

        public async Task<Asset?> GetByIdAsync(Guid id)
        {
            return await _context.Assets.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Asset asset)
        {
            await _context.Assets.AddAsync(asset);
            await _context.SaveChangesAsync();
        }

        public async Task AddManyAsync(IEnumerable<Asset> assets)
        {
            await _context.Assets.AddRangeAsync(assets);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateManyAsync(IEnumerable<Asset> assets)
        {
            _context.Assets.UpdateRange(assets);
            await _context.SaveChangesAsync();
        }
    }
}
