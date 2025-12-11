using AttendanceSystem.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class StudentReportController : ControllerBase
{
    // GET: api/StudentReport/basic/5
    [HttpGet("basic/{studentId:int}")]
    public IActionResult GetBasicReport(int studentId)
    {
        if (studentId <= 0)
            return BadRequest("Invalid student ID");

        var report = StudentReportService.GetStudentBasicReport(studentId);

        if (report == null)
            return NotFound(new { Message = "Student not found" });

        return Ok(report);
    }


    // GET: api/StudentReport/by-course/5
    [HttpGet("by-course/{studentId:int}")]
    public IActionResult GetReportByCourse(int studentId)
    {
        if (studentId <= 0)
            return BadRequest("Invalid student ID");

        var report = StudentReportService
                        .GetStudentAttendanceReportByCourse(studentId);

        if (report == null || !report.Any())
            return NotFound(new { Message = "No course data for this student" });

        return Ok(report);
    }


    // GET: api/StudentReport/by-month/5
    [HttpGet("by-month/{studentId:int}")]
    public IActionResult GetReportByMonth(int studentId)
    {
        if (studentId <= 0)
            return BadRequest("Invalid student ID");

        var report = StudentReportService
                        .GetStudentAttendanceByMonth(studentId);

        if (report == null || !report.Any())
            return NotFound(new { Message = "No monthly data for this student" });

        return Ok(report);
    }
}
