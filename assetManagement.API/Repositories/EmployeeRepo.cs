using assetManagement.API.Interfaces;
using assetManagement.API.Data;
using assetManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace assetManagement.API.Repositories
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly DataContext _context;

        public EmployeeRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync()
        {
            return await _context.employees.ToListAsync();
        }

        public async Task<EmployeeModel?> GetEmployeeByIdAsync(int id)
        {
            return await _context.employees.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task AddEmployee(EmployeeModel employee)
        {
            await _context.employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AssignToDepartmentAsync(int employeeId, int departId, bool allowReassign = false)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            var employee = await _context.employees.FirstOrDefaultAsync(x => x.id == employeeId);
            if (employee is null) throw new Exception("No Employee Found");

            var departman = await _context.departments.FirstOrDefaultAsync(x => x.id == departId);
            if (departman is null) throw new KeyNotFoundException("No Department Found");

            if (employee.departId == departId) return false;

            if (employee.departId != null && employee.departId != departId)
            {
                if (!allowReassign)
                    throw new InvalidOperationException("This employee already works in another department.");
            }
            employee.departId = departId;

            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return true;
        }

        public async Task<bool> UnassignFromDepartmentAsync(int employeeId)
        {
            var employee = await _context.employees.FirstOrDefaultAsync(x => x.id == employeeId);
            if (employee is null) throw new KeyNotFoundException("No Employee Found");

            if (employee.departId is null) return false;

            employee.departId = null;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}