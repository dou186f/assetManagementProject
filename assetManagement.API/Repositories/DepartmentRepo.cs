using assetManagement.API.Interfaces;
using assetManagement.API.Data;
using assetManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace assetManagement.API.Repositories
{
    public class DepartmentRepo : IDepartmentRepo
    {
        private readonly DataContext _context;

        public DepartmentRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DepartmentModel>> GetAllDepartmentsAsync()
        {
            return await _context.departments.ToListAsync();
        }

        public async Task<DepartmentModel?> GetDepartmentByIdAsync(int id)
        {
            return await _context.departments.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task AddDepartment(DepartmentModel department)
        {
            await _context.departments.AddAsync(department);
            await _context.SaveChangesAsync();
        }
    }
}
