using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
using assetManagement.MOBILE.Dtos;

namespace assetManagement.MOBILE.Services;

public class LibraryServices
{
    private readonly HttpClient _http;

    public LibraryServices(HttpClient http) => _http = http;

    public async Task<List<LibraryItemDto>> SearchAsync(string? q, string field = "all", string type = "all", int take = 200)
    {
        var query = new Dictionary<string, string>();

        if (!string.IsNullOrWhiteSpace(q)) query["q"] = q;
        if (!string.IsNullOrWhiteSpace(field)) query["field"] = field;
        if (!string.IsNullOrWhiteSpace(type)) query["type"] = type;

        query["take"] = Math.Clamp(take, 1, 500).ToString();

        var url = QueryHelpers.AddQueryString("api/Library/search", query);

        return await _http.GetFromJsonAsync<List<LibraryItemDto>>(url) ?? new();
    }
}
