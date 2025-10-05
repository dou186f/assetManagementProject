using Microsoft.AspNetCore.Mvc;
using assetManagement.API.Interfaces;
using assetManagement.API.Models;

namespace assetManagement.API.Controllers
{
    [ApiController]
    [Route("api/department")]
    public class DepartmentController : ControllerBase
    {
        private readonly IEmployeeRepo _employeerepo;
        private readonly IDepartmentRepo _repo;
        public DepartmentController(IDepartmentRepo repo, IEmployeeRepo employeerepo)
        {
            _repo = repo;
            _employeerepo = employeerepo;
        }

        [HttpGet]
        public async Task<IEnumerable<DepartmentModel>> GetDepartments()
        {
            return await _repo.GetAllDepartmentsAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DepartmentModel>> GetById(int id)
        {
            var c = await _repo.GetDepartmentByIdAsync(id);
            if (c is null) return NotFound();
            return Ok(c);
        }

        [HttpPost]
        public async Task<ActionResult> AddDepartman(DepartmentModel department)
        {
            await _repo.AddDepartment(department);
            return Ok();
        }

        [HttpPost("{departmentId:int}/employee/{employeeId:int}")]
        public async Task<IActionResult> AddEmployeeToDepartment(int departmentId, int employeeId, bool reassign = false)
        {
            try
            {
                var changed = await _employeerepo.AssignToDepartmentAsync(employeeId, departmentId, reassign);
                return changed ? NoContent() : StatusCode(StatusCodes.Status304NotModified);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("employee/{employeeId:int}")]
        public async Task<IActionResult> RemoveEmployeeFromDepartment(int employeeId)
        {
            var changed = await _employeerepo.UnassignFromDepartmentAsync(employeeId);
            return changed ? NoContent() : StatusCode(StatusCodes.Status304NotModified);
        }
    }
}
