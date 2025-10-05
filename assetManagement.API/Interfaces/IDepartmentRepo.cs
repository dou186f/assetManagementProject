using assetManagement.API.Models;

namespace assetManagement.API.Interfaces
{
    public interface IDepartmentRepo
    {
        Task<IEnumerable<DepartmentModel>> GetAllDepartmentsAsync();
        Task<DepartmentModel?> GetDepartmentByIdAsync(int id);
        Task AddDepartment(DepartmentModel depart);
    }
}
