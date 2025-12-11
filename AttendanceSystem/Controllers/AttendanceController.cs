using Microsoft.AspNetCore.Mvc;
using AttendanceSystem.Entites;
using AttendanceSystem.Services;

namespace AttendanceSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        [HttpPost("scan")]
        public IActionResult ScanNfc([FromBody] ScanRequest request)
        {
            if (string.IsNullOrEmpty(request.NfcTag))
                return BadRequest("NFC Tag is required");

            return Ok(AttendanceService.AttendanceRegistration(request.NfcTag));
        }
    }

    public class ScanRequest
    {
        public string NfcTag { get; set; }
    }
}
