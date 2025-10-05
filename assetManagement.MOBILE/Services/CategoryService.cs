using System.Net.Http.Json;
using assetManagement.MOBILE.Models;

namespace assetManagement.MOBILE.Services
{
    public class CategoryService
    {
        private readonly HttpClient _http;
        public CategoryService(HttpClient http) => _http = http;

        public async Task<List<CategoryModel>> GetAllAsync()
            => await _http.GetFromJsonAsync<List<CategoryModel>>("api/category") ?? new();
        public async Task<CategoryModel?> GetCategoryByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<CategoryModel>($"api/category/{id}");
            }
            catch
            {
                return null;
            }
        }
    }
}
