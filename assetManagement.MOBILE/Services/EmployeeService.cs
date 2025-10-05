using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using assetManagement.MOBILE.Models;

namespace assetManagement.MOBILE.Services
{
    public class EmployeeService
    {
        private readonly HttpClient _http;
        public EmployeeService(HttpClient http) => _http = http;
        public async Task<EmployeeModel?> GetEmployeeByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<EmployeeModel>($"api/employee/{id}");
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<EmployeeModel>> GetEmployeesAllAsync()
        {
            return await _http.GetFromJsonAsync<List<EmployeeModel>>("api/employee") ?? new();
        }
        public async Task<DepartmentModel?> GetDepartmentByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<DepartmentModel>($"api/department/{id}");
        }

        public async Task<bool> AddAssetAsync(int employeeId, int assetId, bool reassign = false)
        {
            var url = $"api/employee/{employeeId}/asset/{assetId}?reassign={reassign.ToString().ToLower()}";
            var resp = await _http.PostAsync(url, content: null);

            if (resp.StatusCode is HttpStatusCode.NoContent or HttpStatusCode.OK) return true;
            if ((int)resp.StatusCode == 304) return false;


            if (resp.StatusCode == HttpStatusCode.Conflict)
            {
                var json = await resp.Content.ReadFromJsonAsync<JsonElement>();
                var msg = json.TryGetProperty("message", out var m) ? m.GetString() : "Conflict";
                throw new InvalidOperationException(msg ?? "Conflict");
            }
            resp.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<bool> RemoveAssetAsync(int assetId)
        {
            var resp = await _http.DeleteAsync($"api/employee/asset/{assetId}");
            if (resp.StatusCode == HttpStatusCode.NoContent) return true;
            if ((int)resp.StatusCode == 304) return false;
            resp.EnsureSuccessStatusCode();
            return true;
        }
    }
}
