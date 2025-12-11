using AttendanceSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturesController : ControllerBase
    {
        // ---------------- Add Lecture ----------------
        // POST: api/lectures/add
        [HttpPost("add")]
        public IActionResult AddLecture(int courseId, DateTime startTime, DateTime endTime, string room)
        {
            var result = LectureService.AddLecture(courseId, startTime, endTime, room);

            if (!result.Contains("successfully"))
                return BadRequest(new { message = result });

            return Ok(new { message = result });
        }

        // ---------------- Delete Lecture ----------------
        // DELETE: api/lectures/delete/5
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteLecture(int id)
        {
            var result = LectureService.DeleteLecture(id);

            if (!result.Contains("successfully"))
                return BadRequest(new { message = result });

            return Ok(new { message = result });
        }

        // ---------------- Update Lecture ----------------
        // PUT: api/lectures/update/5
        [HttpPut("update/{id}")]
        public IActionResult UpdateLecture(int id, int courseId, DateTime startTime, DateTime endTime, string room)
        {
            var result = LectureService.UpdateLecture(id, courseId, startTime, endTime, room);

            if (!result.Contains("successfully"))
                return BadRequest(new { message = result });

            return Ok(new { message = result });
        }

        // ---------------- Get All Lectures ----------------
        // GET: api/lectures/all
        [HttpGet("all")]
        public IActionResult GetAllLectures()
        {
            var lectures = LectureService.GetAllLectures();
            return Ok(lectures);
        }

        // ---------------- Get Lecture by ID ----------------
        // GET: api/lectures/5
        [HttpGet("{id}")]
        public IActionResult GetLectureById(int id)
        {
            var lecture = LectureService.GetLectureById(id);

            if (lecture == null)
                return NotFound(new { message = "Lecture not found" });

            return Ok(lecture);
        }
    }
}
