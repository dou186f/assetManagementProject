using assetManagement.API.Models;

namespace assetManagement.API.Interfaces
{
    public interface ICategoryRepo
    {
        Task<IEnumerable<CategoryModel>> GetAllCategoriesAsync();
        Task<CategoryModel?> GetCategoryByIdAsync(int id);
        Task AddCategory(CategoryModel category);
    }
}
