using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using assetManagement.MOBILE.Models;

namespace assetManagement.MOBILE.Services
{
    public class DepartmentService
    {
        private readonly HttpClient _http;
        public DepartmentService(HttpClient http) => _http = http;

        public async Task<List<DepartmentModel>> GetAllAsync()
            => await _http.GetFromJsonAsync<List<DepartmentModel>>("api/department") ?? new();
        public async Task<DepartmentModel?> GetDepartmentByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<DepartmentModel>($"api/department/{id}");
            }
            catch
            {
                return null;
            }
        }   
        public async Task<bool> AddEmployeeAsync(int departmentId, int employeeId, bool reassign = false)
        {
            var url = $"api/department/{departmentId}/employee/{employeeId}?reassign={reassign.ToString().ToLower()}";
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

        public async Task<bool> RemoveEmployeeAsync(int employeeId)
        {
            var resp = await _http.DeleteAsync($"api/department/employee/{employeeId}");
            if (resp.StatusCode == HttpStatusCode.NoContent) return true;
            if ((int)resp.StatusCode == 304) return false;
            resp.EnsureSuccessStatusCode();
            return true;
        }
    }
}
