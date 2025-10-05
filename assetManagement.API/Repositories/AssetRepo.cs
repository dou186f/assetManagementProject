using assetManagement.API.Interfaces;
using assetManagement.API.Data;
using assetManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace assetManagement.API.Repositories
{
    public class AssetRepo : IAssetRepo
    {
        private readonly DataContext _context;

        public AssetRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssetModel>> GetAllAssetsAsync()
        {
            return await _context.assets.ToListAsync();
        }

        public async Task<AssetModel?> GetAssetByIdAsync(int id)
        {
            return await _context.assets.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task AddAsset(AssetModel asset)
        {
            await _context.assets.AddAsync(asset);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AssignToEmployeeAsync(int assetId, int employeeId, bool allowReassign = false, DateTime? assignedAt = null)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            var asset = await _context.assets.FirstOrDefaultAsync(x => x.id == assetId);
            if (asset == null) throw new Exception("No Asset Found");

            var calisan = await _context.employees.FirstOrDefaultAsync(x => x.id == employeeId);
            if (calisan is null) throw new KeyNotFoundException("No Employee Found");

            if (asset.ownerId == employeeId) return false;
            if (asset.ownerId != null && asset.ownerId != employeeId)
            {
                if (!allowReassign)
                    throw new InvalidOperationException("Another employee uses this asset now.");
            }
            asset.ownerId = employeeId;
            asset.assignedAt = assignedAt ?? DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return true;
        }

        public async Task<bool> UnassignFromEmployeeAsync(int assetId)
        {
            var asset = await _context.assets.FirstOrDefaultAsync(x => x.id == assetId);
            if (asset is null) throw new KeyNotFoundException("No Asset Found");

            if (asset.ownerId is null) return false;

            asset.ownerId = null;
            asset.assignedAt = null;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}