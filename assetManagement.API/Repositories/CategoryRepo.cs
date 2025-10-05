using Microsoft.EntityFrameworkCore;
using assetManagement.API.Data;
using assetManagement.API.Interfaces;
using assetManagement.API.Models;

namespace assetManagement.API.Repositories
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly DataContext _context;

        public CategoryRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllCategoriesAsync()
        {
            return await _context.categories.ToListAsync();
        }

        public async Task<CategoryModel?> GetCategoryByIdAsync(int id)
        {
            return await _context.categories.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task AddCategory(CategoryModel category)
        {
            await _context.categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }
    }
}
