using AttendanceSystem.Entites;
using AttendanceSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        // DTO بسيط للـ request
      

        // 1) Add Instructor
        // POST: api/Instructors/add
        [HttpPost("add")]
        public IActionResult AddInstructor([FromBody] Instructor request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.Gmail))
            {
                return BadRequest(new { message = "All fields are required" });
            }

            InstructorService.AddInstructor(
                request.FirstName,
                request.LastName,
                request.Gmail
            );

            return Created("", new { message = "Instructor added successfully" });
        }

        // 2) Delete Instructor
        // DELETE: api/Instructors/delete/5
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteInstructor(int id)
        {
            var instructor = InstructorService.GetInstructorByID(id);

            if (instructor == null)
                return NotFound(new { message = "Instructor not found" });

            InstructorService.DeleteInstructorByID(id);

            return Ok(new { message = "Instructor deleted successfully" });
        }

        // 3) Update Instructor
        // PUT: api/Instructors/update/5
        [HttpPut("update/{id}")]
        public IActionResult UpdateInstructor(int id, [FromBody] Instructor request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.Gmail))
            {
                return BadRequest(new { message = "All fields are required" });
            }

            var instructor = InstructorService.GetInstructorByID(id);

            if (instructor == null)
                return NotFound(new { message = "Instructor not found" });

            InstructorService.UpdateInstructor(
                id,
                request.FirstName,
                request.LastName,
                request.Gmail
            );

            return Ok(new { message = "Instructor updated successfully" });
        }

        // 4) Get All Instructors
        // GET: api/Instructors/all
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var instructors = InstructorService.GetAllInstructors();
            return Ok(instructors);
        }

        // 5) Get Instructor By ID
        // GET: api/Instructors/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var instructor = InstructorService.GetInstructorByID(id);

            if (instructor == null)
                return NotFound(new { message = "Instructor not found" });

            return Ok(instructor);
        }
    }
}
