using assetManagement.API.Interfaces;
using assetManagement.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace assetManagement.API.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepo _repo;
        private readonly IAssetRepo _assetrepo;
        public EmployeeController(IEmployeeRepo repo, IAssetRepo assetrepo)
        {
            _repo = repo;
            _assetrepo = assetrepo;
        }

        [HttpGet]
        public async Task<IEnumerable<EmployeeModel>> GetEmployees()
        {
            return await _repo.GetAllEmployeesAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeModel>> GetById(int id)
        {
            var e = await _repo.GetEmployeeByIdAsync(id);
            if (e is null) return NotFound();
            return Ok(e);
        }

        [HttpPost]
        public async Task<ActionResult> AddEmplo(EmployeeModel employee)
        {
            await _repo.AddEmployee(employee);
            return Ok();
        }

        [HttpPost("{employeeId:int}/asset/{assetId:int}")]
        public async Task<IActionResult> AssignAssetTo(int employeeId, int assetId, [FromQuery] bool reassign = false, [FromQuery] DateTime? at = null)
        {
            try
            {
                var changed = await _assetrepo.AssignToEmployeeAsync(assetId, employeeId, reassign, at);
                return changed ? NoContent() : StatusCode(StatusCodes.Status304NotModified);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("asset/{assetId:int}")]
        public async Task<IActionResult> UnassignAssetFromEmployee(int assetId)
        {
            var changed = await _assetrepo.UnassignFromEmployeeAsync(assetId);
            return changed ? NoContent() : StatusCode(StatusCodes.Status304NotModified);
        }
    }
}
