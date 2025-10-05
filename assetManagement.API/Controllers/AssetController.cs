using Microsoft.AspNetCore.Mvc;
using assetManagement.API.Interfaces;
using assetManagement.API.Models;

namespace assetManagement.API.Controllers
{
    [ApiController]
    [Route("api/asset")]
    public class AssetController : ControllerBase
    {
        private readonly IAssetRepo _repo;

        public AssetController(IAssetRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IEnumerable<AssetModel>> GetAssets()
        {
            return await _repo.GetAllAssetsAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AssetModel>> GetById(int id)
        {
            var c = await _repo.GetAssetByIdAsync(id);
            if (c is null) return NotFound();
            return Ok(c);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsse(AssetModel asset)
        {
            await _repo.AddAsset(asset);
            return Ok();
        }
    }
}
