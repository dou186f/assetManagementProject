using Microsoft.AspNetCore.Mvc;
using assetManagement.API.Dtos;
using assetManagement.API.Interfaces;

namespace assetManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryRepository _libraryRepository;
        public LibraryController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<LibraryItemDto>>> Search(
            [FromQuery] string? q,
            [FromQuery] string field = "All",
            [FromQuery] string type = "All",
            [FromQuery] int take = 200)
        {
            var list = await _libraryRepository.SearchAsync(q, field, type, take);
            return Ok(list);
        }
    }
}
