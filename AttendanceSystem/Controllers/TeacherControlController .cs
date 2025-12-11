using AttendanceSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherControlController : ControllerBase
    {
        //========================
        // 1) Open Lecture
        // POST: api/TeacherControl/open-lecture/10
        //========================
        [HttpPost("open-lecture/{lectureId}")]
        public IActionResult OpenLecture(int lectureId)
        {
            bool result = TeacherControlService.OpenLecture(lectureId);

            if (!result)
                return BadRequest(new
                {
                    success = false,
                    message = "Lecture not found or already open."
                });

            return Ok(new
            {
                success = true,
                message = "Lecture opened successfully."
            });
        }

        //========================
        // 2) Close Lecture
        // POST: api/TeacherControl/close-lecture/10
        //========================
        [HttpPost("close-lecture/{lectureId}")]
        public IActionResult CloseLecture(int lectureId)
        {
            bool result = TeacherControlService.CloseLecture(lectureId);

            if (!result)
                return BadRequest(new
                {
                    success = false,
                    message = "Lecture not open or not found."
                });

            return Ok(new
            {
                success = true,
                message = "Lecture closed successfully."
            });
        }

        //========================
        // 3) Change Lecture Room
        // PUT: api/TeacherControl/change-room/10?room=C203
        //========================
        [HttpPut("change-room/{lectureId}")]
        public IActionResult ChangeRoom(int lectureId, string room)
        {
            var result = TeacherControlService.ChangeLectureRoom(lectureId, room);

            if (!result)
                return BadRequest(new
                {
                    success = false,
                    message = "Error changing room."
                });

            return Ok(new
            {
                success = true,
                message = "Room updated successfully."
            });
        }

        //========================
        // 4) Force Register Attendance
        // POST: api/TeacherControl/force-attendance?studentId=5&lectureId=10
        //========================
        [HttpPost("force-attendance")]
        public IActionResult ForceAttendance(int studentId, int lectureId)
        {
            var result = TeacherControlService.ForceRegisterAttendance(studentId, lectureId);

            return Ok(new
            {
                success = true,
                message = result
            });
        }

        //========================
        // 5) Mark Student Absent
        // POST: api/TeacherControl/mark-absent?studentId=5&lectureId=10
        //========================
        [HttpPost("mark-absent")]
        public IActionResult MarkAbsent(int studentId, int lectureId)
        {
            var msg = TeacherControlService.MarkStudentAbsent(studentId, lectureId);

            return Ok(new
            {
                success = true,
                message = msg
            });
        }

        //========================
        // 6) Remove Attendance
        // DELETE: api/TeacherControl/remove-attendance?studentId=7&lectureId=10
        //========================
        [HttpDelete("remove-attendance")]
        public IActionResult RemoveAttendance(int studentId, int lectureId)
        {
            var result = TeacherControlService.RemoveAttendance(studentId, lectureId);

            return Ok(new
            {
                success = true,
                message = result
            });
        }

        //========================
        // 7) Mark Student Late
        // POST: api/TeacherControl/mark-late?studentId=5&lectureId=10
        //========================
        [HttpPost("mark-late")]
        public IActionResult MarkLate(int studentId, int lectureId)
        {
            var msg = TeacherControlService.MarkStudentLate(studentId, lectureId);

            return Ok(new
            {
                success = true,
                message = msg
            });
        }

        //========================
        // 8) Get Present Students
        // GET: api/TeacherControl/present-students/10
        //========================
        [HttpGet("present-students/{lectureId}")]
        public IActionResult GetPresentStudents(int lectureId)
        {
            var students = TeacherControlService.GetPresentStudents(lectureId);

            return Ok(new
            {
                success = true,
                data = students
            });
        }

        //========================
        // 9) Get Absent Students
        // GET: api/TeacherControl/absent-students/10
        //========================
        [HttpGet("absent-students/{lectureId}")]
        public IActionResult GetAbsentStudents(int lectureId)
        {
            var students = TeacherControlService.GetAbsentStudents(lectureId);

            return Ok(new
            {
                success = true,
                data = students
            });
        }

        //========================
        // 10) Block Student From Lecture
        // POST: api/TeacherControl/block-student?studentId=5&lectureId=10
        //========================
        [HttpPost("block-student")]
        public IActionResult BlockStudent(int studentId, int lectureId)
        {
            var msg = TeacherControlService.BlockStudentFromLecture(studentId, lectureId);

            return Ok(new
            {
                success = true,
                message = msg
            });
        }
    }
}
