using System.Net.Http.Json;
using assetManagement.MOBILE.Models;

namespace assetManagement.MOBILE.Services
{
    public class AssetService
    {
        private readonly HttpClient _http;
        public AssetService(HttpClient http) => _http = http;
        public async Task<AssetModel?> GetAssetByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<AssetModel>($"api/asset/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<AssetModel>> GetAssetsAllAsync()
        {
            return await _http.GetFromJsonAsync<List<AssetModel>>("api/asset") ?? new();
        }

        public async Task<EmployeeModel?> GetCalisanByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<EmployeeModel>($"api/employee/{id}");
        }
    }
}
