using assetManagement.API.Models;

namespace assetManagement.API.Interfaces
{
    public interface IEmployeeRepo
    {
        Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync();
        Task<EmployeeModel?> GetEmployeeByIdAsync(int id);
        Task AddEmployee(EmployeeModel employee);
        Task<bool> AssignToDepartmentAsync(int employeeId, int departId, bool allowReassign);
        Task<bool> UnassignFromDepartmentAsync(int employeeId);
    }
}
