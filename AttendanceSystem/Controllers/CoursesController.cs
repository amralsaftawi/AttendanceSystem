using AttendanceSystem.Entites;
using AttendanceSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        // ================= Add Course =================
        [HttpPost("add")]
        public IActionResult AddCourse([FromBody] Course request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = CourseService.AddCourse(
                request.Name,
                request.Code,
                request.InstructorID,
                request.Level
            );

            if (result.ToLower().Contains("not") || result.ToLower().Contains("exists"))
                return BadRequest(new { message = result });

            return Ok(new { message = result });
        }

        // ================= Delete Course =================
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var result = CourseService.DeleteCourseByID(id);

            if (result.ToLower().Contains("not"))
                return NotFound(new { message = result });

            if (result.ToLower().Contains("cannot"))
                return BadRequest(new { message = result });

            return Ok(new { message = result });
        }

        // ================= Update Course =================
        [HttpPut("update/{id}")]
        public IActionResult UpdateCourse(int id, [FromBody] Course request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = CourseService.UpdateCourse(
                id,
                request.Name,
                request.Code,
                request.InstructorID,
                request.Level
            );

            if (result.ToLower().Contains("not"))
                return NotFound(new { message = result });

            if (result.ToLower().Contains("exists"))
                return BadRequest(new { message = result });

            return Ok(new { message = result });
        }

        // ================= Get All Courses =================
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var courses = CourseService.GetAllCourse();
            return Ok(courses);
        }

        // ================= Change Instructor =================
        [HttpPut("change-instructor")]
        public IActionResult ChangeCourseInstructor(int CouesrID,int InstructorID)
        {
            var result = CourseService.ChangeCourseInstructor(
                CouesrID,
                InstructorID
            );

            if (result.ToLower().Contains("not"))
                return NotFound(new { message = result });

            return Ok(new { message = result });
        }

        // ================= Get Course By ID =================
        [HttpGet("{id}")]
        public IActionResult GetCourse(int id)
        {
            var course = CourseService.GetCourseByID(id);

            if (course == null)
                return NotFound(new { message = "Course not found" });

            return Ok(course);
        }
    }


}
