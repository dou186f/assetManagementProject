using Microsoft.EntityFrameworkCore;
using assetManagement.API.Models;

namespace assetManagement.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<EmployeeModel> employees {  get; set; }
        public DbSet<DepartmentModel> departments { get; set; }
        public DbSet<AssetModel> assets { get; set; }
        public DbSet<CategoryModel> categories { get; set; }
    }
}
