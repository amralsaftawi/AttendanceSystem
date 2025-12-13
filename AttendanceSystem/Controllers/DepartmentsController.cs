using AttendanceSystem.Data;
using AttendanceSystem.Services;
using AttendanceSystem.Entites;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly DepartmentService _departmentService;

        public DepartmentController(AppDbContext context)
        {
            _context = context;
            _departmentService = new DepartmentService();
        }

        // POST: api/Department
        [HttpPost("add")]
        public IActionResult AddDepartment([FromBody] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Department name is required.");

            _departmentService.AddDepartment(name, _context);
            return Ok("Department added successfully.");
        }

        // DELETE: api/Department/{id}
        [HttpDelete("delete{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            var department = _departmentService.GetDepartmentById(id, _context);
            if (department == null)
                return NotFound("Department not found.");

            _departmentService.DeleteDepartment(id, _context);
            return Ok("Department deleted successfully.");
        }

        // GET: api/Department
        [HttpGet("all")]
        public ActionResult<List<Department>> GetAllDepartments()
        {
            return Ok(_departmentService.GetAllDepartments(_context));
        }

        // GET: api/Department/{id}
        [HttpGet("getbyid{id}")]
        public ActionResult<Department> GetDepartmentById(int id)
        {
            var department = _departmentService.GetDepartmentById(id, _context);
            if (department == null)
                return NotFound();

            return Ok(department);
        }

        // GET: api/Department/search?name=IT
        [HttpGet("search")]
        public ActionResult<List<Department>> GetDepartmentByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name is required.");

            return Ok(_departmentService.GetDepartmentByName(name, _context));
        }
    }
}
