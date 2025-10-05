using Microsoft.AspNetCore.Mvc;
using assetManagement.API.Interfaces;
using assetManagement.API.Models;

namespace assetManagement.API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo _repo;

        public CategoryController(ICategoryRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryModel>> GetCategories()
        {
            return await _repo.GetAllCategoriesAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryModel>> GetById(int id)
        {
            var c = await _repo.GetCategoryByIdAsync(id);
            if (c is null) return NotFound();
            return Ok(c);
        }

        [HttpPost]
        public async Task<ActionResult> AddCate(CategoryModel category)
        {
            await _repo.AddCategory(category);
            return Ok();
        }
    }
}
