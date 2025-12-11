using AttendanceSystem.DTOs;
using AttendanceSystem.Entites;
using AttendanceSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        // 1) Add Student
        // POST: api/Student/add
        [HttpPost("add")]
        public IActionResult AddStudent([FromBody] Student request)
        {
            var result = StudentService.AddStudent(
                request.FirstName,
                request.LastName,
                request.Gmail,
                request.DepartmentID,
                request.NFC_Tag,
                request.Level
            );

            if (!result.Contains("success"))
                return BadRequest(new { Message = result });

            return Ok(new { Message = result });
        }

        // 2) Delete Student by ID
        // DELETE: api/Student/delete/5
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var result = StudentService.DeleteStudentByID(id);

            if (!result.Contains("success"))
                return BadRequest(new { Message = result });

            return Ok(new { Message = result });
        }

        // 3) Update Student
        // PUT: api/Student/update/5
        [HttpPut("update/{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] Student request)
        {
            var result = StudentService.UpdateStudent(
                id,
                request.FirstName,
                request.LastName,
                request.Gmail,
                request.DepartmentID,
                request.NFC_Tag,
                request.Level
            );

            if (!result.Contains("success"))
                return BadRequest(new { Message = result });

            return Ok(new { Message = result });
        }

        // 4) Get All Students
        // GET: api/Student/all
        [HttpGet("all")]
        public IActionResult GetAllStudents()
        {
            var students = StudentService.GetAllStudents();
            return Ok(students);
        }

        // 5) Get Students by Level
        // GET: api/Student/by-level/3
        [HttpGet("by-level/{level}")]
        public IActionResult GetStudentsByLevel(int level)
        {
            var students = StudentService.GetAllStudentsByLevel(level);
            return Ok(students);
        }

        // 6) Get Students by Department
        // GET: api/Student/by-department/2
        [HttpGet("by-department/{departmentId}")]
        public IActionResult GetStudentsByDepartment(int departmentId)
        {
            var students = StudentService.GetAllStudentsByDepartment(departmentId);

            if (students.Count == 0)
                return NotFound(new { Message = "No students found for this department" });

            return Ok(students);
        }

        // 7) Get Student by ID
        // GET: api/Student/5
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = StudentService.GetStudentByID(id);

            if (student == null)
                return NotFound(new { Message = "Student not found" });

            return Ok(student);
        }
    }
}
