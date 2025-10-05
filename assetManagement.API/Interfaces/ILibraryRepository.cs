using assetManagement.API.Dtos;

namespace assetManagement.API.Interfaces
{
    public interface ILibraryRepository
    {
        Task<IEnumerable<LibraryItemDto>> SearchAsync(
            string? q,
            string field = "All",
            string type = "All",
            int take = 200
        );
    }
}
