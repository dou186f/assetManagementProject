using assetManagement.API.Models;

namespace assetManagement.API.Interfaces
{
    public interface IAssetRepo
    {
        Task<IEnumerable<AssetModel>> GetAllAssetsAsync();
        Task<AssetModel?> GetAssetByIdAsync(int id);
        Task AddAsset(AssetModel asset);
        Task<bool> AssignToEmployeeAsync(int assetId, int employeeId, bool allowReassign, DateTime? assignedAt = null);
        Task<bool> UnassignFromEmployeeAsync(int assetId);
    }
}
